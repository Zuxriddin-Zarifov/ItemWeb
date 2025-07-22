using Anjir.Zuhriddin.Items.DataAccess;
using Anjir.Zuhriddin.Items.Web;
using Item.Server.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// CORS sozlamasi (Angular uchun)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins("http://localhost:5000", "https://localhost:52492", "https://dev.zuhriddin.com") // frontend domeningiz
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


// JWT konfiguratsiya
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// Xizmatlar (Context va servislar)
builder.Services.ConfigureDataContext(builder);
builder.Services.ConfigureServices();

// Swagger sozlamalari (JWT bilan)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Anjir API", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Bearer token kiriting: `Bearer {token}`",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// LOG YOZISH
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // Debug va undan yuqori loglar yoziladi
    .WriteTo.Console() // Consolega yozish
    .WriteTo.File(
        path: "logs/log-.txt",      // Fayl yo‘li
        rollingInterval: RollingInterval.Day, // Har kuni yangi fayl
        retainedFileCountLimit: 7,  // Maksimum 7ta faylni saqlaydi
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}" // Log format
    )
    .CreateLogger();

builder.Host.UseSerilog();



var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MainContext>();
    db.Database.Migrate(); // <== bu yer aynan dotnet ef database update vazifasini bajaradi
}
app.UseSwagger();
app.UseSwaggerUI();

// 🌐 Static fayllar uchun (agar Angular frontend shu yerda bo‘lsa)
app.UseDefaultFiles();
app.UseStaticFiles(); // wwwroot/index.html va boshqa Angular fayllar

app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
