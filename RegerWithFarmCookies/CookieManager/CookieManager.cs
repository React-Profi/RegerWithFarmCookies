using System;
using RegerWithCookies.SearchManagers;
using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.ProjectModel;

namespace RegerWithCookies.CookieManager
{

    /// <summary>
    /// Класс нагуливания cookies
    /// </summary>
    public class CookieManager
    {
        private readonly Instance _instance;
        private readonly IZennoPosterProjectModel _project;
        private readonly ISearchManager _searchManager;
        private readonly Random _random;

        /// <summary>
        /// Указывает количество рандомных сайтов которые мы должны посетить
        /// </summary>
        public int NumberOfVisitedRandomSites { get; set; }

        /// <summary>
        /// Указывает случайное время нахождения на сайте(каждый раз выдает новое значение)
        /// </summary>
        public int TimeOnRandomSite => _random.Next(23, 67) * 1000;

        public CookieManager(Instance instance, IZennoPosterProjectModel project)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
            _project = project ?? throw new ArgumentNullException(nameof(project));
            _random = new Random();

            NumberOfVisitedRandomSites = _random.Next(3, 5);

            _searchManager = new GoogleSearchManager(instance, project);
        }
        public void ProcessCookieData()
        {

            for (var i = 0; i < NumberOfVisitedRandomSites; i++)
            {
                _searchManager.WebRandomCookieCrawler();
                System.Threading.Thread.Sleep(TimeOnRandomSite);
            }

            _searchManager.WebScriptTargetSearch();
        }
    }



}


