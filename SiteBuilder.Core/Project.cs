namespace SiteBuilder.Core
{
    public struct Project
    {
        public SiteConfig Site;
        public BuildConfig Build;

        public struct SiteConfig
        {
            public string WebRoot;
            public string DeploymentDirectory;
            public string ResourceDirectory;
            public string[] NonVersionedResources;
        }

        public struct BuildConfig
        {
            public string[] RunBefore;
        }
    }
}