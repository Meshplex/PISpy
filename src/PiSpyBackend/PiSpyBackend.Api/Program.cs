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

app.MapPut("/pispy/api/createUser", () =>
{

});

app.MapDelete("/pispy/api/deleteUser", () =>
{

});

app.MapPost("/pispy/api/updateUser", () =>
{

});

app.MapPut("/pispy/api/activateAlarm", () =>
{

});

app.MapPut("/pispy/api/deactivateAlarm", () =>
{

});

app.MapGet("/pispy/api/getAlarmStatus", () =>
{

});

app.Run();
