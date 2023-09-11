using gPRC_API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using gPRC_API.Data;
using gPRC_API.Models;


var builder = WebApplication.CreateBuilder(args);

//Services INjection
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=app.db"));


builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<ToDoService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
