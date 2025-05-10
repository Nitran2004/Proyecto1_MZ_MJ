using Microsoft.EntityFrameworkCore;
using Proyecto1_MZ_MJ.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Agregar configuración de sesión
builder.Services.AddDistributedMemoryCache();
// Configuración de localización
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Agregar DbContext
builder.Services.AddDbContext<Proyecto1_MZ_MJContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Proyecto1_MZ_MJContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseRequestLocalization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Activar sesiones
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicializar la base de datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<Proyecto1_MZ_MJContext>();
    DbInitializer.Initialize(context);
}

app.Run();