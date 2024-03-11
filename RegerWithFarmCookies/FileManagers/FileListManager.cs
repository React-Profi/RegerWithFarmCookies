using System;
using RegerWithCookies.SearchManagers;
using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.ProjectModel;

namespace RegerWithCookies.FileManagers
{
    public class FileListManager : IFileListManager
    {
        private readonly Instance _instance;
        private readonly IZennoPosterProjectModel _project;
        private readonly string _namesList;
        private readonly Random _random;

        public FileListManager(Instance instance, IZennoPosterProjectModel project, string namesList)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
            _project = project ?? throw new ArgumentNullException(nameof(project));
            _namesList = namesList;
            _random = new Random();
        }

        public string GetRandomUrlFromList()
        {
            var list = _project.Lists[_namesList];

            if (list == null)
            {
                _project.SendErrorToLog("list равен null " + " project.Lists[\"listUrlCookie\"] == null");
                throw new FileListManagerException.NullFileException("list равен null " + " project.Lists[\"listUrlCookie\"] == null");
            }

            if (list.Count == 0)
            {
                _project.SendErrorToLog("В list нет значений " + "project.Lists[\"listUrlCookie\"] == 0");
                throw new FileListManagerException.EmptyFileException("В list нет значений " + "project.Lists[\"listUrlCookie\"] == 0");
            }

            return list[_random.Next(list.Count)];
        }
    }
    namespace FileListManagerException
    {
        // Исключение, если список URL не существует
        public class NullFileException : Exception
        {
            public NullFileException(string message) : base(message) { }
        }

        // Исключение, если список URL пуст
        public class EmptyFileException : Exception
        {
            public EmptyFileException(string message) : base(message) { }
        }

    }
}
