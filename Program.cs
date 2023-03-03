
using Microsoft.EntityFrameworkCore;
using DbContexts;
using Carter;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BasketPlayersTeamsDb>(opt => opt.UseInMemoryDatabase("BasketballPlayerList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();
var app = builder.Build();

app.MapCarter();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();