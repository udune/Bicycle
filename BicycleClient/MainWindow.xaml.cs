using System.Windows;
using System.Windows.Input;
using Microsoft.AspNetCore.SignalR.Client;

namespace BicycleClient;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    HubConnection hubConnection;
    
    public MainWindow()
    {
        InitializeComponent();
        hubConnection = new HubConnectionBuilder()
            .WithUrl("https://bicycleminchan.azurewebsites.net/chat")
            .WithAutomaticReconnect()
            .Build();

        hubConnection.On<string, string>("receive", (msg, userId) =>
        {
            Dispatcher.Invoke(() =>
            {
                string message = $"{userId}의 메시지: {msg}";
                chatLog.Items.Add(message);
            });
        });
    }
    
    private async void btnSend_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        await hubConnection.InvokeAsync("Send", textBoxMessage.Text, "관리자");
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            btnSend.IsEnabled = false;
            await hubConnection.StartAsync();

            chatLog.Items.Add("안녕하세요. 따릉따릉 챗봇입니다.");
            btnSend.IsEnabled = true;
        }
        catch (Exception)
        {
            chatLog.Items.Add("서버와의 연결 실패");
            throw;
        }
    }
}