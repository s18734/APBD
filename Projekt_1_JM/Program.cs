using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Projekt_1_JM
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //int? tmp1 = null;
            //double tmp2 = 1.0;

            //string tmp3 = "aaa";
            //Boolean tmp4 = true;

            //var tmp5 = "cos";
            //var newPerson = new Person { FirstName = "julian" };
            var url = args.Length > 0 ? args[0] : "https://www.pja.edu.pl";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            // jak chcesz stworzyc sciezke mozna skopiowac i postawic malpke
            // var path = @"P:\FTP(Public)\pgago\APBD" i git jest

            if (response.IsSuccessStatusCode)
            {
                var htmlContent = await response.Content.ReadAsStringAsync();


                var regex = new Regex("[a-z]+[a-z0-9]*@[a-z0-9]+\\.[a-z]+", RegexOptions.IgnoreCase);
                var matches = regex.Matches(htmlContent);

                foreach(var match in matches)
                {
                    Console.WriteLine(match.ToString());
                    
                }

                
            }
        }
    }
}
