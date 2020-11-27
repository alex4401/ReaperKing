using System;

using ReaperKing.Anhydrate.Models;
using ReaperKing.Core;

namespace ReaperKing.Anhydrate
{
    public abstract class AnhydrateDocument<T> : IPageGenerator
        where T : AnhydrateModel
    {
        protected SiteContext Context { get; private set; }
        
        public abstract string GetName();
        public abstract string GetTemplateName();
        public abstract T GetModel();

        public virtual string GetUri() => "";
        public virtual NavigationItem[] GetNavigation() => Array.Empty<NavigationItem>();
        
        public virtual string GetOpenGraphsType() => "";
        public virtual string GetOpenGraphsImage() => "";

        public virtual PageGenerationResult Generate(SiteContext ctx)
        {
            Context = ctx;
            
            return new()
            {
                Uri = GetUri(),
                Name = GetName(),
                Template = GetTemplateName(),
                Model = GetModel() with {
                    Navigation = GetNavigation(),
                    OpenGraphsType = GetOpenGraphsType(),
                    OpenGraphsImage = GetOpenGraphsImage(),
                },
            };
        }
    }
}