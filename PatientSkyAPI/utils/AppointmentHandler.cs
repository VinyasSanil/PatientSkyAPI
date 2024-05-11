namespace PatientSkyAPI;
using System.Text.Json;
using System.Collections.Generic;
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

  public List<CalendarAppointmentDetails> GetCalendarAppointmentDetails(Guid[] calendarIds, DateRanges dateRanges)
  {
    List<CalendarAppointmentDetails> result = [];
    string appointmentFilePath = "Data/Appointment.json";
    string timeSlotFilePath = "Data/TimeSlots.json";
    string appointmentJsonData = File.ReadAllText(appointmentFilePath);
    string timeSlotJsonData = File.ReadAllText(timeSlotFilePath);
    List<Appointments> appointments = JsonSerializer.Deserialize<List<Appointments>>(appointmentJsonData);
    List<TimeSlots> timeSlots = JsonSerializer.Deserialize<List<TimeSlots>>(timeSlotJsonData);
    // var test = from app in appointments
    //             join ts in timeSlots on app.calendar_id equals ts.calendar_id
    //             where calendarIds.Contains(app.calendar_id) && ts.start > StartupBa

    

    return result;
  }

  public DateRanges SplitDateRange(string dateRange){
     var parts = dateRange.Split('/');
     DateTimeOffset start;
     DateTimeOffset end;
     if (parts.Length == 2)
      {
        DateTimeOffset.TryParseExact(parts[0], "yyyy-MM-ddTHH:mm:ssK", null, System.Globalization.DateTimeStyles.None, out start);
        DateTimeOffset.TryParseExact(parts[1], "yyyy-MM-ddTHH:mm:ssK", null, System.Globalization.DateTimeStyles.None, out end);
        return new DateRanges{
          startDate = start,
          endDate = end
        };
      }
     return new DateRanges();
  }
}