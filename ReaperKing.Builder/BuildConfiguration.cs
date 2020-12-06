using System.Collections.Generic;

using ReaperKing.Core;

namespace ReaperKing.Builder
{
    [RkProjectProperty("rk", "build")]
    internal record BuildConfiguration
    {
        public List<string> BeforeBuild { get; init; } = new();
        public List<string> ExtraBeforeBuild { get; init; } = new();
    }
}