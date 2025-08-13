using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using NotesFullStack.Shared.Services;
using NotesFullStack.Web.Components;
using NotesFullStack.Web.Data;
using NotesFullStack.Web.Data.Entities;
using NotesFullStack.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add device-specific services used by the NotesFullStack.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddDbContextFactory<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    options.UseSqlServer(connectionString);
});

builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication("Web")
    .AddCookie("Web", options =>
    {
        options.Cookie.Name = "Web";
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.Cookie.HttpOnly = true;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, WebAuthStateProvider>();

var app = builder.Build();

AutoMigrateDbAsync(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication().UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(NotesFullStack.Shared._Imports).Assembly);

app.Run();

void AutoMigrateDbAsync(IServiceProvider sp) {
    using var scope = sp.CreateScope();
    var context = scope.ServiceProvider
        .GetRequiredService<IDbContextFactory<DataContext>>()
        .CreateDbContext();

    if (context.Database.GetPendingMigrations().Any())
        context.Database.Migrate();
}