using EasyAuth.Database;
using EasyAuth.Overrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme)
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<AppDbContext>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using IServiceScope scope = app.Services.CreateScope();
    using AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.UseHttpsRedirection();

app.MapIdentityApiFilterable<User>(new IdentityApiEndpointRouteBuilderOptions()
{
    ExcludeRegisterPost = true,
    ExcludeLoginPost = true,
    ExcludeRefreshPost = true,
    ExcludeConfirmEmailGet = true,
    ExcludeResendConfirmationEmailPost = true,
    ExcludeForgotPasswordPost = true,
    ExcludeResetPasswordPost = true,
    // setting ExcludeManageGroup to false will disable
    // 2FA and both Info Actions
    ExcludeManageGroup = true,
    Exclude2faPost = true,
    ExcludegInfoGet = true,
    ExcludeInfoPost = true,
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
