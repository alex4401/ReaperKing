using System.Collections.Generic;

namespace SiteBuilder.Core
{
    public interface ISiteContentProvider
    {
        public void BuildContent(SiteContext ctx);
    }
}