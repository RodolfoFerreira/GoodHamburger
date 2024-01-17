using GoodHamburger.API.Configuration;

var builder = WebApplication.CreateBuilder(args).ConfigureApplicationBuilder();

var app = builder.Build().ConfigureApplication();

app.Run();