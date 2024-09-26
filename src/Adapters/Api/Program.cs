using Application;
using Application.Abstractions.Ports;
using Infrastructure.PostgreSql.Database;
using Infrastructure.ServiceBus;
using Infrastructure.ServiceBus.Listener;
using Infrastructure.ServiceBus.Publisher;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ISaleRepository, SaleRepository>();

builder.Services.Configure<ServiceBusSettings>(builder.Configuration.GetSection("ServiceBusSettings"));
builder.Services.AddScoped<IServiceBusListener, ServiceBusListener>();
builder.Services.AddScoped<IServiceBusPublisher, ServiceBusPublisher>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddSingleton<NpgsqlConnection>(sp => new NpgsqlConnection(connectionString));

builder.Services.AddApplicationLayer();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Urls.Add("http://0.0.0.0:5003");

app.Run();
