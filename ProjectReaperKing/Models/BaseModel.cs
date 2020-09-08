namespace SiteBuilder.Core
{
    public struct BaseModel
    {
        public string SiteName;
        public string DisplayTitle;
        public string Root;
        public string RootUri;
        public string ResourcesDirectory;

        public BaseModel(Site site)
        {
            SiteName = "Untitled";
            DisplayTitle = "Untitled";
            Root = site.ProjectConfig.Site.WebRoot;
            RootUri = "/";
            ResourcesDirectory = site.ProjectConfig.Site.ResourceDirectory;
        }
    }
}