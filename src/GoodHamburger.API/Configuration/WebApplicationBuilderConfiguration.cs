using Infrastructure;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using Service;
using Repository;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoodHamburger.API.Configuration
{
    public static class WebApplicationBuilderConfiguration
    {
        public static WebApplicationBuilder ConfigureApplicationBuilder(this WebApplicationBuilder builder)
        {
            #region Serialisation

            builder.Services.Configure<JsonOptions>(opt =>
            {
                opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                opt.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                opt.SerializerOptions.PropertyNameCaseInsensitive = true;
                opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });

            #endregion Serialisation

            #region Swagger

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = $"GoodHamburger API",
                        Description = "An API to place and list orders to a GoodHamburger",
                        License = new OpenApiLicense
                        {
                            Name = "GoodHamburger API - License - MIT",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        },
                        TermsOfService = new Uri("https://github.com/rodolfoferreira/goodhamburger")
                    });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                options.DocInclusionPredicate((name, api) => true);
            });

            #endregion Swagger

            #region Validation

            //_ = builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            #endregion Validation

            #region Project Dependencies

            builder.Services.AddInfrastructure();
            builder.Services.AddRepositories();
            builder.Services.AddServices();

            #endregion Project Dependencies

            return builder;
        }
    }
}
