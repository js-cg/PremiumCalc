var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); 

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();

