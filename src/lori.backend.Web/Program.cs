using Ardalis.ListStartupServices;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using lori.backend.Core;
using lori.backend.Core.Interfaces;
using lori.backend.Infrastructure;
using lori.backend.Infrastructure.Models;
using lori.backend.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.CheckConsentNeeded = context => true;
  options.MinimumSameSitePolicy = SameSiteMode.None;
});

string? connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

builder.Services.AddDbContext(connectionString!);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ITokenService<Login>, TokenService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "LORI API", Version = "v1" });
  c.EnableAnnotations();

  c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
  {
    Description = "Standard Authorization header using the Bearer scheme",
    In = ParameterLocation.Header,
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    BearerFormat = "JWT",
    Scheme = "Bearer"
  });
  c.OperationFilter<SecurityRequirementsOperationFilter>();
});

var secret = builder.Configuration.GetValue<string>("AppSettings:Token");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
      ClockSkew = TimeSpan.Zero,
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = builder.Configuration.GetValue<string>("AppSettings:Issuer"),
      ValidAudience = builder.Configuration.GetValue<string>("AppSettings:Audience"),
      IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret!)),
    });

// add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
builder.Services.Configure<ServiceConfig>(config =>
{
  config.Services = new List<ServiceDescriptor>(builder.Services);

  // optional - default path to view services is /listallservices - recommended to choose your own path
  config.Path = "/listservices";
});


builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
  containerBuilder.RegisterModule(new DefaultCoreModule());
  containerBuilder.RegisterModule(new DefaultInfrastructureModule(builder.Environment.EnvironmentName == "Development"));
});

//builder.Logging.AddAzureWebAppDiagnostics(); add this if deploying to Azure

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseShowAllServicesMiddleware();
}
else
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}
app.UseRouting();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LORI API v1"));

app.UseEndpoints(endpoints =>
{
  endpoints.MapDefaultControllerRoute();
});

app.Run();

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program
{
}
