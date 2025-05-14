using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SRC.ST.Email;
using SRC.ST.Email.Data;
using SRC.ST.Email.Interfaces;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Configuration;
using SRC.DB.Models;
using SRC.DB.Interfaces.Settings;
using SRC.DB.Models.Complex;
using SRC.Backend.Models.System;
using SRC.Backend.Models.Config;
using SRC.DB.Models.EFMSSQL;


Log.Logger = new LoggerConfiguration()
           //.MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
           //.Enrich.FromLogContext()
           //.WriteTo.Console()
           .CreateLogger();


Log.Information("Starting web host");

WebApplicationBuilder builder = null;

try
{
    builder = WebApplication.CreateBuilder(args);
    builder
          .Host
          .UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    //.WriteTo.Console()
                    );
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return;
}
finally
{
    Log.CloseAndFlush();
}

//init folder
try
{
    SysAppsetting sysFolder = new SysAppsetting();
    sysFolder.InitFolder();

}
catch (Exception ex)
{
    Log.Fatal(ex, $"Create System Folder fail,{ex.Message}");
}


// Add services to the container.

//builder.Services.AddDbContext<SintongContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

//builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);

//builder.Services.AddAuthorization();

//builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

//builder.Services.AddControllersWithViews(options =>
//{
//    var policy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//    options.Filters.Add(new AuthorizeFilter(policy));
//});

//builder.Services.AddRazorPages()
//     .AddMicrosoftIdentityUI();


