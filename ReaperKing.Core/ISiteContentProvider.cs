using System.Collections.Generic;

namespace ReaperKing.Core
{
    public interface ISiteContentProvider
    {
        public void BuildContent(SiteContext ctx);
    }
}