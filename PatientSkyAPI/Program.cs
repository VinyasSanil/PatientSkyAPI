var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/findAvailableTime", (AppointmentRequest request) =>
{
    AppointmentHandler appointmentHandler = new AppointmentHandler();
    List<AvailableTimes> AvailableTimeList = appointmentHandler.GetAvailableTimes(request.CalendarIds, request.PeriodToSearch, request.Duration, request.TimeSlotType);
    
    return AvailableTimeList;
});

app.Run();

