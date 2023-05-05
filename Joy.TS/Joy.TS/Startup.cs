using Joy.TS.BAL.Implementation;
using Joy.TS.DAL.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace Joy.TS.Api
{
    //public class Startup
    //{
    //}
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Adding DbContext and connection string
            var connectionString = Configuration.GetConnectionString("subconnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string is not defined or empty.");
            }
            services.AddDbContext<TimeSheetContext>(options =>
                options.UseSqlServer(connectionString));

            // Adding JWT authentication
            var token = Configuration.GetSection("AppSettings:Token").Value;
            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("JWT token is not defined or empty.");
            }
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token))
                };
            });

            // Adding Swagger documentation
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            // Adding application services
            services.AddTransient<IAdmin, AdminRepo>();
            services.AddTransient<EmployeeInterface, EmployeeRepo>();
            services.AddTransient<ILogin, LoginRepo>();
            services.AddCors(options => {
                options.AddPolicy("AllowAnyOrigin", builder => {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Joy.TS.Api V1");
                c.RoutePrefix = "swagger";
            });
            app.UseCors(
 builder => {
     builder.AllowAnyOrigin();
     builder.AllowAnyMethod();
     builder.AllowAnyHeader();
 });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
