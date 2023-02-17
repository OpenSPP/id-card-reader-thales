using Microsoft.Extensions.Hosting.WindowsServices;
using IdCardReaderThales;

var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService()
                                     ? AppContext.BaseDirectory : default
};

var builder = WebApplication.CreateBuilder(options);

builder.Host.UseWindowsService();

var app = builder.Build();

app.MapGet("/", () => "ID Card Reader Thales");
app.MapGet("/initialise", API.Initialise);
app.MapGet("/shutdown", API.Shutdown);
app.MapGet("/readdocument", API.ReadDocument);
app.MapGet("/qrcode", API.QrCode);

app.Run();
