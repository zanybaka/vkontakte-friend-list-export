namespace Shared.Utils.Lib.Entities.IO
{
    public class FilePath
    {
        private readonly string _path;

        public FilePath(string path)
        {
            _path = path;
        }

        public override string ToString()
        {
            return _path;
        }
    }
}