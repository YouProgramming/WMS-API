using Microsoft.EntityFrameworkCore;
using WMS_Repository_Data_Layer.Data;
using WMS_Repository_Data_Layer.Repository.IRepos;
using WMS_Repository_Data_Layer.Repository.Repos;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Queries.ProductQueriesHandlers;
using WMS_Repository_Data_Layer.Data.Entities.Models;
using Microsoft.AspNetCore.Identity;
using WMS_API.extentions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendDev",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // React dev server
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // If you're using cookies or auth headers
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagerJWTGenAuth();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<IIssuingRepo, IssuingRepo>();
builder.Services.AddScoped<IReceivingRepo, ReceivingRepo>();
builder.Services.AddScoped<ILogRepo, LogRepo>();
builder.Services.AddScoped<IUserRepository,  UserRepository>();
builder.Services.AddScoped<IStockMovementReportRepo, StockMovementReportRepo>();
builder.Services.AddScoped<iStockOverviewReportRepo, StockOverviewReportRepo>();

builder.Services.AddMediatR(typeof(GettAllProductsQueryHandler).Assembly);

builder.Services.AddCustomJWTAuth(builder.Configuration);

var app = builder.Build();

app.UseCors("AllowFrontendDev"); // Make sure it's before UseAuthorization and UseEndpoints

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
