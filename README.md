# UnoRedisAssistant

A desktop Redis GUI application for visual management and monitoring, built with Uno Platform.

## Features

- **Connection Management**: Connect to Redis servers with custom host, port, password, and database selection
- **Key Browsing**: Search and browse Redis keys with pattern matching (e.g., `user:*`)
- **Key Details**: View key type, value, and TTL (Time To Live)
- **Key Operations**: Delete keys with confirmation dialog
- **Server Monitoring**: Display important Redis server information including:
  - Redis version
  - Operating system
  - Uptime
  - Connected clients
  - Memory usage
  - Connection and command statistics
- **Multiple Data Types**: Support for String, List, Set, and Hash types

## Technologies

- **Uno Platform**: Cross-platform UI framework
- **.NET 9.0**: Latest .NET framework
- **StackExchange.Redis**: High-performance Redis client
- **WPF/WinUI**: Desktop rendering via Skia

## Prerequisites

- .NET 9.0 SDK or later
- Redis server (local or remote)

## Getting Started

### Building the Application

```bash
cd UnoRedisAssistant
dotnet build
```

### Running the Application

```bash
cd UnoRedisAssistant
dotnet run
```

Or use your preferred IDE (Visual Studio, VS Code, Rider).

## Usage

1. **Connect to Redis**:
   - Enter the Redis server host (default: localhost)
   - Enter the port (default: 6379)
   - Optionally enter a password
   - Select the database number (default: 0)
   - Click "Connect"

2. **Browse Keys**:
   - Use the search box with pattern matching (e.g., `*`, `user:*`, `session:*`)
   - Click "Search" to load matching keys
   - Keys appear in the middle panel

3. **View Key Details**:
   - Click on any key in the list
   - Key details (type, TTL, value) appear in the right panel

4. **Delete Keys**:
   - Select a key
   - Click "Delete Key"
   - Confirm the deletion

5. **Monitor Server**:
   - Server information is displayed in the left panel when connected

## Project Structure

```
UnoRedisAssistant/
├── Models/              # Data models
│   ├── RedisConnection.cs
│   └── RedisKeyInfo.cs
├── Services/            # Business logic
│   └── RedisService.cs
├── ViewModels/          # View models
│   └── MainViewModel.cs
├── MainPage.xaml        # Main UI
└── MainPage.xaml.cs     # UI code-behind
```

## License

MIT License - feel free to use this project for your own purposes.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.