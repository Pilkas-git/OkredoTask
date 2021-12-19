using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using OkredoTask.Infrastructure;
using OkredoTask.Infrastructure.Data;
using OkredoTask.Infrastructure.Repositories;
using OkredoTask.Infrastructure.Repositories.Interfaces;
using OkredoTask.Infrastructure.Services;
using OkredoTask.Infrastructure.Services.Interfaces;
using OkredoTask.Web.Extensions;
using OkredoTask.Web.Options;
using System;
using System.Text;

namespace OkredoTask.Web
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:57678")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                        builder.WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddDbContext(Configuration.GetConnectionString("DefaultConnection"));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            //Add configurations
            services.Configure<JwtConfig>(Configuration.GetSection(nameof(JwtConfig)));
            services.Configure<DatabaseOptions>(Configuration.GetSection(nameof(DatabaseOptions)));
            services.Configure<AdminUserOptions>(Configuration.GetSection(nameof(AdminUserOptions)));

            //Setup authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });
            services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AppDbContext>();

            //Register repositories
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddScoped(s =>
            {
                var memoryCache = s.GetService<IMemoryCache>();
                var dbContext = s.GetService<AppDbContext>();

                IProductRepository concreteService = new ProductRepository(dbContext);
                IProductRepository cachingDecorator = new ProductRepositoryCachingDecorator(concreteService, memoryCache);

                var useCaching = Convert.ToBoolean(Configuration["EnableCaching"]);
                if (useCaching)
                {
                    return cachingDecorator;
                }

                return concreteService;
            });

            //Register Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICartItemService, CartItemService>();
            services.AddScoped<IDiscountService, DiscountService>();

            services.AddScoped<IOrderCreatorServiceFactory, OrderCreatorServiceFactory>();

            services.AddScoped<OrderCreatorStrategy, AnonymousOrderCreatorStrategy>();
            services.AddScoped<OrderCreatorStrategy, RegularOrderCreatorStrategy>();

            //Used by nlog to get user identity
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Configure swagger
            services.SetupSwagger();

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.Converters.Add(new StringEnumConverter()));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseCors();

            app.UseHttpsRedirection();

            app.UseCookiePolicy();

            app.UseSwagger();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OkredoTask API V1"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}