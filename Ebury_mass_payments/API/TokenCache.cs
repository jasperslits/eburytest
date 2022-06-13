using System;
using System.IO;

namespace Ebury_mass_payments
{
    public class TokenCache
    {
        private const string filename = "TokenCache.txt";

        public TokenCache()
        {
        }

        public async Task SetCache(string token)
        {
            Console.WriteLine("Writing " + token + " to cache " + filename);
            await File.WriteAllTextAsync(filename, token);

        }



        public string? GetCache()
        {
            Console.WriteLine("Checking cached token " + filename);

            if (! File.Exists(filename))
            {
                Console.WriteLine("Cache file does not exist " + filename);
                return null;
            }

            DateTime dt = File.GetLastWriteTime(filename);
            DateTime secondDate = DateTime.Now;
            System.TimeSpan diff = secondDate.Subtract(dt);
            if ((int)diff.TotalMinutes > EburyConfig.timeout)
            {
                Console.WriteLine("Expired as age is " + diff.TotalMinutes + " and max age is " + EburyConfig.timeout);
                File.Delete(filename);
                return null;
            }

            string token = System.IO.File.ReadAllText(filename).Trim();
            Console.WriteLine("Found cached token " + token + " of age " + diff.TotalMinutes);

            return token;
        }
    }
}

