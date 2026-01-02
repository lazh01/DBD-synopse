using Crud.Application.Services;
using Crud.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Postgres (CRUD DB)
builder.Services.AddDbContext<CrudDbContext>(options =>
    options.UseNpgsql(builder.Configuration["WriteConnection"]));

// Services
builder.Services.AddScoped<CrudOrderService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();