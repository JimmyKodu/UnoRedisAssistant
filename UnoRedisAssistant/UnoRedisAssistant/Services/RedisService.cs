using StackExchange.Redis;
using UnoRedisAssistant.Models;

namespace UnoRedisAssistant.Services;

public class RedisService : IDisposable
{
    private ConnectionMultiplexer? _connection;
    private IDatabase? _database;
    
    public bool IsConnected => _connection?.IsConnected ?? false;
    
    public async Task<bool> ConnectAsync(RedisConnection connection)
    {
        try
        {
            var configOptions = new ConfigurationOptions
            {
                EndPoints = { { connection.Host, connection.Port } },
                AbortOnConnectFail = false,
                ConnectTimeout = 5000,
            };
            
            if (!string.IsNullOrWhiteSpace(connection.Password))
            {
                configOptions.Password = connection.Password;
            }
            
            _connection = await ConnectionMultiplexer.ConnectAsync(configOptions);
            _database = _connection.GetDatabase(connection.Database);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public void Disconnect()
    {
        _connection?.Close();
        _connection = null;
        _database = null;
    }
    
    public async Task<List<string>> GetKeysAsync(string pattern = "*", int count = 100)
    {
        if (_connection == null || _database == null) return new List<string>();
        
        var keys = new List<string>();
        var server = _connection.GetServer(_connection.GetEndPoints().First());
        
        await foreach (var key in server.KeysAsync(_database.Database, pattern, count))
        {
            keys.Add(key.ToString());
        }
        
        return keys;
    }
    
    public async Task<RedisKeyInfo?> GetKeyInfoAsync(string key)
    {
        if (_database == null) return null;
        
        var type = await _database.KeyTypeAsync(key);
        var ttl = await _database.KeyTimeToLiveAsync(key);
        
        var keyInfo = new RedisKeyInfo
        {
            Key = key,
            Type = type.ToString(),
            TimeToLive = (long)(ttl?.TotalSeconds ?? -1),
        };
        
        switch (type)
        {
            case RedisType.String:
                keyInfo.Value = await _database.StringGetAsync(key);
                break;
            case RedisType.List:
                var list = await _database.ListRangeAsync(key, 0, 99);
                keyInfo.Value = string.Join("\n", list.Select(x => x.ToString()));
                break;
            case RedisType.Set:
                var set = await _database.SetMembersAsync(key);
                keyInfo.Value = string.Join("\n", set.Select(x => x.ToString()));
                break;
            case RedisType.Hash:
                var hash = await _database.HashGetAllAsync(key);
                keyInfo.Value = string.Join("\n", hash.Select(x => $"{x.Name}: {x.Value}"));
                break;
            default:
                keyInfo.Value = $"Type: {type}";
                break;
        }
        
        return keyInfo;
    }
    
    public async Task<bool> DeleteKeyAsync(string key)
    {
        if (_database == null) return false;
        return await _database.KeyDeleteAsync(key);
    }
    
    public async Task<Dictionary<string, string>> GetServerInfoAsync()
    {
        if (_connection == null) return new Dictionary<string, string>();
        
        var server = _connection.GetServer(_connection.GetEndPoints().First());
        var info = await server.InfoAsync();
        
        var result = new Dictionary<string, string>();
        foreach (var section in info)
        {
            foreach (var item in section)
            {
                result[$"{section.Key}:{item.Key}"] = item.Value;
            }
        }
        
        return result;
    }
    
    public void Dispose()
    {
        Disconnect();
    }
}
