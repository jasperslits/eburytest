using System;
using System.IO;

namespace Ebury_mass_payments
{
    public class TokenCache
    {
        private const string filename = "TokenCache.txt";

        private List<String> messages { get; set; } = new();

        public TokenCache()
        {

        }

        public async Task SetCache(string token)
        {
            messages.Add($"Writing {token} to cache {filename}");
            await File.WriteAllTextAsync(filename, token);

        }

        public List<String> GetMessages()
        {
            return messages;
        }

        public string? GetCache()
        {
            messages.Add($"Checking cached token {filename}");

            if (! File.Exists(filename))
            {
                messages.Add($"Cache file does not exist {filename}");
                return null;
            }

            DateTime dt = File.GetLastWriteTime(filename);
            DateTime secondDate = DateTime.Now;
            System.TimeSpan diff = secondDate.Subtract(dt);
            if ((int)diff.TotalMinutes > EburyConfig.Timeout)
            {
                messages.Add($"Expired as age is {diff.TotalMinutes} and max age is {EburyConfig.Timeout}");
                File.Delete(filename);
                return null;
            }

            string token = System.IO.File.ReadAllText(filename).Trim();
            messages.Add($"Found cached token {token} of age {diff.TotalMinutes}");

            return token;
        }
    }
}

