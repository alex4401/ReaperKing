using Microsoft.Extensions.Logging;

namespace ReaperKing.Core
{
    public abstract class RkModule
    {
        protected Site Site { get; }
        protected ILogger Log { get; }

        public RkModule(Site site)
            => (Site, Log) = (site, site.Log);
    }
}