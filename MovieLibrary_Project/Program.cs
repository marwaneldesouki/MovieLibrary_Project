using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add DbContext and configure it to use SQL Server
builder.Services.AddDbContext<MediaLibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
//pattern: "{controller=MediaLibrary}/{action=Seri8es}/{id?}");
//pattern: "{controller=MediaLibrary}/{action=Details}/{id=3}");
pattern: "{controller=MediaAdmin}/{action=Index}/{id=3}");

app.MapControllerRoute(
    name: "admin",
    pattern: "{controller=MediaAdmin}/{action=Index}/{id?}");
app.Run();
