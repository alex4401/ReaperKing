using System;
using Microsoft.Extensions.Logging;

namespace ReaperKing.Core
{
    public abstract class RkModule
    {
        protected Site Site { get; }
        protected ILogger Log { get; }

        protected RkModule(Type selfType, Site site)
            => (Site, Log) = (site, site.LogFactory.CreateLogger(selfType.FullName));
    }
}