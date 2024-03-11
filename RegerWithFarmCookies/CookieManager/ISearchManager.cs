namespace RegerWithCookies.CookieManager
{
    public interface ISearchManager
    {
        /// <summary>
        ///  Метод для фарма куков по случайным сайтам
        /// </summary>
        void WebRandomCookieCrawler();

        /// <summary>
        ///  script для поиска в гугле нашей цели и вход в нее
        /// </summary>
        void WebScriptTargetSearch();
    }
}
