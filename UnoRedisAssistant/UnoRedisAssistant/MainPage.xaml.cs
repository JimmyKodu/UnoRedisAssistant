using UnoRedisAssistant.ViewModels;

namespace UnoRedisAssistant;

public sealed partial class MainPage : Page
{
    private readonly MainViewModel _viewModel;
    
    public MainPage()
    {
        this.InitializeComponent();
        _viewModel = new MainViewModel();
        
        // Initialize UI with default values
        HostTextBox.Text = _viewModel.Host;
        PortTextBox.Text = _viewModel.Port.ToString();
        DatabaseTextBox.Text = _viewModel.Database.ToString();
        SearchTextBox.Text = _viewModel.SearchPattern;
        
        // Bind collections
        KeysList.ItemsSource = _viewModel.Keys;
        ServerInfoList.ItemsSource = _viewModel.ServerInfo;
    }
    
    private async void ConnectButton_Click(object sender, RoutedEventArgs e)
    {
        // Update viewmodel from UI
        _viewModel.Host = HostTextBox.Text;
        if (int.TryParse(PortTextBox.Text, out var port))
            _viewModel.Port = port;
        _viewModel.Password = PasswordBox.Password;
        if (int.TryParse(DatabaseTextBox.Text, out var db))
            _viewModel.Database = db;
        
        ConnectButton.IsEnabled = false;
        StatusTextBlock.Text = "Connecting...";
        
        await _viewModel.ConnectAsync();
        
        ConnectButton.IsEnabled = !_viewModel.IsConnected;
        DisconnectButton.IsEnabled = _viewModel.IsConnected;
        StatusTextBlock.Text = _viewModel.StatusMessage;
    }
    
    private void DisconnectButton_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.Disconnect();
        ConnectButton.IsEnabled = true;
        DisconnectButton.IsEnabled = false;
        StatusTextBlock.Text = _viewModel.StatusMessage;
        ClearKeyDetails();
    }
    
    private async void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        if (!_viewModel.IsConnected) return;
        
        _viewModel.SearchPattern = SearchTextBox.Text;
        SearchButton.IsEnabled = false;
        StatusTextBlock.Text = "Searching...";
        
        await _viewModel.LoadKeysAsync();
        
        SearchButton.IsEnabled = true;
        StatusTextBlock.Text = _viewModel.StatusMessage;
    }
    
    private async void KeysList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (KeysList.SelectedItem is string key)
        {
            StatusTextBlock.Text = "Loading key details...";
            await _viewModel.LoadKeyDetailsAsync(key);
            
            KeyNameTextBox.Text = _viewModel.SelectedKey;
            KeyTypeTextBox.Text = _viewModel.KeyType;
            KeyTtlTextBox.Text = _viewModel.KeyTtl;
            KeyValueTextBox.Text = _viewModel.KeyValue;
            
            StatusTextBlock.Text = _viewModel.StatusMessage;
        }
    }
    
    private async void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        if (!_viewModel.IsConnected) return;
        
        RefreshButton.IsEnabled = false;
        StatusTextBlock.Text = "Refreshing...";
        
        await _viewModel.LoadKeysAsync();
        
        RefreshButton.IsEnabled = true;
        StatusTextBlock.Text = _viewModel.StatusMessage;
    }
    
    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_viewModel.SelectedKey)) return;
        
        var dialog = new ContentDialog
        {
            Title = "Confirm Delete",
            Content = $"Are you sure you want to delete the key '{_viewModel.SelectedKey}'?",
            PrimaryButtonText = "Delete",
            CloseButtonText = "Cancel",
            XamlRoot = this.XamlRoot
        };
        
        var result = await dialog.ShowAsync();
        
        if (result == ContentDialogResult.Primary)
        {
            DeleteButton.IsEnabled = false;
            var key = _viewModel.SelectedKey;
            await _viewModel.DeleteKeyAsync(key);
            DeleteButton.IsEnabled = true;
            StatusTextBlock.Text = _viewModel.StatusMessage;
        }
    }
    
    private void ClearKeyDetails()
    {
        KeyNameTextBox.Text = string.Empty;
        KeyTypeTextBox.Text = string.Empty;
        KeyTtlTextBox.Text = string.Empty;
        KeyValueTextBox.Text = string.Empty;
    }
}
