namespace PatientSkyAPI;
using System.Text.Json;
using System.Collections.Generic;
public class AppointmentHandler
{
  public List<string> GetCalendarRecord(Guid[] calendarIds){
    string jsonFilePath = "CalendarData.json";
    string jsonData = File.ReadAllText(jsonFilePath);
      List<Calendar> people = JsonSerializer.Deserialize<List<Calendar>>(jsonData);
      return people.Where(x => calendarIds.Contains(x.Id)).Select(x => x.Name).ToList();
    }
}