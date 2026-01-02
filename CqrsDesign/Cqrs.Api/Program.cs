using Cqrs.Application.Commands;
using Cqrs.Application.Queries;
using Cqrs.Application.Services;
using Cqrs.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Postgres (Write DB)
builder.Services.AddDbContext<WriteDbContext>(o =>
    o.UseNpgsql(builder.Configuration["WriteConnection"]));

// Mongo (Read DB)
builder.Services.AddSingleton<ReadDb>();

// Handlers (CQRS)
builder.Services.AddScoped<CreateOrderHandler>();
builder.Services.AddScoped<GetOrdersHandler>();
builder.Services.AddHostedService<ReadModelUpdater>();
var app = builder.Build();

// Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Controllers
app.MapControllers();

app.Run();