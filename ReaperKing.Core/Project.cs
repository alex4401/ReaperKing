using System;
using System.Collections.Generic;

namespace ReaperKing.Core
{
    public struct Project
    {
        public string Inherits;
        public PathConfig Paths;
        public ResourceConfig Resources;
        public BuildConfig Build;

        public struct PathConfig
        {
            public string Root;
            public string Deployment;
            public string Resources;
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