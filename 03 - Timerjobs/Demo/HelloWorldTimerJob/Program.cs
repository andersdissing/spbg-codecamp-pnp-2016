using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorldTimerJob
{
    class Program
    {
        static void Main(string[] args)
        {
            var job = new HelloWorldJob();
            job.UseOffice365Authentication("bernd@rickenberg.net", GetPassword());
            job.AddSite("https://rickenberg.sharepoint.com/codecamp2016");
            job.Run();
        }

        private static string GetPassword()
        {
            Console.WriteLine("Input your password: ");
            var result = string.Empty;
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
                result += keyInfo.KeyChar;
                Console.Write("*");
            } while (keyInfo.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return result;
        }
    }
}
