using System.Collections.Generic;

using ReaperKing.Core;

namespace ReaperKing.Anhydrate
{
    [RkProjectProperty("rk", "anhydrate")]
    public record AnhydrateConfiguration
    {
        public string IncludePath { get; init; } = "";
    }
}