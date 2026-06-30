using System.Text.Json;

namespace Tamphan_BBP.Services
{
    public class LoadContentService
    {
        private readonly IWebHostEnvironment _environment;

        public LoadContentService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public Dictionary<string, string> Load(string fileName)
        {
            string path = Path.Combine(
               _environment.ContentRootPath,
               "Content",
               $"{fileName}.json");

            if (!File.Exists(path))
            {
                return new Dictionary<string, string>();
            }

            string json = File.ReadAllText(path);

            return JsonSerializer.Deserialize<Dictionary<string, string>>(json)
                   ?? new Dictionary<string, string>();
        }
    }
}
