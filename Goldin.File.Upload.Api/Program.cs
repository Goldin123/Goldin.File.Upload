using Goldin.File.Upload.Database.Helper;
using Goldin.File.Upload.Database.Interface;
using Goldin.File.Upload.Database.Repository;
using Goldin.File.Upload.FileHandler.CsvFileHandler.FileUploadProcessor.Implementation;
using Goldin.File.Upload.FileHandler.CsvFileHandler.FileUploadProcessor.Interface;
using Goldin.File.Upload.Manager.UseCases.DataFileManagement.Implementation;
using Goldin.File.Upload.Manager.UseCases.DataFileManagement.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
