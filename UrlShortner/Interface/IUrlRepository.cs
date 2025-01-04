namespace UrlShortner.Interface;

public interface IUrlRepository
{
    /// <summary>
    /// Retrieves the URL associated with the given key.
    /// First checks Redis, and if not found, queries MongoDB.
    /// </summary>
    /// <param name="key">The unique key associated with the URL.</param>
    /// <returns>The URL if found, otherwise null.</returns>
    Task<string> GetUrlAsync(string key);

    /// <summary>
    /// Stores a key-URL pair in both Redis and MongoDB.
    /// </summary>
    /// <param name="key">The unique key to associate with the URL.</param>
    /// <param name="url">The URL to store.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<string> SetUrlAsync(string url);
}