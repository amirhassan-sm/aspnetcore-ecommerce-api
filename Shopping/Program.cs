using Application.Contrast.QueryServices;
using Application.Contrast.Repositories;
using Application.Contrast.Services;
using Application.implementation;
using Application.Validators.Category;
using Catalogue.DomainServiceContract.Service;
using FluentValidation;
using Infrastracture.Catalogue.EF.Persistance;
using Infrastracture.Catalogue.EF.Persistance.FileStorage;
using Infrastracture.Catalogue.EF.Persistance.Querie;
using Infrastracture.Catalogue.EF.Persistance.Repositories;
using Infrastracture.Security.Repository;
using Infrastructure.Sale;
using Infrastructure.Sale.repositry;
using Sale.DomainServiceContract.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Security;
using Security.Identity;
using Security.Repository;
using Shopping.Security;
using System.Security.Claims;
using Shopping;
using System.Text;
using Shopping.mediatype;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

// Add services to the container.

string CatalogueConnectionString = builder.Configuration.GetConnectionString("CatalogueConnectionString");

string SecurityConnectionString = builder.Configuration.GetConnectionString("Security");
string SaleSystemConnectionString = builder.Configuration.GetConnectionString("Sale");

builder.Services.AddDbContext<CatalogueContext>(optionsAction => optionsAction.UseSqlServer(CatalogueConnectionString));

builder.Services.AddDbContext<SecurityContext>(optionsAction => optionsAction.UseSqlServer(SecurityConnectionString));
builder.Services.AddDbContext<SaleSystemContext>(optionsAction =>optionsAction.UseSqlServer(SaleSystemConnectionString));

builder.Services.AddIdentityCore<ApplicationUser>(optionAction =>
{
    optionAction.Password.RequireDigit = true;
    optionAction.Lockout.MaxFailedAccessAttempts = 10;
    optionAction.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
})
.AddRoles<ApplicationRole>()
.AddEntityFrameworkStores<SecurityContext>()
.AddSignInManager()
.AddDefaultTokenProviders();

var secretKey = builder.Configuration["jwt:SecretKey"]
    ?? throw new InvalidOperationException("jwt:SecretKey is missing from configuration.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(optionsAction =>
    {
        optionsAction.RequireHttpsMetadata = false;
        optionsAction.SaveToken = true;
        optionsAction.MapInboundClaims = false;
        optionsAction.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.FromMinutes(2),
            NameClaimType = ClaimTypes.NameIdentifier,
            RoleClaimType = ClaimTypes.Role,
        };
        optionsAction.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"[JWT] Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine($"[JWT] Challenge: {context.Error} - {context.ErrorDescription}");
                return Task.CompletedTask;
            },
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
builder.Services.AddScoped<IBasketQueryService, BasketQueryService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderManagament, OrderManagament>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();

//builder.Services.AddHttpClient("shop", client =>
//{
//    client.BaseAddress = Uri();
//});
builder.Services.AddScoped<ValidationFilter>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
}).AddJsonOptions(json =>
{
    // Keep C# property names (PascalCase + intentional DTO typos like pageSize)
    json.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddValidatorsFromAssemblyContaining<CategoryAddModelValidator>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var allowedOrigins = (builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [])
    .Select(origin => origin.Trim().TrimEnd('/'))
    .Where(origin => origin.Length > 0)
    .ToHashSet(StringComparer.OrdinalIgnoreCase);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .SetIsOriginAllowed(origin =>
            {
                if (string.IsNullOrWhiteSpace(origin)) return false;
                if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri)) return false;

                var normalized = $"{uri.Scheme}://{uri.Authority}".TrimEnd('/');
                if (allowedOrigins.Contains(normalized)) return true;

                return uri.Host is "localhost" or "127.0.0.1"
                    || uri.Host.StartsWith("192.168.")
                    || uri.Host.StartsWith("10.");
            })
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});




var app = builder.Build();

if (string.IsNullOrWhiteSpace(app.Environment.WebRootPath))
{
    app.Environment.WebRootPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot");
}
Directory.CreateDirectory(Path.Combine(app.Environment.WebRootPath, "images", "products"));

await DatabaseMigrator.MigrateAsync(app.Services);
await IdentityDataSeeder.SeedAsync(app.Services);
await mediaTypeDataSeeder.seedAsync(app.Services);

app.UseForwardedHeaders();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("Frontend");
app.UseStaticFiles();
app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();



app.Run();
