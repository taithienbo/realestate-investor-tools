using Core.Options;
using reit_zillow_api.JsonConverters;
using System.Net.Http.Headers;
using Serilog;
using reit_zillow_api.Configurations;


#if !DEBUG
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
#endif

try
{

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, services, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    // Add services to the container.

    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DoubleConverter());
    });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddHttpClient("Zillow", client =>
    {
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));

    }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler() { UseCookies = false });

    builder.Services.Configure<AppOptions>(builder.Configuration.GetSection(AppOptions.App));

    builder.AddServices();


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();


    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}


public partial class Program { }

