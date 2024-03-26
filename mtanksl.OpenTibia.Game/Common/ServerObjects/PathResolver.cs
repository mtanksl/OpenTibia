using System.IO;

namespace OpenTibia.Game
{
    public class PathResolver
    {
        public bool Exists(string relativePath)
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory() );

            var appended = false;

            while (directory != null)
            {
                var fullpath = Path.Combine(directory.FullName, relativePath);

                if (Path.Exists(fullpath) )
                {
                    return true;
                }

                if (!appended)
                {
                    relativePath = Path.Combine("mtanksl.OpenTibia.GameData", relativePath);

                    appended = true;
                }

                directory = directory.Parent;
            }

            return false;
        }

        /// <exception cref="FileNotFoundException"></exception>
        
        public string GetFullPath(string relativePath)
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory() );

            var appended = false;

            while (directory != null)
            {
                var fullpath = Path.Combine(directory.FullName, relativePath);

                if (Path.Exists(fullpath) )
                {
                    return fullpath;
                }

                if (!appended)
                {
                    relativePath = Path.Combine("mtanksl.OpenTibia.GameData", relativePath);

                    appended = true;
                }

                directory = directory.Parent;
            }

            throw new FileNotFoundException();
        }
    }
}