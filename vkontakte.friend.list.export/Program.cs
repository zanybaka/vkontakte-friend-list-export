using Shared.Utils.Lib.Entities.String;
using VkNet.Enums.Filters;
using vkontakte.lib;
using vkontakte.lib.Json;

namespace vkontakte.friend.list.export
{
    static class Program
    {
        static void Main()
        {
            new UserInput()
                .ShowMessage(
                    new FormatString(
                        "Exported to {0}.",
                        new Vk()
                            .ExportFriendsToDisk(
                                new Vk()
                                    .GetFriends(
                                        new Vk()
                                            .Auth(new AppSettings(), new VkAuthToken(), new UserInput()),
                                        ProfileFields.All),
                                new NewtonsoftJson()
                                )
                        ).GetValue()
                    );

            new UserInput()
                .ShowMessage(
                    new FormatString(
                        "Exported to {0}\r\nPress Enter to Exit.",
                        new Vk()
                            .ExportFriendContactsToDisk(
                                new Vk()
                                    .GetFriends(
                                        new Vk()
                                            .Auth(new AppSettings(), new VkAuthToken(), new UserInput()),
                                        ProfileFields.Contacts),
                                new NewtonsoftJson()
                                )
                        ).GetValue()
                    )
                .ReadUserInput();
        }
    }
}
