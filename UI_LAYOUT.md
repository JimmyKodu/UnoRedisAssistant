# UI Layout Description

## Application Window: "Redis Assistant"

### Header (Top)
- Dark blue/accent colored banner
- "Redis Assistant" title in white, large bold text
- Full-width spanning across the window

### Main Content Area (Three Columns)

#### Left Panel (300px) - Connection & Server Info
**Connection Section:**
- "Connection" heading
- Host text box (default: "localhost")
- Port text box (default: "6379")
- Password box (optional, masked input)
- Database text box (default: "0")
- "Connect" button (blue, full-width)
- "Disconnect" button (gray, full-width, disabled by default)

**Server Info Section:**
- Bordered box with rounded corners
- "Server Info" heading
- Scrollable list showing:
  - Server - redis_version: X.X.X
  - Server - os: Linux/Windows
  - Server - uptime_in_days: XX
  - Clients - connected_clients: XX
  - Memory - used_memory_human: XXX MB
  - Stats - total_connections_received: XXXXX
  - Stats - total_commands_processed: XXXXX

#### Middle Panel (Flexible Width) - Key Browser
**Header:**
- "Keys" heading
- Search bar with placeholder: "Search pattern (e.g., user:*)"
- "Search" button

**Key List:**
- Bordered box with rounded corners
- Scrollable list view
- Keys displayed in Consolas font
- Selectable items
- Example keys:
  - user:1001
  - user:1002
  - session:abc123
  - cache:products

#### Right Panel (300px) - Key Details
**Key Details Section:**
- "Key Details" heading
- Key name text box (read-only, Consolas font)
- Type text box (read-only, shows: "String", "List", "Set", "Hash")
- TTL text box (read-only, shows: "123 seconds" or "No expiration")
- Value text box (large, read-only, multi-line, scrollable)
  - Shows key content
  - Formatted based on type
  - Consolas font for better readability

**Action Buttons:**
- "Refresh" button (blue)
- "Delete Key" button (red, warning color)

### Status Bar (Bottom)
- Gray background
- Small text showing current status:
  - "Not connected" (default)
  - "Connecting..."
  - "Connected to localhost:6379"
  - "Loaded 45 keys"
  - Error messages

## Color Scheme
- **Primary**: System accent color (blue)
- **Background**: Light gray/white
- **Text**: Dark gray/black
- **Borders**: Medium gray
- **Warning**: Red for delete operations
- **Header**: Accent color with white text

## Responsive Behavior
- Three-panel layout adjusts to window size
- Middle panel expands/shrinks with window width
- Left and right panels fixed at 300px
- All sections scrollable when content overflows

## User Flow
1. User enters connection details in left panel
2. Clicks "Connect" → Server info appears below
3. Enters search pattern in middle panel
4. Clicks "Search" → Keys populate in list
5. Clicks on a key → Details show in right panel
6. Can click "Delete Key" → Confirmation dialog appears
7. Status bar updates with each operation

## Example Screenshot Description

```
+-------------------------------------------------------------------+
| [Header: Blue] Redis Assistant                                    |
+-------------------+---------------------------+-------------------+
| Connection        | Keys                      | Key Details       |
| [Host: localhost] | [Search: user:*] [Search]| Key: user:1001    |
| [Port: 6379]      | ┌─────────────────────┐  | Type: String      |
| [Password: ****]  | │ user:1001          │  | TTL: No expiration|
| [Database: 0]     | │ user:1002          │  | Value:            |
| [Connect Button]  | │ user:1003          │  | ┌───────────────┐|
| [Disconnect Btn]  | │ session:abc123     │  | │{"name":"John",│|
|                   | │ cache:products     │  | │ "age": 30}    │|
| Server Info       | └─────────────────────┘  | └───────────────┘|
| ┌───────────────┐ |                           | [Refresh] [Delete]|
| │ redis_version │ |                           |                   |
| │ uptime: 5 days│ |                           |                   |
| │ memory: 10 MB │ |                           |                   |
| └───────────────┘ |                           |                   |
+-------------------+---------------------------+-------------------+
| Status: Connected to localhost:6379 - Loaded 5 keys              |
+-------------------------------------------------------------------+
```

This layout provides an efficient, professional interface for Redis management with clear visual separation of concerns and intuitive workflow.
