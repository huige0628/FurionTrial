using Furion;
using FurionTrial.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FurionTrial.Web.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //JWT验证
            services.AddJwt<JwtHandler>(enableGlobalAuthorize:true);

            //跨域
            services.AddCorsAccessor();

            //自定义返回模型
            //services.AddControllers().AddInjectWithUnifyResult<FurionTrialRESTfulResultProvider>();

            //默认返回模型
            services.AddControllers().AddInjectWithUnifyResult();

            //SqlSugar注入
            services.AddSqlSugar(new SqlSugar.ConnectionConfig
            {
                ConnectionString = App.Configuration["SqlServerDataSource:ConnectionString"],
                DbType = SqlSugar.DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = SqlSugar.InitKeyType.Attribute
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCorsAccessor();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseInject(string.Empty);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}