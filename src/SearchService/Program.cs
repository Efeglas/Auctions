using Polly;
using Polly.Extensions.Http;
using SearchService.Data;
using SearchService.Services;
using System.Net;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () => 
{ 
    try
    {
	    await DbInitializer.InitDb(app);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
});

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy () 
{
    return HttpPolicyExtensions.HandleTransientHttpError().OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound).WaitAndRetryForeverAsync(_ =>TimeSpan.FromSeconds(3));
}