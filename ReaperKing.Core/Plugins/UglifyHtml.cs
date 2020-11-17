using NUglify;

namespace ReaperKing.Core.Plugins
{
    public class RkUglifyModule : RkDocumentProcessorModule
    {
        public bool IsEnabled { get; set; }

        public RkUglifyModule(Site site) : base(site)
        {
            IsEnabled = Site.ProjectConfig.Build.MinifyHtml;
        }

        public override void PostProcessDocument(string uri, ref IntermediateGenerationResult result)
        {
            if (!IsEnabled)
            {
                return;
            }

            UglifyResult uglifyResult = Uglify.Html(result.Content);
            if (uglifyResult.HasErrors)
            {
                // TODO: print the errors
            }
            else
            {
                result.Content = uglifyResult.Code;
            }
        }
    }
}