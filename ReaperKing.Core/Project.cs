using System.Collections.Generic;

namespace ReaperKing.Core
{
    public struct Project
    {
        public string Inherits;
        public string ContentDirectory;
        public string AssemblyDirectory;
        
        public PathConfig Paths;
        public ResourceConfig Resources;
        public BuildConfig Build;

        public struct PathConfig
        {
            public string Root;
            public string Deployment;
            public string Resources;
            public string Sitemap;
        }

        public struct ResourceConfig
        {
            public string[] CopyNonVersioned;
        }

        public struct BuildConfig
        {
            public List<string> Define;
            public bool MinifyHtml;
            public List<string> RunBefore;
            
            public string[] AddRunBeforeCmds;
            public string[] AddDefines;
        }
    }
}