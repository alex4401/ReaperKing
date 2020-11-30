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
         * Whether the default document name format should be
         * ignored, causing DocumentTitle to be used instead.
         */
        public bool OverrideDocumentTitle { get; init; }

        /**
         * 
         */
        public FooterInfo Footer { get; init; }
        
        /**
         * Configuration for the layout in whatever format one
         * takes.
         *
         * BUG: This property is always writable due to a Razor's
         *      compiler bug.
         */
        public dynamic LayoutConfig { get; set; }

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