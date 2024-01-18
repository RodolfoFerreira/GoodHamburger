using GoodHamburger.API.Endpoints;
using GoodHamburger.Infrastructure.DatabaseContext;
using System.Globalization;

namespace GoodHamburger.API.Configuration
{
    public static class WebApplicationConfiguration
    {
        public static WebApplication ConfigureApplication(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                using var scope = app.Services.CreateScope();
                using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context?.Database.EnsureCreated();
            }

            app.UseHttpsRedirection();

            #region Enpoints

            app.MapProductEndpoints();
            app.MapOrderEndpoints();

            #endregion Enpoints

            return app;
        }
    }
}
