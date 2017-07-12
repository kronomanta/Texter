using System.Threading.Tasks;

namespace Texter
{
    class FileManager
    {
        private const string _configPath = "data.data";

        public static async Task SaveAsync<T>(string path, T data)
        {
            using(var file = System.IO.File.CreateText(path))
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                await file.WriteAsync(json);
            }
        }

        public static async Task<T> LoadAsync<T>(string path)
        {
            if (!System.IO.File.Exists(path)) return default(T);

            using (var file = System.IO.File.OpenRead(path))
            {
                byte[] jsonByte = new byte[file.Length]; 
                await file.ReadAsync(jsonByte, 0, (int)file.Length);

                string json = System.Text.Encoding.UTF8.GetString(jsonByte);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
        }

        public static async Task SaveConfigAsync<T>(T data)
        {
            await SaveAsync(_configPath, data);
        }

        public static async Task<T> LoadConfigAsync<T>()
        {
            return await LoadAsync<T>(_configPath);
        }
    }
}
