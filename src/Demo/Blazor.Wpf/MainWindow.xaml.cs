using System.Net.Http;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Shipwreck.ViewModelUtils.Demo.Blazor
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWpfBlazorWebView();
            serviceCollection.AddBlazorWebViewDeveloperTools(); 
            serviceCollection.AddScoped(sp => new HttpClient { BaseAddress = new Uri("/") });
            serviceCollection.AddScoped(sp => new InteractionService());
            var sp = serviceCollection.BuildServiceProvider();

            Resources.Add("services", sp);
        }

        private async void webView_Loaded(object sender, RoutedEventArgs e)
        {
            await webView.WebView.EnsureCoreWebView2Async();

            var cwv = webView.WebView.CoreWebView2;

            cwv.WebResourceResponseReceived += (s, e) =>
            {
                if (e.Request.Uri?.StartsWith("data:") == false)
                {
                    Debug.WriteLine($"{e.Request.Method} {e.Request.Uri}: {e.Response.StatusCode}");
                }
            };
        }
    }
}
