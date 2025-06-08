using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using CustomerApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "", Version = "v1",});
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); 
});

builder.Services.AddControllers();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddDbContext<CustomerApi.Data.CustomerServiceDBContext>(
     options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("./v1/swagger.json", "Customer Service API v1");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

//TDOD: add healthchecks

app.Run();

