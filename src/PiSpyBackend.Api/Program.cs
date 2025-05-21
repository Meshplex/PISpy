using System.Device.Gpio;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using PiSpyBackend.Api.ApiService;
using PiSpyBackend.Application;
using PiSpyBackend.Application.Services;
using PiSpyBackend.Domain.Interfaces;
using PiSpyBackend.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddSingleton(new JwtTokenService(
    jwtSettings["SecretKey"]!,
    jwtSettings["Issuer"]!,
    jwtSettings["Audience"]!
));

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<PictureService>();
builder.Services.AddScoped<MapKeyToUserService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MapKeyToUserService>();
builder.Services.AddSingleton<IGpioControllerService>(sp =>
{
    try
    {
        return new GpioControllerService(PinNumberingScheme.Logical);
    }
    catch
    {
        return new MockGpioControllerService();
    }
});
builder.Services.AddScoped<AlarmService>();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IEventStore, InMemoryEventStore>();
builder.Services.AddScoped<PictureService>();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactLocalhost", policy =>
    {
        policy
            .WithOrigins("http://localhost:4173", "https://localhost:5173", "http://localhost:5173", "http://localhost:5272")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactLocalhost");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<AlarmHub>("/alarmHub");
var imgFolder = Path.Combine(builder.Environment.ContentRootPath, "img");
Console.WriteLine($"Image folder: {imgFolder}");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imgFolder),
    RequestPath = "/images"
});


app.Run();
