using System.Text;
using ClothShopperBack.API.Jobs;
using ClothShopperBack.BLL.Services;
using ClothShopperBack.DAL;
using ClothShopperBack.DAL.Common;
using ClothShopperBack.DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Quartz.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"), builder =>
    {
        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    });
});

builder.Services.AddIdentityCore<User>()
    .AddRoles<Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
                ValidateAudience = true,
                ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Secret").Value)),
                ValidateIssuerSigningKey = true,
            };
        });

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    q.ScheduleJob<UpdateClothesJob>(trigger =>
    {
        trigger.WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromHours(1)));
    });
    // base Quartz scheduler, job and trigger configuration
});

builder.Services.AddQuartzServer(options =>
{
    // when shutting down we want jobs to complete gracefully
    options.WaitForJobsToComplete = true;
});

builder.Services.AddHttpClient();

AddDIServices(builder);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void AddDIServices(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<UserManager<User>>();
    builder.Services.AddScoped<RoleManager<Role>>();

    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IClothService, ClothService>();
    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddScoped<IAlbumService, AlbumService>();
    builder.Services.AddScoped<IUserService, UserService>();

    builder.Services.AddScoped<IVkAPI, VkAPI>();
}
