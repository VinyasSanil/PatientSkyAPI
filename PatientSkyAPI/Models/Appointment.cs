using Microsoft.AspNetCore.Mvc;
public class Calendar
{
    public string name { get; set; }
    public Guid id { get; set; }
}

public record AppointmentRequest([FromBody] Guid[] CalendarIds, int Duration, string PeriodToSearch, Guid? TimeSlotType = null);

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
  public DateTime start {get; set;}
  public DateTime end {get; set;}
  public Guid type_id {get; set;}
}

public class DateRanges {
  public DateTime startDate {get; set;}
  public DateTime endDate {get; set;}
}

public class AvailableTimes {
  public Guid CalendarId {get; set;}
  public List<DateRanges> dateRanges {get; set;}
}
