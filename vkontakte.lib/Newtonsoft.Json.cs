using System;
using System.Reflection;
using Newtonsoft.Json;

namespace vkontakte.lib
{
    public class NewtonsoftJson
    {
        public Lazy<string> Version = new Lazy<string>(() => Assembly.GetAssembly(typeof(JsonConvert)).ToString());
    }
}