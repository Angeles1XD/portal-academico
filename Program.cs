using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PortalAcademico.Data;
using PortalAcademico.Models; // 👈 IMPORTANTE para Curso

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// =====================================
// 👉 MIGRACIONES + SEED (MUY IMPORTANTE)
// =====================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Aplica migraciones
    db.Database.Migrate();

    // Datos iniciales (solo si está vacío)
    if (!db.Cursos.Any())
    {
        db.Cursos.AddRange(
            new Curso { Nombre = "Matemática", Creditos = 4 },
            new Curso { Nombre = "Programación", Creditos = 5 },
            new Curso { Nombre = "Base de Datos", Creditos = 3 }
        );

        db.SaveChanges();
    }
}

// Configuración
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ⚠️ Render no usa HTTPS interno
// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();