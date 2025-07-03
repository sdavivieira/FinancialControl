using FinancialControl.Application.Interface;
using FinancialControl.Application.Service;
using FinancialControl.Application.UseCase;
using FinancialControl.Domain.Interfaces.Expenses;
using FinancialControl.Domain.Interfaces.Generic;
using FinancialControl.Domain.Interfaces.Profiles;
using FinancialControl.Domain.Interfaces.Users;
using FinancialControl.Domain.Models;
using FinancialControl.Infrastructure.Repositories;

namespace FinancialControl.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection DependencyInjectionServices(this IServiceCollection services)
        {
            services.AddScoped<IUserValidator, UserValidator>();
            services.AddScoped<IUserReadRepository, UserRepository>();
            services.AddScoped<IUserWriteRepository, UserRepository>();
            services.AddScoped<IProfileReadRepository, ProfileRepository>();
            services.AddScoped<IProfileWriteRepository, ProfileRepository>();
            services.AddScoped<IExpenseTypeReadRepository, ExpenseTypeRepository>();
            services.AddScoped<IExpenseTypeWriteRepository, ExpenseTypeRepository>();
            services.AddScoped<IExpenseWriteRepository, ExpenseRepository>();
            services.AddScoped<IExpenseReadRepository, ExpenseRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IExpenseTypeService, ExpenseTypeService>();

            services.AddScoped<IReadRepository<User>, GenericRepository<User>>();
            services.AddScoped<IWriteRepository<User>, GenericRepository<User>>();
            services.AddScoped<IReadRepository<Profile>, GenericRepository<Profile>>();
            services.AddScoped<IWriteRepository<Profile>, GenericRepository<Profile>>();
            services.AddScoped<IReadRepository<ExpenseType>, GenericRepository<ExpenseType>>();
            services.AddScoped<IWriteRepository<ExpenseType>, GenericRepository<ExpenseType>>();
            services.AddScoped<IWriteRepository<Expense>, GenericRepository<Expense>>();
            services.AddScoped<IReadRepository<Expense>, GenericRepository<Expense>>();

            return services;
        }
    }
}
