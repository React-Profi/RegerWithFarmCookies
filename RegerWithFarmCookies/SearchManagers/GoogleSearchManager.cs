using System;
using RegerWithCookies.CookieManager;
using RegerWithCookies.FileManagers;
using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.ProjectModel;

namespace RegerWithCookies.SearchManagers
{
    public class GoogleSearchManager : ISearchManager
    {
        private readonly Instance _instance;
        private readonly IZennoPosterProjectModel _project;
        private readonly IFileListManager _listOfNewsSiteLinks;
        private readonly IFileListManager _listTargetSearchQueries;

        public GoogleSearchManager(Instance instance, IZennoPosterProjectModel project)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
            _project = project ?? throw new ArgumentNullException(nameof(project));
            _listOfNewsSiteLinks = new FileListManager(instance, project, "readOnlyListUrlCookie");
            _listTargetSearchQueries = new FileListManager(instance, project, "readOnlyListTargetedQueries");
        }

        public void WebScriptTargetSearch()
        {
            GoToLink("google.com");

            ClickGoogleSearchTextArea();

            SetFieldValueGoogleTextArea(_listTargetSearchQueries.GetRandomUrlFromList()); 

            PressEnterOnBoard();

            TargetElementClick();
        }

        public void WebRandomCookieCrawler()
        {
            GoToLink("google.com");

            GoToLink(_listOfNewsSiteLinks.GetRandomUrlFromList());
        } 
        
        private void PressEnterOnBoard()
        {
            GetActiveTab().KeyEvent("enter", "press", "");
        }

        private void TargetElementClick()
        {
            var he = GetActiveTab().FindElementByXPath("//span[text()='Proton']",0);
            if (he.IsVoid)
            {
                _project.SendErrorToLog(he.IsVoid+"Не нашли элемент:" + "\n" + "FindElementByAttribute(\"h3\", \"innertext\", \"Pro.*ton\", \"regexp\", 0);");
                throw new GoogleSearchManagerException.FindElementException("Не нашли элемент:" + "\n" + "FindElementByAttribute(\"h3\", \"innertext\", \"Pro.*ton\", \"regexp\", 0);");
            }
            
            _instance.WaitFieldEmulationDelay();
            
            he.RiseEvent("click", _instance.EmulationLevel);

        }
        private void SetFieldValueGoogleTextArea(string value)
        {
    
            var textareaElement = FindGoogleElementTextarea();
       
            _instance.WaitFieldEmulationDelay();
           
            textareaElement.SetValue(value, _instance.EmulationLevel, false);
        }

        private HtmlElement FindGoogleElementTextarea()
        {
            var textareaElement = GetActiveTab().GetDocumentByAddress("0").FindElementByTag("form", 0).FindChildById("APjFqb");
            
            if (textareaElement.IsVoid)
            {
                _project.SendErrorToLog("Не нашли элемент:" + "\n" + "FindChildById(\"APjFqb\");");
                throw new GoogleSearchManagerException.FindElementException("Не нашли элемент:" + "\n" + "FindChildById(\"APjFqb\");");
            }

            return textareaElement;
        }

        private void ClickGoogleSearchTextArea()
        {
            var textareaElement = FindGoogleElementTextarea();
            
            _instance.WaitFieldEmulationDelay();
         
            textareaElement.RiseEvent("click", _instance.EmulationLevel);
        }

        private void GoToLink(string linkName)
        {
            var activeTab = GetActiveTab();
            activeTab.Navigate(linkName, "");
            WaitForTabToFinishLoading(activeTab);
        }


        private void WaitForTabToFinishLoading(Tab activeTab)
        {
            if (activeTab.IsBusy)
            {
                activeTab.WaitDownloading();
            }
        }

        private Tab GetActiveTab()
        {
            var activeTab = _instance.ActiveTab;

            if (activeTab.IsVoid)
            {
                _project.SendErrorToLog("ActiveTab оказался void");
                throw new GoogleSearchManagerException.VoidActiveTab("ActiveTab оказался void");
            }

            if (activeTab.IsNull)
            {
                _project.SendErrorToLog("ActiveTab оказался null");
                throw new GoogleSearchManagerException.NullActiveTab(" ActiveTab оказался null");
            }

            return activeTab;
        }
    }
    namespace GoogleSearchManagerException
    {
        // Исключение, если не удалось найти элемент на странице
        public class FindElementException : Exception
        {
            public FindElementException(string message) : base(message) { }
        }
        // Исключение, если ActiveTab оказался null
        public class NullActiveTab : Exception
        {
            public NullActiveTab(string message) : base(message) { }
        }

        // Исключение, если  ActiveTab оказался void
        public class VoidActiveTab : Exception
        {
            public VoidActiveTab(string message) : base(message) { }
        }

    }
}
