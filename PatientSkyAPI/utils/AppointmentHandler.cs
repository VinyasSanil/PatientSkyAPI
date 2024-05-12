using System;
using System.Text.Json;
public class AppointmentHandler
{
  public List<Calendar> GetCalendarRecord(Guid[] calendarIds){
    string jsonFilePath = "Data/CalendarMap.json";
    string jsonData = File.ReadAllText(jsonFilePath);
    List<Calendar> people = JsonSerializer.Deserialize<List<Calendar>>(jsonData);
    return people.Where(x => calendarIds.Contains(x.id)).Select(x => new Calendar
    {
      name = x.name,
      id = x.id
    }).ToList();
  }

  public List<AvailableTimes> GetAvailableTimes(Guid[] CalendarIds, string DateRange, int Duration, Guid? TimeSlotType)
  {
    try
    {
      var availableTimes = new List<AvailableTimes>();
      string[] parts = DateRange.Split('/');
      DateTime intervalStart = DateTime.Parse(parts[0]);
      DateTime intervalEnd = DateTime.Parse(parts[1]);

      //Shared Data has been split and stored in seperate files 
      string appointmentFilePath = "Data/Appointment.json";
      string timeSlotFilePath = "Data/TimeSlots.json";
      string appointmentJsonData = File.ReadAllText(appointmentFilePath);
      string timeSlotJsonData = File.ReadAllText(timeSlotFilePath);
      var appointmentsList = JsonSerializer.Deserialize<List<Appointments>>(appointmentJsonData);
      var timeSlotsList = JsonSerializer.Deserialize<List<TimeSlots>>(timeSlotJsonData);
      foreach(var calendarId in CalendarIds){
        var dateRangeList = new List<DateRanges>();
        var combinedTimes = new List<DateRanges>();
        foreach(var timeSlot in timeSlotsList.Where(x => x.calendar_id == calendarId && x.start >= intervalStart && x.end <= intervalEnd && (TimeSlotType == null || x.type_id == TimeSlotType))){
         bool isAvailable = true;
         if(appointmentsList.Any(x => x.calendar_id == calendarId && !(timeSlot.start >= x.end || timeSlot.end <= x.start))){
          isAvailable = false;
         }
          if(isAvailable){
            dateRangeList.Add(new DateRanges {
              startDate = timeSlot.start,
              endDate = timeSlot.end
            });
          }
        }

        //Combining Available times to create larger duration
        if(dateRangeList.Any()){
          dateRangeList = dateRangeList.OrderBy(x => x.startDate).ToList();
          DateRanges currentRange = dateRangeList[0];
          for(int i=1; i < dateRangeList.Count(); i++){
            DateRanges nextRange = dateRangeList[i];
            if(currentRange.endDate == nextRange.startDate){
              currentRange.endDate = nextRange.endDate;
              dateRangeList.RemoveAt(i);
              i--;
            }else{
              if((currentRange.endDate - currentRange.startDate).TotalMinutes > Duration){
                combinedTimes.Add(new DateRanges{
                  startDate = currentRange.startDate,
                  endDate = currentRange.endDate
                });
              }
              currentRange = nextRange;
            }
          }
          if((currentRange.endDate - currentRange.startDate).TotalMinutes > Duration){
            combinedTimes.Add(new DateRanges{
              startDate = currentRange.startDate,
              endDate = currentRange.endDate
            });
          }
          availableTimes.Add(new AvailableTimes{
          CalendarId = calendarId,
          dateRanges = combinedTimes
          });
        }
      }
      return availableTimes;
    }
    catch (Exception)
    {
      throw;
    }
  }

}