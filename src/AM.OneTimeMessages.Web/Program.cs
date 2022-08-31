using AM.OneTimeMessages.Core;
using AM.OneTimeMessages.Core.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<RedisConfiguration>((sp) => builder.Configuration.GetSection("Redis").Get<RedisConfiguration>());
builder.Services.AddSingleton<IMessageStorage, RedisMessageStorage>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/message/error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(name: "default", pattern: "{controller=message}/{action=index}/{id?}");

app.Run();
