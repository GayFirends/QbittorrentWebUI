using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace QbittorrentWebUI
{
    public partial class MainPage : ContentPage
    {
        private HttpClient httpClient = new HttpClient();
        private ObservableCollection<Torrent> torrents;

        public MainPage()
        {
            InitializeComponent();
            torrents = new ObservableCollection<Torrent>();
            torrentsCollectionView.ItemsSource = torrents;
        }

        private async void OnConnectButtonClicked(object sender, EventArgs e)
        {
            var url = urlEntry.Text;
            var username = usernameEntry.Text;
            var password = passwordEntry.Text;

            var result = await Authenticate(url, username, password);
            resultLabel.Text = result ? "登录成功" : "登录失败";

            if (result)
            {
                await LoadTorrents(url);
            }
        }

        private async Task<bool> Authenticate(string url, string username, string password)
        {
            var response = await httpClient.PostAsync($"{url}/api/v2/auth/login",
                new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
                }));

            return response.IsSuccessStatusCode;
        }

        private async Task LoadTorrents(string url)
        {
            var response = await httpClient.GetAsync($"{url}/api/v2/torrents/info");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var torrentsList = JsonSerializer.Deserialize<List<Torrent>>(json);
                torrents.Clear();

                foreach (var torrent in torrentsList)
                {
                    torrents.Add(torrent);
                }
            }
        }

        private void OnTorrentsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle selection changes if needed
        }
    }

    public class Torrent
    {
        public string name { get; set; }
        public int num_seeds { get; set; }
        public double progress { get; set; }
        public double dl_speed { get; set; }
        public double up_speed { get; set; }
    }
}
