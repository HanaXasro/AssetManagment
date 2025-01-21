using Application.Helper;
using Infrastructure.AutoSeeds;
using Infrastructure.DataContext;
using Infrastructure.Helper;
using Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationService();
builder.Services.AddInfrastructureServices(builder.Configuration);


builder.Services.AddCors(o => {
    o.AddPolicy("all", e => e.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    );
});

builder.Services.AddCors(o => {
    o.AddPolicy("MyOrigin", e => e.WithOrigins("*.*")
    .AllowAnyHeader()
    .AllowAnyMethod()
    );
});



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataDbContext>();
    context.Database.EnsureCreated(); 
    PermissionSeed.Seed(context);
}


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
