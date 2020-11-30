using System;
using RazorLight;
using ReaperKing.Anhydrate.Models;

namespace ReaperKing.Anhydrate
{
    public abstract class AnhydratePage<T> : TemplatePage<T>
        where T : AnhydrateModel
    {
        public async void RenderPartial(string sectionName, string templateName)
        {
            if (string.IsNullOrEmpty(sectionName))
            {
                throw new ArgumentNullException(nameof(sectionName));
            }
            
            if (string.IsNullOrEmpty(templateName))
            {
                throw new ArgumentNullException(nameof(templateName));
            }

            if (IsSectionDefined(sectionName))
            {
                Write(await RenderSectionAsync(sectionName));
                return;
            }
            
            await IncludeAsync(templateName, Model);
        }
        
        public void RenderWidget(string ns, string widgetName)
        {
            if (string.IsNullOrEmpty(ns))
            {
                throw new ArgumentNullException(nameof(ns));
            }
            
            if (string.IsNullOrEmpty(widgetName))
            {
                throw new ArgumentNullException(nameof(widgetName));
            }

            RenderPartial($"WidgetOverride.{widgetName}", $"{ns}/{widgetName}");
        }

        public string VersionedResource(string path, string uriTemplate)
        {
            return Model.Ctx.CopyVersionedResource(path, uriTemplate);
        }
    }
}