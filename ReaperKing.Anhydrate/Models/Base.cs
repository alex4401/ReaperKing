using ReaperKing.Core;

namespace ReaperKing.Anhydrate.Models
{
    public class AnhydrateModel : BaseModel
    {
        public AnhydrateModel(SiteContext ctx) : base(ctx)
        { }

        /**
         * Name of the site section.
         */
        public string SectionName { get; set; }
        
        /**
         * Name of the document.
         */
        public string DocumentTitle { get; set; }
        
        /**
         * Whether the default document name format should be
         * ignored, causing DocumentTitle to be used instead.
         */
        public bool OverrideDocumentTitle { get; set; }

        /**
         * Site navigation items.
         */
        public NavigationItem[] Navigation;
    }
}