builder.Services
    //.AddDataBaseCoreFactory(builder.Configuration, DataBaseCoreModule.DatabaseEngine.MYSQL)
    .AddDataBaseCoreFactory(builder.Configuration, DataBaseCoreModule.DatabaseEngine.MSSQL)
    .AddDFModule()
    .AddSystemService();

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("RequireWindowsAuth", policy =>
//    {
//        policy.AuthenticationSchemes.Add(IISDefaults.AuthenticationScheme);
//        policy.RequireAuthenticatedUser();
        
//    });
//});



builder.Services.AddDbContext<EFContext>(options =>
      options
      //.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"))
      .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
      //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
      );


builder.Services.AddLogging();

builder.Services.AddScoped<ISelectUI>((m) =>
{
    IDF_SystemCode sysCodeDF = m.GetRequiredService<IDF_SystemCode>();
    Serilog.ILogger log = m.GetRequiredService<Serilog.ILogger>();
    SelectUI sysDD = new SelectUI(log, sysCodeDF);
    return sysDD;
});
builder.Services.AddTransient<SysAppsetting>((m) =>
{
    return builder.Configuration.Get<SysAppsetting>();
});


builder.Services.AddScoped<ISRCEmail>((m) => {
    bool isSmtp = builder.Configuration.GetValue<bool>("EMAIL_BY_SMTP");
    Microsoft.Extensions.Logging.ILogger log = m.GetRequiredService<ILoggerFactory>().CreateLogger("EMailLogger");
    if (isSmtp)
    {
        //Email email = new Email(builder.Configuration.GetSection("EmailConfig").Get<EmailConfig>(), log);

        IDF_SystemCode sysCodeDF = m.GetRequiredService<IDF_SystemCode>();
        SmtpConfig smtpCfg = sysCodeDF.GetSmtpConfig();
        EmailConfig emailCfg = new EmailConfig()
        {
            From = smtpCfg.From,
            MailServer = smtpCfg.MailServer,
            MailServerAccount = smtpCfg.MailServerAccount,
            MailServerPassword = smtpCfg.MailServerPassword,
            Port = smtpCfg.Port,
        };

        Email email = new Email(emailCfg, log);

        if (email == null)
        {
            Log.Error("EMailConfig未設定");
        }
        return email;
    }

    return null;
});


//builder.Services.AddTransient<ISystem>((m) =>
//{
//    return new SystemDatabase(m.GetRequiredService<SintongContext>());
//});


builder.Services.AddHttpContextAccessor();

//builder.Services.AddTransient<AuthMeta<LoginMeta>>();

//這段可能不需要須測試
Serilog.ILogger log = null;
log = new LoggerConfiguration()
         .ReadFrom
         .Configuration(builder.Configuration)
         .CreateLogger();

//services.AddDistributedMemoryCache();
builder.Services.AddSession((opt) =>
{

    opt.IdleTimeout = new TimeSpan(0, 33, 0);
    opt.Cookie.HttpOnly = true;
    opt.Cookie.IsEssential = true;
});

builder.Services.AddScoped<MenusAuth>((service) =>
{
    MenusAuth menu = new MenusAuth();
    menu.Urls = new List<string>();

    IHttpContextAccessor accessor = service.GetRequiredService<IHttpContextAccessor>();

    bool isAutn = (
                    (accessor.HttpContext.User != null) &&
                    (accessor.HttpContext.User.Identity != null) &&
                    accessor.HttpContext.User.Identity.IsAuthenticated
                    );

    if (isAutn)
    {
        //InfinityContext db = service.GetRequiredService<InfinityContext>();
        EFContext db = service.GetRequiredService<EFContext>();
        SRCLoginMeta meta = accessor.HttpContext.User.Identity.LoginMeta();
        string editor = meta.Account; //((ClaimsIdentity)accessor.HttpContext.User.Identity).Claims.Where(m => m.Type == "Account").First().Value;

        if (!string.IsNullOrWhiteSpace(editor))
        {
            backend_user user = db.backend_users.Where(m => m.account == editor).FirstOrDefault();
            if (user != null)
            {
                List<backend_users_role> roles = db.backend_users_roles.Where(m => m.user_id == user.user_id).ToList();
                List<int> rolesID = roles.Select(m => m.role_id).ToList();
                List<int> funcsID = db.role_funcs.Where(m => rolesID.Contains(m.role_id)).ToList().Select(m => m.func_id).ToList();

                menu.Urls = db.funcs.Where(m => funcsID.Contains(m.pid)).Select(m => m.url.ToUpper()).ToList();
                menu.FirstLogin = user.first_login;
            }

        }

        //if (!string.IsNullOrWhiteSpace(editor))
        //{
        //    backend_user user = db.backend_users.Where(m => m.account == editor).FirstOrDefault();
        //    if (user != null)
        //    {
        //        List<Member_Role> roles = db.backend_users_roles.Where(m => m.member_id == user.user_id).ToList();
        //        List<int> rolesID = roles.Select(m => m.role_id).ToList();
        //        List<int> funcsID = db.Role_Funcs.Where(m => rolesID.Contains(m.role_id)).ToList().Select(m => m.func_id).ToList();

        //        menu.Urls = db.Funcs.Where(m => funcsID.Contains(m.pid)).Select(m => m.url.ToUpper()).ToList();

        //    }

        //}

    }

    return menu;
});


string cookieDomain = builder.Configuration.GetValue<string>("CookieDomain");
builder.Services
  .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(
    CookieAuthenticationDefaults.AuthenticationScheme,
      options =>
      {
          options.LoginPath = "/Entry/Login";
          options.Cookie.HttpOnly = true;
          options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
          //options.AccessDeniedPath = "/Account/AccessDenied";
          options.SlidingExpiration = true;
          options.LogoutPath = "/Entry/Logout";
          //options.d
          options.Cookie.Domain = cookieDomain;
          options.Cookie.Name = "eprotobackend";

          //options.Cookie.Path = "/";
      }
  );


//builder.Services.AddSignalR();
//builder.Services.AddSingleton<IUserIdProvider, UserNotificationTickerHubProvider>();
//builder.Services.AddSingleton<UserNotificationTableDependency>();

//builder.Services.AddOptions().Configure<FcmConfig>(builder.Configuration.GetSection("Fcm"));

builder.WebHost.ConfigureKestrel(o => {
    o.Limits.MaxRequestBodySize = 100 * 1024 * 1024;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Index");
    app.UseStatusCodePagesWithReExecute("/Error/NotFound", "?statusCode={0}");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseCookiePolicy();

app.UseRouting();

//app.UseMiddleware<CustomUnauthorizedHandler>("/Entry/Login");

//app.MapHub<UserNotificationTickerHub>("/signalr");


//var ConnectionString = app.Configuration.GetConnectionString("DefaultConnection");

//app.UseTableDependency(ConnectionString);

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Entry}/{action=Login}/{id?}");
app.Run();
