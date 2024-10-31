using Microsoft.Maui.Controls;

namespace QbittorrentWebUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage(); // 确保这里指向正确的页面
        }
    }
}
