using System;
using System.Collections.Generic;

namespace ReaperKing.Core
{
    public interface IPageGenerator
    {
        public PageGenerationResult Generate(SiteContext ctx);
    }
}