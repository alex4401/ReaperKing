using System;
using System.Collections.Generic;

namespace SiteBuilder.Core
{
    public interface IPageGenerator
    {
        public PageGenerationResult Generate(SiteContext ctx);
    }
}