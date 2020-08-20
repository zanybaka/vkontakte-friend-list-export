using System;
using System.IO;
using Shared.Utils.Lib.Entities.IO;

namespace vkontakte.lib
{
    public class VkAuthToken
    {
        public string ReadLastValue()
        {
            return new FileContent(
                    new FilePath(_settingsPath.Value))
                        .Read();
        }

        public void StoreNewToken(string newToken)
        {
            new FileContent(
                new FilePath(_settingsPath.Value))
                    .Write(newToken);
        }

        private readonly Lazy<string> _settingsPath = new Lazy<string>(() => Path.Combine(Path.GetTempPath(), "vkontakte.lib"));
    }
}