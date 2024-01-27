using Flashcards.WebAPI.Settings;
using MongoDB.Driver;

namespace Flashcards.WebAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();

			builder.Services.AddSingleton(serviceProvider =>
			{
				var configuration = serviceProvider.GetService<IConfiguration>();

				var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
				var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

				var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);

				return mongoClient.GetDatabase(serviceSettings.ServiceName);
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}