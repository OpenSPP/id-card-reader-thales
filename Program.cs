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

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
    context.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,OPTIONS");
    context.Response.Headers.Add("Access-Control-Allow-Headers", "Accept, Origin, Content-type");
    context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
    context.Response.Headers.Add("Pragma", "no-cache");
    context.Response.Headers.Add("Expires", "0");
    await next();
});

app.MapGet("/", () => "ID Card Reader Thales");
app.MapGet("/initialise", API.Initialise);
app.MapGet("/shutdown", API.Shutdown);
app.MapGet("/readdocument", API.ReadDocument);
app.MapGet("/qrcode", API.QrCode);

app.Run();
