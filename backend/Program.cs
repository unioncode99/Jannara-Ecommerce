using Jannara_Ecommerce.Business;
using Jannara_Ecommerce.DataAccess;
using Jannara_Ecommerce.Utilities;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<ImageSettings>(builder.Configuration.GetSection("ImageSettings"));
builder.Services.AddApplicationServices();
builder.Services.AddApplicationRepositories();
builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder => builder
            .WithOrigins("http://localhost:5173") // Frontend origin
            .AllowAnyHeader()
            .AllowAnyMethod());
});


var app = builder.Build();

// Use CORS
app.UseCors("AllowLocalhost");

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.Run();
