using Application.Helper;
using Infrastructure.Helper;
using Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationService();
builder.Services.AddInfrastructureServices(builder.Configuration);

//builder.WebHost.UseUrls("http://192.168.47.90:5000/");

builder.Services.AddCors(o => {
    o.AddPolicy("all", builder => builder.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    );
});

builder.Services.AddCors(o => {
    o.AddPolicy("MyOrigin", builder => builder.WithOrigins("*.*")
    .AllowAnyHeader()
    .AllowAnyMethod()
    );
});




builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
     app.UseSwagger();
     app.UseSwaggerUI();
}


app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<GlobalException>();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("all");
app.UseCors("MyOrigin");

app.Run();
