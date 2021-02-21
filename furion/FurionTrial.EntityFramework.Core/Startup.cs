﻿using Furion;
using Microsoft.Extensions.DependencyInjection;

namespace FurionTrial.EntityFramework.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseAccessor(options =>
            {
                options.AddDbPool<DefaultDbContext>();
            }, "FurionTrial.Database.Migrations");
        }
    }
}