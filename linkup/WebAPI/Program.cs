using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Infrasturcture; // 
var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. SERVICES CONFIGURATION (Dependency Injection)
// =========================================================================

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// .NET 8.0 සඳහා සාම්ප්‍රදායික Swagger OpenAPI setup එක
builder.Services.AddSwaggerGen(); 

// PostgreSQL Database Connection (EF Core) Setup එක
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

// Redis Cache (Matchmaking Queue එක සඳහා) Setup එක
var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString!));

// SignalR Setup (Real-time Chat සහ Live Matchmaking සඳහා)
builder.Services.AddSignalR();

var app = builder.Build();

// =========================================================================
// 2. HTTP REQUEST PIPELINE (Middleware)
// =========================================================================

// Local එකේදී API Endpoints ටික test කරන්න Swagger UI එක හදමු
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LinkUp API v1");
    });
}

app.UseHttpsRedirection();

// Authentication සහ Authorization (පසුවට Tokens දාන්න ඕනේ)
app.UseAuthorization();

// Controllers වල තියෙන API Endpoints map කිරීම
app.MapControllers();

// 💡 SignalR Hubs (පසුවට Hub classes හැදුවාම මේවායේ comment අයින් කරන්න)
// app.MapHub<MatchmakingHub>("/matchmakinghub");
// app.MapHub<ChatHub>("/chathub");

app.Run();