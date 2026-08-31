using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using StudentRegistry.Data;
using StudentRegistry;
using Serilog;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. CONFIGURAÇÃO DE IDIOMAS
// ==========================================
var supportedCultures = new[] { "pt-BR", "en-US" };

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console());

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture("pt-BR")
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// ==========================================
// 2. BANCO DE DADOS
// ==========================================

if (!builder.Environment.IsEnvironment("Testing"))
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                           throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));
}

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

// ==========================================
// TELEMETRIA (OpenTelemetry / Aspire)
// ==========================================
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("StudentRegistry"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation())
    .UseOtlpExporter();

// ==========================================
// 3. IDENTITY
// ==========================================
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages(options =>
    {
        options.Conventions.AuthorizeFolder("/Students");
        options.Conventions.AuthorizeFolder("/Premiums");
    })
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(SharedResource));
    });

var app = builder.Build();

// ==========================================
// 3. PIPELINE DE REQUISIÇÕES
// ==========================================

app.UseRequestLocalization();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapHealthChecks("/health");

app.Run();

public partial class Program { }
