using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TestAPI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();

            var credentials = Encoding.ASCII.GetBytes("user:password");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));

            var url = "http://www.api.mediatekdocument.grdy2507.odns.fr/commande";

            var response = await client.GetAsync(url);
            var contentType = response.Content.Headers.ContentType;

            Console.WriteLine($"Status: {response.StatusCode}");
            Console.WriteLine($"Content-Type: {contentType}");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
        }
    }
}
