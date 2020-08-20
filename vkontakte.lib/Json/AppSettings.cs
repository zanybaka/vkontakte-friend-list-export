using System;
using System.IO;
using Newtonsoft.Json;

namespace vkontakte.lib.Json
{
    public class AppSettings
    {
        private string _login;
        private ulong _appId;

        public string VkLogin
        {
            get { return _login != null ? _login : _settings.Value.VkLogin; }
            set => _login = value;
        }

        public ulong VkApplicationId
        {
            get { return _appId != 0 ? _appId : _settings.Value.VkApplicationId; }
            set => _appId = value;
        }

        private readonly Lazy<AppSettings> _settings = new Lazy<AppSettings>(() =>
        {
            return JsonConvert.DeserializeObject<AppSettings>(
                File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(),
                    "AppSettings.json")));
        });
    }
}