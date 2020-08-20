using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Shared.Utils.Lib.Entities.IO;
using Shared.Utils.Lib.Entities.Json;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Utils;
using vkontakte.lib.Json;
using FileContent = Shared.Utils.Lib.Entities.IO.FileContent;

namespace vkontakte.lib
{
    public class Vk
    {
        public VkApi Auth(AppSettings appSettings, VkAuthToken vkAuthToken, UserInput userInput)
        {
            VkApi api = new VkApi();
            try
            {
                string token = vkAuthToken.ReadLastValue();
                if (string.IsNullOrEmpty(token))
                {
                    throw new VkApiException("Token is null");
                }
                api.Authorize(new ApiAuthParams()
                {
                    AccessToken = token
                });
                // warm-up
                api.Friends.Get(new FriendsGetParams());
            }
            catch (VkApiException)
            {
                userInput.ShowMessage("Authorization:\r\nEnter password and press Enter: ");
                string password = userInput.ReadUserInput();
                api.Authorize(new ApiAuthParams
                {
                    ApplicationId = appSettings.VkApplicationId, // https://vk.com/editapp?id=&section=options
                    Login = appSettings.VkLogin,
                    Password = password,
                    Settings = Settings.All,
                    TwoFactorAuthorization = () =>
                    {
                        userInput.ShowMessage("Two Factor Authorization:\r\nEnter code from vk.com/messages and press Enter: ");
                        return userInput.ReadUserInput();
                    }
                });
                vkAuthToken.StoreNewToken(api.Token);
            }
            return api;
        }

        public VkCollection<User> GetFriends(VkApi vkApi, ProfileFields fields)
        {
            VkCollection<User> friends = vkApi.Friends.Get(new FriendsGetParams()
            {
                Fields = fields,
            });
            return friends;
        }

        public string ExportFriendsToDisk(VkCollection<User> friends, NewtonsoftJson newtonsoftJson)
        {
            string json = JsonConvert.SerializeObject(
                new OutputJson
                {
                    JsonVersion = newtonsoftJson.Version.Value,
                    Friends = friends.ToArray()
                },
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });
            string outputFile = Path.Combine(Directory.GetCurrentDirectory(), $"friends_{DateTime.Now:yyyy-MM-dd}.json");
            new FileContent(
                new FilePath(outputFile))
                    .Write(json);
            return outputFile;
        }

        public string ExportFriendContactsToDisk(VkCollection<User> friendContacts, NewtonsoftJson newtonsoftJson)
        {
            string json = JsonConvert.SerializeObject(
                new OutputJson()
                {
                    JsonVersion = newtonsoftJson.Version.Value,
                    Friends = friendContacts
                        .Where(x => !x.IsDeactivated && (!string.IsNullOrEmpty(x.HomePhone) || !string.IsNullOrEmpty(x.MobilePhone)))
                        .ToArray()
                },
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new JsonFieldSerializationPredicate(
                        property => !new[] { typeof(string), typeof(User[]) }.Contains(property.PropertyType),
                        obj => false)
                });
            string outputFile = Path.Combine(Directory.GetCurrentDirectory(), $"friend_contacts_{DateTime.Now:yyyy-MM-dd}.json");
            new FileContent(
                    new FilePath(outputFile))
                .Write(json);
            return outputFile;
        }
    }
}
