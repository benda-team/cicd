using Elastic.Apm.NetCoreAll;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ODTradePromotion.API.Helper;
using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Models;
using ODTradePromotion.API.Services.Base;
using ODTradePromotion.API.Services.Budget;
using ODTradePromotion.API.Services.Common;
using ODTradePromotion.API.Services.Customer;
using ODTradePromotion.API.Services.ExternalCheckBudgetService;
using ODTradePromotion.API.Services.Promotion;
using ODTradePromotion.API.Services.Promotion.Report;
using ODTradePromotion.API.Services.RegisterPromotion;
using ODTradePromotion.API.Services.SalesOrg;
using ODTradePromotion.API.Services.Settlement;
using ODTradePromotion.API.Services.SystemUrl;
using ODTradePromotion.API.Services.TempOrder;
using ODTradePromotion.API.Services.TpDiscount;

using ODTradePromotion.API.Services.User;
using Sys.Common;
using Sys.Common.Helper;
using Sys.Common.Models;
using System;
using System.Reflection.Metadata;
using ODTradePromotion.API.Constants;
using ODTradePromotion.API.Services.UserWithDistributor;
using ODTradePromotion.API.HttpClients;
using ODTradePromotion.API.Services.Principals;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ODTradePromotion.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddLogging();
            #region AutoMapper
            services.AddAutoMapper(typeof(Startup).Assembly);
            #endregion
            var connectStrings = Environment.GetEnvironmentVariable("CONNECTION");
            if (connectStrings == null || connectStrings == string.Empty)
                connectStrings = Configuration.GetConnectionString("DefaultConnection");
            CoreDependency.InjectDependencies(services, connectStrings);
            services.AddHealthChecks();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddSingleton<IFirebaseHelper, FirebaseHelper>();
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseNpgsql(connectStrings));
            services.AddControllers();

            #region local service
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IExternalService, ExternalService>();
            services.AddScoped<IPromotionSyntheticReportService, PromotionSyntheticReportService>();
            services.AddScoped<IPromotionDetailReportPointSaleService, PromotionDetailReportPointSaleService>();
            services.AddScoped<IPromotionDetailReportRouteZoneService, PromotionDetailReportRouteZoneService>();
            services.AddScoped<IPromotionDetailReportOrderService, PromotionDetailReportOrderService>();
            services.AddScoped<IPromotionSyntheticReportSettlementService, PromotionSyntheticReportSettlementService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IBudgetService, BudgetService>();
            services.AddScoped<IBudgetAdjustmentService, BudgetAdjustmentService>();
            services.AddScoped<ISettlementService, SettlementService>();
            services.AddScoped<ISystemUrlService, SystemUrlService>();
            services.AddScoped<ISalesOrgService, SalesOrgService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITempTpOrderService, TempTpOrderService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IDapperRepositories, DapperRepositories>();
            services.AddHostedService<ConsumeScopedServiceHostedService>();
            services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
            services.AddScoped<ICheckBudgetService, CheckBudgetService>();
            services.AddScoped<IResgiterPromotion, ResgiterPromotion>();
            services.AddScoped<IOrdinalNumberRegistrationService, OrdinalNumberRegistrationService>();
            services.AddScoped<IExternalCheckBudgetService, ExternalCheckBudgetService>();
            services.AddSingleton<IHttpClientEcoGateway, HttpClientEcoGateway>();
            services.AddTransient<HttpClientEcoGateway>();
            services.AddSingleton<IHttpClientCommon, HttpClientCommon>();
            services.AddScoped<HttpClientCommon>();
            services.AddScoped<IUserWithDistributorService, UserWithDistributorService>();
            services.AddScoped<IPrincipalService, PrincipalService>();
            services.AddScoped<IPromotionExternalService, PromotionExternalService>();
            
            #endregion

            GetSystemUrls();
            AppConstant.CommonApiBaseUrl = 
                AppConstant.SystemUrlConstant?
                .Where(d => d.Code == SystemUrlCode.ODCommonAPI)
                .Select(d => d.Url)
                .FirstOrDefault();
        }

        private static void GetSystemUrls()
        {
            SystemUrlService systemUrlService = new();
            SystemUrlListModel result = systemUrlService.GetAllSystemUrl().Result;
            AppConstant.SystemUrlConstant = result.Items;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider, ApplicationDbContext _context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ODTradePromotion.API v1"));
            }
            ListServices services = new ListServices()
            {
                App = app,
                Env = env,
                Provider = provider
            };
            //_context.Database.Migrate();
            CoreDependency.Configure(services);

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseHealthChecks("/ping");
            //app.UseAuthorization();
            app.UseAllElasticApm(Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
