using System.IO;
using Microsoft.Extensions.Logging;

namespace ReaperKing.Core
{
    public struct IntermediateGenerationResult
    {
        public PageGenerationResult Meta { get; set; }
        public string Path { get; set; }
        public string Content { get; set; }
    }
    
    public abstract partial class Site
    {
        public async void SavePage(PageGenerationResult result, string uri)
        {
            Log.LogInformation($"Saving page {result.Name} at {uri}/{result.Uri}");

            // Construct an intermediate object to hold generated data.
            var intermediate = new IntermediateGenerationResult
            {
                Meta = result,
                Path = Path.Join(DeploymentPath, uri),
                Content = await GetRazor().CompileRenderAsync(result.Template, result.Model),
            };

            if (result.Uri != null)
            {
                intermediate.Path = Path.Join(intermediate.Path, result.Uri);
            }
            
            // Execute the post-processors
            foreach (var module in GetModuleInstances<RkDocumentProcessorModule>())
            {
                module.PostProcessDocument(uri, ref intermediate);
            }
            
            // Write the document to disk.
            Directory.CreateDirectory(intermediate.Path);
            intermediate.Path = Path.Join(intermediate.Path, result.Name + ".html");
            File.WriteAllText(intermediate.Path, intermediate.Content);
        }
    }
}