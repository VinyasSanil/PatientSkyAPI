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

  public List<AvailableTimes> GetAvailableTimes(Guid[] CalendarIds, string DateRange, int Duration)
  {
    try
    {
      var availableTimes = new List<AvailableTimes>();
      string[] parts = DateRange.Split('/');
      DateTime intervalStart = DateTime.Parse(parts[0]);
      DateTime intervalEnd = DateTime.Parse(parts[1]);
      string appointmentFilePath = "Data/Appointment.json";
      string timeSlotFilePath = "Data/TimeSlots.json";
      string appointmentJsonData = File.ReadAllText(appointmentFilePath);
      string timeSlotJsonData = File.ReadAllText(timeSlotFilePath);
      var appointmentsList = JsonSerializer.Deserialize<List<Appointments>>(appointmentJsonData);
      var timeSlotsList = JsonSerializer.Deserialize<List<TimeSlots>>(timeSlotJsonData);
      foreach(var calendarId in CalendarIds){
        var dateRangeList = new List<DateRanges>();
        foreach(var timeSlot in timeSlotsList.Where(x => x.calendar_id == calendarId && x.start >= intervalStart && x.end <= intervalEnd)){
         bool isAvailable = true;
         if(appointmentsList.Any(x => x.calendar_id == calendarId && !(timeSlot.start >= x.end || timeSlot.end <= x.start))){
          isAvailable = false;
         }
         //  foreach(var appointment in appointmentsList.Where(x => x.calendar_id == calendarId && x.start >= intervalStart && x.end <= intervalEnd)){
         //    if(!(timeSlot.start >= appointment.end || timeSlot.end <= appointment.start)){
         //      isAvailavle = false;
         //      break;
         //    }
         //  }
         if(isAvailable && (timeSlot.end - timeSlot.start).TotalMinutes > Duration){
           dateRangeList.Add(new DateRanges {
             startDate = timeSlot.start,
             endDate = timeSlot.end
           });
         }
        }
        if(dateRangeList.Any()){
          availableTimes.Add(new AvailableTimes{
          CalendarId = calendarId,
          dateRanges = dateRangeList
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

  // public DateRanges SplitDateRange(string dateRange){
  //    var parts = dateRange.Split('/');
  //    DateTimeOffset start;
  //    DateTimeOffset end;
  //    if (parts.Length == 2)
  //     {
  //       DateTimeOffset.TryParseExact(parts[0], "yyyy-MM-ddTHH:mm:ssK", null, System.Globalization.DateTimeStyles.None, out start);
  //       DateTimeOffset.TryParseExact(parts[1], "yyyy-MM-ddTHH:mm:ssK", null, System.Globalization.DateTimeStyles.None, out end);
  //       return new DateRanges{
  //         startDate = start,
  //         endDate = end
  //       };
  //     }
  //    return new DateRanges();
  // }
}