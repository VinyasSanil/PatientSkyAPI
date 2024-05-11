using Microsoft.AspNetCore.Mvc;
public class Calendar
{
    public string name { get; set; }
    public Guid id { get; set; }
}

public record AppointmentRequest([FromBody] Guid[] CalendarIds, int Duration, string PeriodToSearch, string TimeInterval, Guid TimeSlot = default);

public class CalendarAppointmentDetails:Calendar
{
  public List<Appointments> appointments {get; set;}
  public List<TimeSlots> timeslots {get; set;}
}
public class Appointments
{
  public Guid id {get; set;}
  public Guid calendar_id {get; set;}
  public DateTime start {get; set;}
  public DateTime end {get; set;}
}

public class TimeSlots
{
  public Guid id {get; set;}
  public Guid calendar_id {get; set;}
  public DateTimeOffset start {get; set;}
  public DateTimeOffset end {get; set;}
}

public class DateRanges {
  public DateTimeOffset startDate {get; set;}
  public DateTimeOffset endDate {get; set;}
}

public class AvailableTimes {
  public Guid CalendarId {get; set;}
  public List<DateRanges> dateRanges {get; set;}
}