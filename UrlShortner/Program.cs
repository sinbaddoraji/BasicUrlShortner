using Keycloak.AuthServices.Authentication;
using MongoDB.Driver;
using StackExchange.Redis;
using UrlShortner.Implementation;
using UrlShortner.Interface;

namespace UrlShortner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //builder.Services.AddKeycloakAuthentication(builder.Configuration);
            //builder.Services.AddAuthorization();

            // Redis configuration
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(builder.Configuration["Redis:ConnectionString"]));


            // Configure MongoDB client
            var mongoClient = new MongoClient(builder.Configuration["Mongo:Address"]);
            var database = mongoClient.GetDatabase(builder.Configuration["Mongo:Database"]);

            builder.Services.AddSingleton(database);

            // Register the repository
            builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

            // Register the redis repository
            builder.Services.AddScoped<IUrlRepository, UrlRepository>();

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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
