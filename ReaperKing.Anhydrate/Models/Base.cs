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

using ReaperKing.Core;

namespace ReaperKing.Anhydrate.Models
{
    public record AnhydrateModel : BaseModel
    {
        /**
         * Name of the site section.
         */
        public string SectionName { get; init; }
        
        /**
         * Name of the document.
         */
        public string DocumentTitle { get; init; }
        
        /**
         * Whether the default document name format should be ignored, causing DocumentTitle to be used instead.
         */
        public bool OverrideDocumentTitle { get; init; }

        /**
         * 
         */
        public FooterInfo Footer { get; init; }
        
        /**
         * Configuration for the layout in whatever format one takes.
         *
         * BUG: This property is always writable due to a Razor's
         *      compiler bug.
         */
        public dynamic LayoutConfig { get; set; }
        
        public string LayoutCssClasses { get; set; }

        public string OpenGraphsType { get; init; }
        public string OpenGraphsImage { get; init; }
        
        /**
         * CSS class of the icon element in site header widget.
         */
        public string HeaderIconClass { get; init; }

        /**
         * Site navigation items.
         */
        public NavigationItem[] Navigation;
        
        public AnhydrateModel(SiteContext ctx)
            : base(ctx)
        { }
    }
}