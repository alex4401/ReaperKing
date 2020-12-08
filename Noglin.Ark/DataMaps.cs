using System;
using System.Collections.Generic;

namespace Noglin.Ark
{
    public record DataMap
    {
        public string Name { get; init; }
        public string Level { get; init; }
        public CoordSetup Setup { get; init; }
        public TopomapSetup Topomap { get; init; }
    }
}