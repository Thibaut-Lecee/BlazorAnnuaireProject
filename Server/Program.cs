using System.Text.Json.Serialization;
using BlazorAnnuaireProject.Context;
using BlazorAnnuaireProject.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlazorAnnuaireProject.AnnuaireServices.SalarieService;
using BlazorAnnuaireProject.AnnuaireServices.SiteService;
using BlazorAnnuaireProject.AnnuaireServices.ServiceService;
using BlazorAnnuaireProject.AnnuaireServices.AdminService;
using BlazorAnnuaireProject.Authorization;
using BlazorAnnuaireProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<ISalarieService, SalarieService>();
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
//Db context
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(true);
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme. Example: (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });


    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "oauth2",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new string[] { }
        }
    });

});
builder.Services.Configure<IdentityOptions>(
    options => options.SignIn.RequireConfirmedEmail = true);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var value = builder.Configuration.GetSection("AppSettings:Secret").Value;
        if (value != null)
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(value)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
    });


builder.Services.AddCors(options =>
    options.AddDefaultPolicy(cors =>
    {
        cors
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    }));

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
var adminConfig = builder.Configuration.GetSection("AdminConfiguration").Get<AdminConfiguration>();
builder.Services.AddSingleton(adminConfig);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlazorAnnuaireProject v1"));
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
