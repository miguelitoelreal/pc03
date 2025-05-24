using NewsPortal.Services;
using NewsPortal.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registrar HttpClient y PostService para inyección de dependencias
builder.Services.AddHttpClient<PostService>();

builder.Services.AddDbContext<FeedbackContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("FeedbackConnection") ?? "Data Source=feedback.db"));

var app = builder.Build();

// Ejecutar migraciones automáticamente al iniciar (para Docker/Render)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FeedbackContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapControllers(); // <-- Esto habilita los endpoints de la API

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
