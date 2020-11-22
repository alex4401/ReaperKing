using System;

using ReaperKing.Core;

namespace ReaperKing.Plugins
{
    public struct SitemapLocalExclusion : IDisposable
    {
        private RkSitemapExclusionModule Module { get; }
        public bool PreviousState { get; set; }

        public SitemapLocalExclusion(SiteContext ctx, bool newState)
            : this(ctx.Site, newState)
        { }

        public SitemapLocalExclusion(Site site, bool newState)
        {
            Module = site.GetModuleInstance<RkSitemapExclusionModule>();
            PreviousState = Module.ShouldExclude;
            Module.ShouldExclude = newState;
        }

        public void Dispose()
        {
            Module.ShouldExclude = PreviousState;
        }
    }
    
    public class RkSitemapExclusionModule : RkDocumentProcessorModule
    {
        public bool ShouldExclude { get; set; } = false;
        
        public RkSitemapExclusionModule(Site site) : base(site)
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
            return new SitemapLocalExclusion(site, !enable);
        }
        
        public static SitemapLocalExclusion OverrideSitemaps(this SiteContext ctx, bool enable)
        {
            return new SitemapLocalExclusion(ctx, !enable);
        }
    }
}