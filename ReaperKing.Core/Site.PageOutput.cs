using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace ReaperKing.Core
{
    public struct IntermediateGenerationResult
    {
        public PageGenerationResult Meta { get; set; }
        public string FilePath { get; set; }
        public string Uri { get; set; }
        public string Content { get; set; }
    }
    
    public abstract partial class Site
    {
        public async void SavePage(PageGenerationResult result, string uri)
        {
            Log.LogInformation($"Saving page {result.Name} at {uri}/{result.Uri}");
            
            // Ensure the file extension is set
            if (String.IsNullOrEmpty(result.Extension))
            {
                result.Extension = "html";
            }

            // Construct an intermediate object to hold generated data.
            var intermediate = new IntermediateGenerationResult
            {
                Meta = result,
                Uri = Path.Combine(uri ?? "", result.Uri ?? "", $"{result.Name}.{result.Extension}"),
                FilePath = Path.Join(DeploymentPath, uri),
            };
            intermediate.FilePath = Path.Join(DeploymentPath, intermediate.Uri);
            intermediate.Uri = Path.Combine(ProjectConfig.Paths.Root, intermediate.Uri);
            
            // Move the content to the intermediate object
            if (!String.IsNullOrWhiteSpace(result.Template))
            {
                intermediate.Content = await GetRazor().CompileRenderAsync(result.Template, result.Model);
            }
            else
            {
                intermediate.Content = result.Text;
            }

            // Execute the post-processors
            foreach (var module in GetModuleInstances<RkDocumentProcessorModule>())
            {
                module.PostProcessDocument(uri, ref intermediate);
            }
            
            // Write the document to disk.
            Directory.CreateDirectory(Path.GetDirectoryName(intermediate.FilePath));
            File.WriteAllText(intermediate.FilePath, intermediate.Content);
        }
    }
}