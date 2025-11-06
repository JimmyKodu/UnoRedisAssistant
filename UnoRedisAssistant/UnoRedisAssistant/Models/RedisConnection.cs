namespace UnoRedisAssistant.Models;

public class RedisConnection
{
    public string Name { get; set; } = string.Empty;
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 6379;
    public string? Password { get; set; }
    public int Database { get; set; } = 0;
}
