 using Application.Contrast.Services;
using Application.implementation;
using Catalogue.DomainServiceContract.Service;
using Infrastracture.Catalogue.EF.Persistance;
using Infrastracture.Catalogue.EF.Persistance.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security;
using Security.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string CatalogueConnectionString = builder.Configuration.GetConnectionString("CatalogueConnectionString");

string SecurityConnectionString = builder.Configuration.GetConnectionString("Security");

builder.Services.AddDbContext<CatalogueContext>(optionsAction => optionsAction.UseSqlServer(CatalogueConnectionString));

builder.Services.AddDbContext<SecurityContext>(optionsAction => optionsAction.UseSqlServer(SecurityConnectionString));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(optionAction => { 
    optionAction.Password.RequireDigit=true;
    optionAction.Lockout.MaxFailedAccessAttempts=10;
    optionAction.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(5);
}).AddEntityFrameworkStores<SecurityContext>().AddDefaultTokenProviders();



builder.Services.ConfigureApplicationCookie(OptionAction =>
{
    OptionAction.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    OptionAction.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
    OptionAction.SlidingExpiration = true;
    OptionAction.LoginPath = "/Account/Login";
});


//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddBearerToken()
//    .AddCookie("x", option =>
//    {
//        option.LoginPath="safws";
//    })
//    ;
//builder.Services.AddAuthorization(optaion => {
//    optaion.AddPolicy("AdminOnly", policy => policy.RequireClaim("role","admin"));
//    optaion.AddPolicy("hr", policy =>
//    {
//        policy.AddRequirements();

//    });
    

//});

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryApplicationServiceContract, CategoryApplication>();

//builder.Services.AddHttpClient("shop", client =>
//{
//    client.BaseAddress = Uri();
//});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.Run();
