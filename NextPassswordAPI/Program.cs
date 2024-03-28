using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NextPassswordAPI.Data;
using NextPassswordAPI.Models;
using NextPassswordAPI.Repository;
using NextPassswordAPI.Repository.Interfaces;
using NextPassswordAPI.Services;
using NextPassswordAPI.Services.Interfaces;
using Swashbuckle.AspNetCore.Filters;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});


IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Identity.Bearer";
    options.DefaultSignInScheme = "Identity.Bearer";
    options.DefaultAuthenticateScheme = "Identity.Bearer";
    options.DefaultChallengeScheme = "Identity.Bearer";
})
   .AddCookie("Identity.Bearer", options =>
   {
       options.Cookie.Name = "access_token";
       options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
   });

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

/*builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SECRET_KEY"))
    };
});*/

builder.Services.AddDefaultIdentity<ApplicationUser> ()
        .AddEntityFrameworkStores<DataContext>();

/* Services */
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IHashPassword, HashPasswordService>();

/* Repositories */
builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<ApplicationUser>();

app.UseHttpsRedirection();

app.UseAuthentication();    
app.UseAuthorization();

app.MapControllers();

app.Run();
