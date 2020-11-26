namespace ReaperKing.Core
{
    public abstract partial class Site
    {
        public void BuildPage(IPageGenerator generator, string uri = null)
        {
            SiteContext context = new()
            {
                Site = this,
                PathPrefix = uri,
            };
            var result = generator.Generate(context);
            SavePage(result, uri);
        }

        public void BuildWithProvider(ISiteContentProvider provider, string uri = null)
        {
            SiteContext context = new()
            {
                Site = this,
                PathPrefix = uri,
            };
            provider.BuildContent(context);
        }
    }
}