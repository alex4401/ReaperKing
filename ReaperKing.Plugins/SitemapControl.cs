/*!
 * This file is a part of Reaper King, and the project's repository may be found at
 * https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it under the terms of the GNU General Public
 * License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
 * version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with this program. If not, see
 * https://www.gnu.org/licenses/.
 */

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
    
    public class RkSitemapExclusionModule
        : RkModule, IRkDocumentProcessorModule
    {
        public bool ShouldExclude { get; set; } = false;
        
        public RkSitemapExclusionModule(Site site)
            : base(typeof(RkSitemapExclusionModule), site)
        { }

        public void PostProcessDocument(string uri, ref IntermediateGenerationResult result)
        {
            if (ShouldExclude)
            {
                DocumentGenerationResult moved = result.Meta;
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