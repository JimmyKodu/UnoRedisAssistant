namespace UnoRedisAssistant.Models;

public class RedisKeyInfo
{
    public string Key { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public long TimeToLive { get; set; } = -1;
    public string? Value { get; set; }
}
