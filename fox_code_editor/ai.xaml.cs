using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OpenAI;

namespace fox_code_editor
{
    /// <summary>
    /// ai.xaml の相互作用ロジック
    /// </summary>
    public partial class ai : Window
    {
        public ai()
        {
            InitializeComponent();
        }

        private async void input_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        // OpenAI APIのレスポンスを格納するクラス
        public class OpenAIResponse
        {
            public List<Choice> choices { get; set; }
        }

        public class Choice
        {
            public string text { get; set; }
        }

        private async void okuru_Click(object sender, RoutedEventArgs e)
        {
            var apiKey = "";
            var model = "text-davinci-003"; // 使用するモデル
            var prompt = output.Text;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.openai.com/v1/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var content = new StringContent(JsonSerializer.Serialize(new
                {
                    model = model,
                    prompt = prompt
                }), System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync("completions", content);
                var responseString = await response.Content.ReadAsStringAsync();

                var responseJson = JsonSerializer.Deserialize<OpenAIResponse>(responseString);
                if (responseJson?.choices != null && responseJson.choices.Count > 0)
                {
                    output.Text = responseJson.choices[0].text.Trim();
                }
            }
        }
    }
}
