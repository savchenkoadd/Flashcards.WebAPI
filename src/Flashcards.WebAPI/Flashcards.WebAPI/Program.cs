using Flashcards.Core.Domain.Entities;
using Flashcards.Core.Domain.Identity;
using Flashcards.Core.Domain.RepositoryContracts;
using Flashcards.Core.ServiceContracts;
using Flashcards.Core.Services;
using Flashcards.Infrastructure.Db;
using Flashcards.Infrastructure.Repositories;
using Flashcards.WebAPI.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Flashcards.WebAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();

			builder.Services.AddScoped(serviceProvider =>
			{
				var configuration = serviceProvider.GetService<IConfiguration>();

				var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
				var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

				var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);

				return mongoClient.GetDatabase(serviceSettings.ServiceName);
			});

			builder.Services.AddScoped<IRepository<Flashcard>, CardRepository>();
			builder.Services.AddScoped<ICardService, CardService>();

			//Identity
			builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
			{
				//TO DO: move to configuration 
				options.Password.RequiredLength = 8;
			})
				.AddEntityFrameworkStores<AccountDbContext>()
				.AddDefaultTokenProviders()
				.AddUserStore<UserStore<ApplicationUser, ApplicationRole, AccountDbContext, Guid>>()
				.AddRoleStore<RoleStore<ApplicationRole, AccountDbContext, Guid>>();

			builder.Services.AddDbContext<AccountDbContext>(options =>
			{
				var sqlServerSettings = builder.Configuration.GetSection(nameof(SqlServerSettings)).Get<SqlServerSettings>();

				options.UseSqlServer(sqlServerSettings.ConnectionString);
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			app.UseHsts();
			app.UseHttpsRedirection();

			app.UseRouting();
			app.UseCors();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}