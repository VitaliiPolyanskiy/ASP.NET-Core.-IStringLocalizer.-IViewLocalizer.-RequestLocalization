using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using MultilingualSite;
using MultilingualSite.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Налаштування підключення до бази даних
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ClubContext>(options => options.UseSqlServer(connection));

// Налаштування шляху до папки з файлами ресурсів (.resx)
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Реєстрація MVC та налаштування локалізації представлень і моделей
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options => {
        // Вказуємо інфраструктурі завжди використовувати маркерний клас з кореня проєкту
        // для пошуку перекладів атрибутів валідації (наприклад, [Required])
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });

// Конфігурація підтримуваних культур
var supportedCultures = new[]
{
    new CultureInfo("uk"),
    new CultureInfo("en"),
    new CultureInfo("de"), 
    new CultureInfo("fr"),
    new CultureInfo("ru")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    // Мова за замовчуванням
    options.DefaultRequestCulture = new RequestCulture("uk");
    // Культури для форматування дат, чисел тощо
    options.SupportedCultures = supportedCultures;
    // Культури для пошуку локалізованих ресурсів (інтерфейсу)
    options.SupportedUICultures = supportedCultures;
});

var app = builder.Build();

// ==========================================
// Налаштування конвеєра запитів (Middleware)
// ==========================================
// Додаємо Middleware для локалізації
// ВАЖЛИВО: UseRequestLocalization має бути викликано ПЕРЕД маршрутизацією
app.UseRequestLocalization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Club}/{action=Index}/{id?}");

app.Run();