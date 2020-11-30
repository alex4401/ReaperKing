using System;
using System.Collections.Generic;

namespace ReaperKing.Core
{
    [RkProjectProperty("webPaths")]
    public record WebPathConfiguration
    {
        public string Root { get; init; } = "/";
        public string Resources { get; init; } = "/assets";
    }
    
    [RkProjectProperty("build")]
    public record BuildConfiguration
    {
        public string[] AssetDependencies { get; init; } = Array.Empty<string>();
        public string[] BeforeBuild { get; init; } = Array.Empty<string>();
    }
    
}