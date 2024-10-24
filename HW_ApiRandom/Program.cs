using Newtonsoft.Json;
using System.ComponentModel;

namespace HW_ApiRandom
{
     class Program
    {
        private const string BaseUrl = "https://api.random.org/";
        static async Task Main(string[] args)
        {
            var result = await RollDiceAsync(6);
        }
        static async Task PlayerOrComputer()
        {
            Console.WriteLine("player press enter for roll");
            Console.ReadLine();
            int playerRoll = await RollDiceAsync(6);
            Console.WriteLine($"player roll total {playerRoll}");

            Console.WriteLine("computer is rolling dice");
            int computerTotal = await RollDiceAsync(6);
            Console.WriteLine($"total roll computer {computerTotal}");

            if(computerTotal > playerRoll)
            {
                Console.WriteLine("this time win Computer");
            }
            else if(computerTotal < playerRoll) 
            {
                Console.WriteLine("this time win Player");
            }
            else
            {
                Console.WriteLine("it's tie");
            }
        }

        static async Task<int> RollDiceAsync(int sides)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                string ApiKey = "8198745c-e4ba-4b99-b0cf-63e61399a31e";

                var content = new StringContent(
                    $"{{\"jsonrpc\":\"2.0\",\"method\":\"generateIntegers\",\"params\":{{\"apikey\":\"{ApiKey}\",\"n\":1,\"min\":1,\"max\":{sides},\"replacement\":true}},\"id\":1}}",
                     System.Text.Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync("", content);

                if(response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(json);
                    return result.result.random.data[0];
                }
                else
                {
                    throw new Exception("failed to generate random nubmer");
                }
            }
        }
    }
}
