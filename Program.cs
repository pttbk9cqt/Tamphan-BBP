using Tamphan_BBP.Services;    //Tâm thêm để bắn dữ liệu trong json ra view, thì khi thêm vào thì phải thêm using Tamphan_BBP.Services; để gọi đến class LoadContentService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<LoadContentService>();  //Tâm thêm để bắn dữ liệu trong json ra view

var app = builder.Build();

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

app.MapStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=TTr1506}/{action=Index}/{id?}").WithStaticAssets();

app.Run();
