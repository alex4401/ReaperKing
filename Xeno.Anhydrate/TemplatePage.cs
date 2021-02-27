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
using RazorLight;

using Xeno.Anhydrate.Models;

namespace Xeno.Anhydrate
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