using Application.Contrast.QueryServices;
using Application.Contrast.Repositories;
using Application.Contrast.Services;
using Application.implementation;
using Catalogue.DomainServiceContract.Service;
using Infrastracture.Catalogue.EF.Persistance;
using Infrastracture.Catalogue.EF.Persistance.FileStorage;
using Infrastracture.Catalogue.EF.Persistance.Querie;
using Infrastracture.Catalogue.EF.Persistance.Repositories;
using Infrastracture.Security.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Security;
using Security.Identity;
using Security.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string CatalogueConnectionString = builder.Configuration.GetConnectionString("CatalogueConnectionString");

string SecurityConnectionString = builder.Configuration.GetConnectionString("Security");

builder.Services.AddDbContext<CatalogueContext>(optionsAction => optionsAction.UseSqlServer(CatalogueConnectionString));

builder.Services.AddDbContext<SecurityContext>(optionsAction => optionsAction.UseSqlServer(SecurityConnectionString));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(optionAction =>
{
    optionAction.Password.RequireDigit = true;
    optionAction.Lockout.MaxFailedAccessAttempts = 10;
    optionAction.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
}).AddEntityFrameworkStores<SecurityContext>().AddDefaultTokenProviders();




var secretKey = builder.Configuration["jwt:SecretKey"];


builder.Services.
    AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(optionsAction =>
{
    optionsAction.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero,

    };

});
builder.Services.AddSwaggerGen(options =>
{
    // 1️⃣ Add JWT Authentication definition
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token like this: Bearer {your token}"
    });

    // 2️⃣ Require JWT globally (optional but recommended)
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});



//builder.Services.AddAuthorization(optaion =>
//{
//    optaion.AddPolicy("admin", policy => policy.RequireClaim("roles", "admin"));
   


//});

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryApplicationServiceContract, CategoryApplication>();
builder.Services.AddScoped<IAuth, Auth>();
builder.Services.AddScoped<IUserService, UserServices>();
builder.Services.AddScoped<IRoleServices, RoleServices>();
builder.Services.AddScoped<IUserRoleServices, UserRoleServices>();
builder.Services.AddScoped<Application.Contrast.Repositories.IUserManegment, Infrastracture.Security.Repository.Usermanagement>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductQueryServices, ProductQueries>();
builder.Services.AddScoped<IProductApplicationServiceContract, ProductApplication>();
builder.Services.AddScoped<IFeatureRepository, FeatureRepository>();
builder.Services.AddScoped<IFeatureQueryServices, FeatureQueries>();
builder.Services.AddScoped<IFeatureApplicationServiceContract, FeatureApplication>();
builder.Services.AddScoped<IProductMediaRepository, ProductMediaRepository>();
builder.Services.AddScoped<IProductMediaService, ProductMediaService>();
builder.Services.AddScoped<IFileStorage, FileStorage>();

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
app.UseStaticFiles();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.Run();
