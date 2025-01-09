using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.Interface.IRepositories;
using Todo.Application.Interface.IServices;
using Todo.Domain.Entities;
using Todo.Infrastructure.Context;
using Todo.Infrastructure.Repositories;
using Todo.Infrastructure.Services;

namespace Todo.Infrastructure.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("TodoDb"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)),
            ServiceLifetime.Transient);

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddDbContext<AppDbContext>();

            #region Repositories
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IListRepository, ListRepository>();
            services.AddTransient<ISubTasksRepository, SubTasksRepository>();
            services.AddTransient<ITasksRepository, TasksRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            #endregion

            #region Services
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IListService, ListService>();
            services.AddTransient<ISubTasksService, SubTasksService>();
            services.AddTransient<ITasksService, TasksService>();
            services.AddTransient<IUserService, UserService>();
            #endregion

            //services.AddSingleton<IUserIdProvider, UserIdProvider>();
            //services.AddSignalR().AddJsonProtocol(options =>
            //{
            //    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
            //});

            return services;
        }
    }
}
