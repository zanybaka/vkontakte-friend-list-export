using System.Text;

namespace Shared.Utils.Lib.Entities.IO
{
    public class FileContent
    {
        private readonly FilePath _zFile;

        public FileContent(FilePath zFile)
        {
            _zFile = zFile;
        }

        public string Read()
        {
            string path = _zFile.ToString();
            if (System.IO.File.Exists(path))
            {
                return System.IO.File.ReadAllText(path);
            }
            return "";
        }

        public void Write(string content)
        {
            System.IO.File.WriteAllText(_zFile.ToString(), content, Encoding.UTF8);
        }
    }
}