using ASP.NET_Core_MVC_Piacom.Authorizes;
using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using ASP.NET_Core_MVC_Piacom.Repositories;
using ASP.NET_Core_MVC_Piacom.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthorization(options =>
        {

            options.AddPolicy("CreateCustomer", policy =>
                policy.RequireClaim("Permission", "CreateCustomers"));


            options.AddPolicy("EditCustomer", policy =>
                policy.RequireClaim("Permission", "EditCustomers"));


            options.AddPolicy("CreateOrder", policy =>
                policy.RequireClaim("Permission", "CreateOrders"));


            options.AddPolicy("EditOrder", policy =>
                policy.RequireClaim("Permission", "EditOrders"));

            options.AddPolicy("CreateUnit", policy =>
                policy.RequireClaim("Permission", "CreateUnits"));


            options.AddPolicy("EditUnit", policy =>
                policy.RequireClaim("Permission", "EditUnits"));


            options.AddPolicy("CreateProduct", policy =>
                policy.RequireClaim("Permission", "CreateProducts"));


            options.AddPolicy("EditProduct", policy =>
                policy.RequireClaim("Permission", "EditProducts"));

            options.AddPolicy("CreatePrice", policy =>
                policy.RequireClaim("Permission", "CreatePrices"));


            options.AddPolicy("EditPrice", policy =>
                policy.RequireClaim("Permission", "EditPrices"));


            options.AddPolicy("CreateEmployee", policy =>
                policy.RequireClaim("Permission", "CreateEmployees"));


            options.AddPolicy("EditEmployee", policy =>
                policy.RequireClaim("Permission", "EditEmployees"));


            // Claim handler
            options.AddPolicy("ViewOrder", policy =>
            policy.Requirements.Add(new ViewClaimRequirement()));

            options.AddPolicy("ViewCustomer", policy =>
            policy.Requirements.Add(new ViewClaimRequirement()));

            options.AddPolicy("ViewUnit", policy =>
            policy.Requirements.Add(new ViewClaimRequirement()));

            options.AddPolicy("ViewPrice", policy =>
            policy.Requirements.Add(new ViewClaimRequirement()));

            options.AddPolicy("ViewProduct", policy =>
            policy.Requirements.Add(new ViewClaimRequirement()));

            options.AddPolicy("ViewEmployee", policy =>
            policy.Requirements.Add(new ViewClaimRequirement()));


        });


builder.Services.AddSingleton<IAuthorizationHandler, ViewClaimHandler>();

builder.Services.AddControllersWithViews();

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();


builder.Services.AddDbContext<PiacomDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PiacomDbConnectionString")));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PiacomDbAuthConnectionString")));



builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 1;
    options.Password.RequiredLength = 6;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
    options.LoginPath = "/Account/Login"; // Redirect path for unauthorized users
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Secrets.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    }
    );




// Add Repos
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ICreditRepository, CreditRepository>();
builder.Services.AddScoped<IPriceRepository, PriceRepository>();
builder.Services.AddScoped<IPriceDetailRepository, PriceDetailRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();


//Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<PaymentValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddCustomerRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AccountValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EditCustomerRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UnitValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EditOrderRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddPriceValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EditPriceValidator>();

builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

    
// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
