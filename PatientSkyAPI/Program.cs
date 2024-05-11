using Microsoft.AspNetCore.Mvc;
using PatientSkyAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/findAvailableTime", (AppointmentRequest request) =>
{
    AppointmentHandler appointmentHandler = new AppointmentHandler();
    DateRanges dateRanges = appointmentHandler.SplitDateRange(request.PeriodToSearch);
    List<Calendar> calendarNames = appointmentHandler.GetCalendarRecord(request.CalendarIds);
    List<CalendarAppointmentDetails> calendarAppointments = appointmentHandler.GetCalendarAppointmentDetails(request.CalendarIds, dateRanges);
    
    return Results.Ok();
});

app.Run();

