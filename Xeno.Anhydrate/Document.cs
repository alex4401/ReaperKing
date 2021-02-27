/*!
 * This file is a part of Xeno, and the project's repository may be found at https://github.com/alex4401/rk.
 *
 * The project is free software: you can redistribute it and/or modify it under the terms of the GNU General Public
 * License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
 * version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with this program. If not, see
 * http://www.gnu.org/licenses/.
 */

using System;

using Xeno.Anhydrate.Models;
using Xeno.Core;

namespace Xeno.Anhydrate
{
    public abstract class AnhydrateDocument<T> : IDocumentGenerator
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

        public virtual FooterInfo GetFooter()
            => new()
            {
                CopyrightMessage = "Fill your copyright message in GetFooter()",
            };

        public virtual T GenerateModel()
        {
            return GetModel() with {
                Navigation = GetNavigation(),
                Footer = GetFooter(),
                OpenGraphsType = GetOpenGraphsType(),
                OpenGraphsImage = GetOpenGraphsImage(),
            };
        }

        public virtual DocumentGenerationResult Generate(SiteContext ctx)
        {
            Context = ctx;
            
            return new()
            {
                Uri = GetUri(),
                Name = GetName(),
                Template = GetTemplateName(),
                
                Model = GenerateModel(),
            };
        }
    }
}