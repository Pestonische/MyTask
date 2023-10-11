using Microsoft.EntityFrameworkCore;
using MyTask;
using MyTask.DataModels;
using MyTask.Interfaces;
using MyTask.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<Seed>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedDatabaseContext();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
