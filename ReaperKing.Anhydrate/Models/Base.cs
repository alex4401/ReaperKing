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