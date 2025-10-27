using MassTransit;
using Microsoft.EntityFrameworkCore;
using PB.CreditProposalService.Application.Services;
using PB.CreditProposalService.Infrastructure;
using PB.CreditProposalService.Infrastructure.Consumers;
using PB.CreditProposalService.Infrastructure.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Application Services
builder.Services.AddScoped<IScoreCalculator, ScoreCalculator>();

// MassTransit + RabbitMQ
builder.Services.AddMassTransit(x =>
{
    // Registrar consumer
    x.AddConsumer<CustomerRegisteredConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], builder.Configuration["RabbitMQ:VirtualHost"], h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });

        // Configurar endpoint para o consumer
        cfg.ReceiveEndpoint("customer-registered-queue", e =>
        {
            e.ConfigureConsumer<CustomerRegisteredConsumer>(context);
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Aplicar migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProposalDbContext>();
    db.Database.Migrate();
}

app.Run();