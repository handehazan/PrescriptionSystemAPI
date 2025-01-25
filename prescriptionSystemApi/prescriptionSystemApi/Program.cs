using Microsoft.EntityFrameworkCore;
using prescriptionSystemApi.context;
using prescriptionSystemApi.source.db;
using prescriptionSystemApi.source.svc;

var builder = WebApplication.CreateBuilder(args);
// Add this at the VERY TOP of Main()
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin() // Allow requests from any origin (for development only)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<noSqlContext>();

// Register SqlDbContext
builder.Services.AddDbContext<SqlDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<MedicineAccess>();
builder.Services.AddHttpClient<MedicineService>();
builder.Services.AddScoped<MedicineService>();
builder.Services.AddScoped<PrescriptionAccess>();
builder.Services.AddScoped<PrescriptionService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Use CORS policy
app.UseCors("AllowAllOrigins");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
