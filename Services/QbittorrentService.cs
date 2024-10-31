using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QbittorrentWebUI.Services
{
    public class QbittorrentService
    {
        private readonly HttpClient _httpClient;
        private string _baseUrl;
        private string _username;
        private string _password;

        public QbittorrentService(string baseUrl, string username, string password)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _username = username;
            _password = password;

            // 设置基础地址和请求头
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Add("Referer", _baseUrl);
        }

        public async Task<bool> Connect()
        {
            // 创建请求体
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", _username),
                new KeyValuePair<string, string>("password", _password)
            });

            // 发送 POST 请求
            var response = await _httpClient.PostAsync("/api/v2/auth/login", content);

            // 登录成功则返回 true
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Torrent>> GetTorrents()
        {
            // 发送 GET 请求获取种子信息
            var response = await _httpClient.GetAsync("/api/v2/torrents/info");

            // 确保请求成功
            response.EnsureSuccessStatusCode();

            // 读取响应内容
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Torrent>>(jsonResponse);
        }
    }

    public class Torrent
    {
        public string hash { get; set; }
        public string name { get; set; }
        public int state { get; set; }
        // 添加其他需要的属性
    }
}
