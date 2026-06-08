using AutoMapper;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Security.KeyVault.Secrets;
using Employee_Dept_Loc_Proj.Services;
using EmployeeApi.Middleware;
using Employees.AutoMappers;
using Employees.DbContxt;
using Employees.Filters;
using Employees.Interfaces;
using Employees.Repositories;
using JWTAuthentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using Serilog;



var builder = WebApplication.CreateBuilder(args);
QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddAuthorization();
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext());



builder.Services.AddSingleton<ServiceBusClient>(sp =>
            new ServiceBusClient(builder.Configuration.GetConnectionString("ServiceBusConnection")));

builder.Services.AddSingleton<ServiceBusSender>(sp =>
{
    var client = sp.GetRequiredService<ServiceBusClient>();
    return client.CreateSender("myqueue");
});

//string keyVaultUri = builder.Configuration["KeyVaultUrl"];

//var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());

//KeyVaultSecret secret = client.GetSecret("kvAzureSecret");

//string dbConnectionString = secret.Value;

//builder.Services.AddDbContext<DatabaseContext>
//    (options => options.UseSqlServer(dbConnectionString));

builder.Services.AddDbContext<DatabaseContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbConnection")));

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = new[] 
    { 
        "application/json",
        "text/plain",
        "text/html"
    };
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Optimal;
});

builder.Services.AddScoped<AsyncExceptionFilter>();
builder.Services.AddScoped<ApiResponseResultFilter>();

// Register AutoMapper and scan for Profile classes in this assembly
// Add services to the container.

// REGISTER AUTOMAPPER HERE 
// This tells AutoMapper to scan the assembly (project) where MappingProfile lives
// This satisfies the new signature by passing an empty config expression first
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));


builder.Services.AddControllers();
//builder.Services.AddControllers(options =>
//{
//    options.Filters.AddService<AsyncExceptionFilter>();
//    options.Filters.AddService<ApiResponseResultFilter>();
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularAppPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "http://localhost:54224", "https://polite-island-0cda25b00.7.azurestaticapps.net", "https://icy-cliff-0a457e100.7.azurestaticapps.net")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });

});
builder.Services.AddScoped<IEmployee, EmployeeRepo>();

builder.Services.AddHttpClient<DepartmentApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7004/api/");
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
JwtExtensions.AddJwtAuthentication(builder.Services, builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 1. Exception Handling MUST always be first to catch errors from everything below it.
app.UseCustomExceptionHandling();

// 2. Move HTTPS redirection here. Drop insecure requests immediately.
app.UseHttpsRedirection();

// 3. Move Serilog up here. It can now log execution times, status codes, and auth failures.
app.UseSerilogRequestLogging();

// 4. Response compression should compress everything underneath it.
app.UseResponseCompression();

// 5. Routing comes before security boundaries.
app.UseRouting();

// 6. CORS MUST come after UseRouting, but BEFORE Authentication/Authorization.
// This ensures auth failure responses still include CORS headers for Angular.
app.UseCors("AllowAngularAppPolicy");

// 7. Identity Check: Who are you?
app.UseAuthentication();

// 8. Permissions Check: Are you allowed here?
app.UseAuthorization();

// 9. Execute the Endpoint (Maps the request to your Controllers)
app.MapControllers();

// 10. Start the App
app.Run();