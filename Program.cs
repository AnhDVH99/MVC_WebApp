using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using ASP.NET_Core_MVC_Piacom.Repositories;
using ASP.NET_Core_MVC_Piacom.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthorization(options =>
        {

            options.AddPolicy("ViewCustomer", policy =>
                policy.RequireClaim("Permission", "ViewCustomers"));


            options.AddPolicy("CreateCustomer", policy =>
                policy.RequireClaim("Permission", "CreateCustomers"));


            options.AddPolicy("EditCustomer", policy =>
                policy.RequireClaim("Permission", "EditCustomers"));


            options.AddPolicy("ViewOrder", policy =>
                policy.RequireClaim("Permission", "ViewOrders"));


            options.AddPolicy("CreateOrder", policy =>
                policy.RequireClaim("Permission", "CreateOrders"));


            options.AddPolicy("EditOrder", policy =>
                policy.RequireClaim("Permission", "EditOrders"));

            options.AddPolicy("ViewUnit", policy =>
                policy.RequireClaim("Permission", "ViewUnits"));


            options.AddPolicy("CreateUnit", policy =>
                policy.RequireClaim("Permission", "CreateUnits"));


            options.AddPolicy("EditUnit", policy =>
                policy.RequireClaim("Permission", "EditUnits"));

            options.AddPolicy("ViewProduct", policy =>
                policy.RequireClaim("Permission", "ViewProducts"));


            options.AddPolicy("CreateProduct", policy =>
                policy.RequireClaim("Permission", "CreateProducts"));


            options.AddPolicy("EditProduct", policy =>
                policy.RequireClaim("Permission", "EditProducts"));

            options.AddPolicy("ViewPrice", policy =>
                policy.RequireClaim("Permission", "ViewPrices"));


            options.AddPolicy("CreatePrice", policy =>
                policy.RequireClaim("Permission", "CreatePrices"));


            options.AddPolicy("EditPrice", policy =>
                policy.RequireClaim("Permission", "EditPrices"));

            options.AddPolicy("ViewEmployee", policy =>
                policy.RequireClaim("Permission", "ViewEmployees"));


            options.AddPolicy("CreateEmployee", policy =>
                policy.RequireClaim("Permission", "CreateEmployees"));


            options.AddPolicy("EditEmployee", policy =>
                policy.RequireClaim("Permission", "EditEmployees"));

        });





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
