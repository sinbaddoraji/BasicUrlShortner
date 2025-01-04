using MongoDB.Driver;
using StackExchange.Redis;
using UrlShortner.Interface;
using Microsoft.Extensions.Logging;
using UrlShortner.Model;

namespace UrlShortner.Implementation;

public class UrlRepository : IUrlRepository
{
    private readonly IDatabase _redisDatabase;
    private readonly IMongoCollection<UrlMapping> _mongoCollection;
    private readonly ILogger<UrlRepository> _logger;

    private readonly TimeSpan _defaultTtl = TimeSpan.FromMinutes(10); // Default TTL: 10 minutes

    public UrlRepository(IConnectionMultiplexer redis, IMongoDatabase mongoDatabase, ILogger<UrlRepository> logger)
    {
        _redisDatabase = redis.GetDatabase();
        _mongoCollection = mongoDatabase.GetCollection<UrlMapping>("UrlMappings");
        _logger = logger;
    }

    public async Task<string> GetUrlAsync(string key)
    {
        try
        {
            // Check Redis cache
            var cachedUrl = await _redisDatabase.StringGetAsync(key);
            if (!string.IsNullOrEmpty(cachedUrl))
            {
                _logger.LogInformation($"Cache hit for key: {key}");

                // Reset TTL for frequently accessed keys
                await _redisDatabase.KeyExpireAsync(key, _defaultTtl);
                _logger.LogInformation($"TTL reset for key: {key} to {_defaultTtl.TotalMinutes} minutes");

                return cachedUrl;
            }

            _logger.LogWarning($"Cache miss for key: {key}. Falling back to MongoDB.");

            // Fallback to MongoDB
            var urlMapping = await _mongoCollection.Find(x => x.Key == key).FirstOrDefaultAsync();
            if (urlMapping != null)
            {
                // Cache the URL in Redis
                await _redisDatabase.StringSetAsync(key, urlMapping.Url, _defaultTtl);
                _logger.LogInformation($"URL cached in Redis: {key} -> {urlMapping.Url} with TTL {_defaultTtl.TotalMinutes} minutes");
                return urlMapping.Url;
            }

            _logger.LogWarning($"Key not found in MongoDB: {key}");
            return null; // Key not found
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error while getting URL for key: {key}");
            throw;
        }
    }

    public async Task<string> SetUrlAsync(string url)
    {
        // Generate a unique key
        string key = Guid.NewGuid().ToString().Substring(0, 6);

        try
        {
            // Save to MongoDB
            var urlMapping = new UrlMapping { Key = key, Url = url };
            await _mongoCollection.ReplaceOneAsync(
                filter: x => x.Key == key,
                replacement: urlMapping,
                options: new ReplaceOptions { IsUpsert = true });

            _logger.LogInformation($"URL saved to MongoDB: {key} -> {url}");

            // Save to Redis
            await _redisDatabase.StringSetAsync(key, url, _defaultTtl);
            _logger.LogInformation($"URL cached in Redis: {key} -> {url} with TTL {_defaultTtl.TotalMinutes} minutes");

            return key;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error while setting URL for key: {key}");
            throw;
        }
    }
}
