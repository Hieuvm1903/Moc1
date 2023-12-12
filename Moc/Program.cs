using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Moc.Data;
using Moc.DTO;
using Moc.Repos;
using Serilog;
namespace Moc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("D:log.txt", rollingInterval: RollingInterval.Day).CreateLogger();
            builder.Host.UseSerilog();
            builder.Services.AddControllers(
                option => { option.ReturnHttpNotAcceptable = true; } //This one creates  error 406

                ).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddDbContext<ApplicationContext>(option => { option.UseSqlServer(builder.Configuration.GetConnectionString("Default")); });
            builder.Services.AddScoped<IVillaRepository, VillaRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddAutoMapper(typeof(MappingConfig));
            builder.Services.AddEndpointsApiExplorer();

            var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");
            builder.Services.AddAuthentication(
                x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }

                ).AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    //x.SaveToken = true;
                    x.SaveToken = false;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    
                });

            


            //builder.Services.AddResponseCaching();
            builder.Services.AddSwaggerGen(
                options =>
                {
                    options.AddSecurityDefinition
                    ("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using Bearer scheme.\r\n\r\n" +
                        "Enter 'Bearer' [space] and your token in the text input \r\n\r\n",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Scheme = "Bearer"
                    })
                    ; options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                        }
                    }
                    );
                }
            );
            //builder.Services.AddResponseCaching();

            builder.Services.AddSingleton<ILogging, Logging>();
            //builder.Services.AddCors();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }
            //app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseResponseCaching();
            app.MapControllers();

            app.Run();
            
        }
    }
}