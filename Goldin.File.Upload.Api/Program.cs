using Goldin.File.Upload.Api.Authorization;
using Goldin.File.Upload.Api.Helpers;
using Goldin.File.Upload.Api.Services;
using Goldin.File.Upload.Database.Helper;
using Goldin.File.Upload.Database.Interface;
using Goldin.File.Upload.Database.Repository;
using Goldin.File.Upload.FileHandler.CsvFileHandler.FileUploadProcessor.Implementation;
using Goldin.File.Upload.FileHandler.CsvFileHandler.FileUploadProcessor.Interface;
using Goldin.File.Upload.FileHandler.CsvFileHandler.FileValidationRules.Implementation;
using Goldin.File.Upload.FileHandler.CsvFileHandler.FileValidationRules.Interface;
using Goldin.File.Upload.Manager.UseCases.DataFileManagement.Implementation;
using Goldin.File.Upload.Manager.UseCases.DataFileManagement.Interface;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Zetes API App", Version = "v1" });

    // Add JWT Bearer Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter your JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

var configuration = builder.Services.BuildServiceProvider().GetService<IConfiguration>();

// Get the connection string from the configuration
var connectionString = configuration.GetConnectionString("DatabaseConnection");

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddSingleton<IDatabaseConnection>(new DatabaseConnection(connectionString));
builder.Services.AddScoped<IDataFile, DataFileRepository>();
builder.Services.AddScoped<IDataFileManager, DataFileManager>();
builder.Services.AddScoped<IDataFileCsvProcessor, DataFileCsvProcessor>();
builder.Services.AddScoped<IDataFileCsvUploader, DataFileCsvUploader>();
builder.Services.AddScoped<IDataFileCsvValidationRules, DataFileCsvValidationRules>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts =>
    {
        opts.EnableTryItOutByDefault();
        opts.DocumentTitle = "Zetes API App";
        opts.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

app.UseCors(x => x
       .AllowAnyOrigin()
       .AllowAnyMethod()
       .AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
