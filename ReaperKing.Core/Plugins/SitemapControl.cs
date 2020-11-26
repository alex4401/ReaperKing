using System;

using ReaperKing.Core;

namespace ReaperKing.Plugins
{
    public readonly struct SitemapLocalExclusion : IDisposable
    {
        private readonly RkSitemapExclusionModule _module;
        private readonly bool _previous;

        public SitemapLocalExclusion(SiteContext ctx, bool newState)
            : this(ctx.Site, newState)
        { }

        public SitemapLocalExclusion(Site site, bool newState)
        {
            _module = site.GetModuleInstance<RkSitemapExclusionModule>();
            _previous = _module.ShouldExclude;
            _module.ShouldExclude = newState;
        }

        public void Dispose()
        {
            _module.ShouldExclude = _previous;
        }
    }
    
    public class RkSitemapExclusionModule : RkDocumentProcessorModule
    {
        public bool ShouldExclude { get; set; } = false;
        
        public RkSitemapExclusionModule(Site site)
            : base(site)
        { }

        public override void PostProcessDocument(string uri, ref IntermediateGenerationResult result)
        {
            if (ShouldExclude)
            {
                PageGenerationResult moved = result.Meta;
                moved.SkipInSitemap = true;
                result.Meta = moved;
            }
        }
    }

    public static class SiteSitemapExclusionExtension
    {
        public static SitemapLocalExclusion OverrideSitemaps(this Site site, bool enable)
        {
            return new(site, !enable);
        }
        
        public static SitemapLocalExclusion OverrideSitemaps(this SiteContext ctx, bool enable)
        {
            return new(ctx, !enable);
        }
    }
}