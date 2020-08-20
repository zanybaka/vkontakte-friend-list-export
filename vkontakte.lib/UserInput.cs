using System;

namespace vkontakte.lib
{
    public class UserInput
    {
        public UserInput ShowMessage(string message)
        {
            Console.WriteLine(message);
            return this;
        }

        public string ReadUserInput()
        {
            return Console.ReadLine();
        }
    }
}