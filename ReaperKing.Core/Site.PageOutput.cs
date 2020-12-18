/*!
 * This file is a part of Reaper King, and the project's repository may be
 * found at https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it
 * under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or (at
 * your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See
 * the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */

using System;
using System.IO;
using Microsoft.Extensions.Logging;
using RazorLight;

namespace ReaperKing.Core
{
    public struct IntermediateGenerationResult
    {
        public DocumentGenerationResult Meta { get; set; }
        public string FilePath { get; set; }
        public string Uri { get; set; }
        public string Content { get; set; }
    }
    
    public abstract partial class Site
    {
        public async void SavePage(DocumentGenerationResult result, string uri)
        {
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
            intermediate.Uri = Path.Combine(WebConfig.Root, intermediate.Uri);
            
            Log.LogInformation($"Saving document: \"{intermediate.Uri}\"");
            
            // Move the content to the intermediate object
            if (!String.IsNullOrWhiteSpace(result.Template))
            {
                // The document is templated; render with Razor.

                // Copy the model and set DocumentUri if the model descends
                // from our BaseModel.
                object model = result.Model;
                if (model is BaseModel recordModel)
                {
                    model = recordModel with { DocumentUri = intermediate.Uri };
                }
                
                // Compile the template and render the document.
                ITemplatePage template = await RazorEngine.CompileTemplateAsync(result.Template);
                intermediate.Content = await RazorEngine.RenderTemplateAsync(template, model);
            }
            else
            {
                // Only text is provided.
                intermediate.Content = result.Text;
            }

            // Execute the post-processors
            foreach (var module in GetModuleInstances<IRkDocumentProcessorModule>())
            {
                module.PostProcessDocument(uri, ref intermediate);
            }
            
            // Write the document to disk.
            Directory.CreateDirectory(Path.GetDirectoryName(intermediate.FilePath));
            await File.WriteAllTextAsync(intermediate.FilePath, intermediate.Content);
        }
    }
}