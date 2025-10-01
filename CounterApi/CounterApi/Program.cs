using CounterApi.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opt =>
    opt.AddPolicy("AllowCMS", p =>
    {
        p.WithOrigins("https://localhost:7273")
        .AllowAnyHeader()
        .AllowAnyMethod();
    }));
builder.Services.AddCounterStorage(builder.Configuration.GetConnectionString("DefaultConnection")!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowCMS");
app.UseAuthorization();

app.MapControllers().RequireCors("AllowCMS");

app.Run();
