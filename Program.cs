using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Employee_Dept_Loc_Proj.Services;
using Employees.DbContxt;
using Employees.Filters;
using Employees.Interfaces;
using Employees.Repositories;
using JWTAuthentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
QuestPDF.Settings.License = LicenseType.Community;

string keyVaultUri = builder.Configuration["KeyVaultUrl"];

var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());

KeyVaultSecret secret = client.GetSecret("kvAzureSecret");

string dbConnectionString = secret.Value;

builder.Services.AddDbContext<DatabaseContext>
    (options => options.UseSqlServer(dbConnectionString));

//builder.Services.AddDbContext<DatabaseContext>
//    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbConnection")));


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

builder.Services.AddControllers(options =>
{
    options.Filters.AddService<AsyncExceptionFilter>();
    options.Filters.AddService<ApiResponseResultFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularAppPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "http://localhost:54224", "webappnameangularstaging-bwaxejcbbsg4bve9.centralindia-01.azurewebsites.net", "webappnameangular-fjfwaygdbkc6a6gh.centralindia-01.azurewebsites.net")
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
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularAppPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCompression();
app.MapControllers();

app.Run();
