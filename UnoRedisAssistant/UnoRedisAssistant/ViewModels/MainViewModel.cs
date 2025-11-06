using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnoRedisAssistant.Models;
using UnoRedisAssistant.Services;

namespace UnoRedisAssistant.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly RedisService _redisService;
    
    private string _host = "localhost";
    private int _port = 6379;
    private string _password = string.Empty;
    private int _database = 0;
    private bool _isConnected;
    private string _statusMessage = "Not connected";
    private string _selectedKey = string.Empty;
    private string _keyValue = string.Empty;
    private string _keyType = string.Empty;
    private string _keyTtl = string.Empty;
    private string _searchPattern = "*";
    
    public string Host
    {
        get => _host;
        set { _host = value; OnPropertyChanged(); }
    }
    
    public int Port
    {
        get => _port;
        set { _port = value; OnPropertyChanged(); }
    }
    
    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }
    
    public int Database
    {
        get => _database;
        set { _database = value; OnPropertyChanged(); }
    }
    
    public bool IsConnected
    {
        get => _isConnected;
        set { _isConnected = value; OnPropertyChanged(); }
    }
    
    public string StatusMessage
    {
        get => _statusMessage;
        set { _statusMessage = value; OnPropertyChanged(); }
    }
    
    public string SelectedKey
    {
        get => _selectedKey;
        set { _selectedKey = value; OnPropertyChanged(); }
    }
    
    public string KeyValue
    {
        get => _keyValue;
        set { _keyValue = value; OnPropertyChanged(); }
    }
    
    public string KeyType
    {
        get => _keyType;
        set { _keyType = value; OnPropertyChanged(); }
    }
    
    public string KeyTtl
    {
        get => _keyTtl;
        set { _keyTtl = value; OnPropertyChanged(); }
    }
    
    public string SearchPattern
    {
        get => _searchPattern;
        set { _searchPattern = value; OnPropertyChanged(); }
    }
    
    public ObservableCollection<string> Keys { get; } = new();
    public ObservableCollection<string> ServerInfo { get; } = new();
    
    public MainViewModel()
    {
        _redisService = new RedisService();
    }
    
    public async Task ConnectAsync()
    {
        try
        {
            var connection = new RedisConnection
            {
                Host = Host,
                Port = Port,
                Password = string.IsNullOrWhiteSpace(Password) ? null : Password,
                Database = Database
            };
            
            var success = await _redisService.ConnectAsync(connection);
            
            if (success)
            {
                IsConnected = true;
                StatusMessage = $"Connected to {Host}:{Port}";
                await LoadKeysAsync();
                await LoadServerInfoAsync();
            }
            else
            {
                StatusMessage = "Failed to connect";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
    }
    
    public void Disconnect()
    {
        _redisService.Disconnect();
        IsConnected = false;
        StatusMessage = "Disconnected";
        Keys.Clear();
        ServerInfo.Clear();
        ClearKeyDetails();
    }
    
    public async Task LoadKeysAsync()
    {
        try
        {
            var keys = await _redisService.GetKeysAsync(SearchPattern);
            Keys.Clear();
            foreach (var key in keys)
            {
                Keys.Add(key);
            }
            StatusMessage = $"Loaded {keys.Count} keys";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading keys: {ex.Message}";
        }
    }
    
    public async Task LoadKeyDetailsAsync(string key)
    {
        try
        {
            var keyInfo = await _redisService.GetKeyInfoAsync(key);
            if (keyInfo != null)
            {
                SelectedKey = keyInfo.Key;
                KeyType = keyInfo.Type;
                KeyValue = keyInfo.Value ?? string.Empty;
                KeyTtl = keyInfo.TimeToLive >= 0 
                    ? $"{keyInfo.TimeToLive} seconds" 
                    : "No expiration";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading key: {ex.Message}";
        }
    }
    
    public async Task DeleteKeyAsync(string key)
    {
        try
        {
            var success = await _redisService.DeleteKeyAsync(key);
            if (success)
            {
                StatusMessage = $"Deleted key: {key}";
                await LoadKeysAsync();
                ClearKeyDetails();
            }
            else
            {
                StatusMessage = $"Failed to delete key: {key}";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error deleting key: {ex.Message}";
        }
    }
    
    private async Task LoadServerInfoAsync()
    {
        try
        {
            var info = await _redisService.GetServerInfoAsync();
            ServerInfo.Clear();
            
            // Show important server info
            var importantKeys = new[] 
            { 
                "Server:redis_version", 
                "Server:os", 
                "Server:uptime_in_days",
                "Clients:connected_clients",
                "Memory:used_memory_human",
                "Stats:total_connections_received",
                "Stats:total_commands_processed"
            };
            
            foreach (var key in importantKeys)
            {
                if (info.TryGetValue(key, out var value))
                {
                    ServerInfo.Add($"{key.Replace(":", " - ")}: {value}");
                }
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading server info: {ex.Message}";
        }
    }
    
    private void ClearKeyDetails()
    {
        SelectedKey = string.Empty;
        KeyValue = string.Empty;
        KeyType = string.Empty;
        KeyTtl = string.Empty;
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
