/*!
 * This file is a part of Reaper King, and the project's repository may be found at
 * https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it under the terms of the GNU General Public
 * License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
 * version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with this program. If not, see
 * https://www.gnu.org/licenses/.
 */

using NUglify;

using ReaperKing.Core;
using ReaperKing.Core.Configuration;

namespace ReaperKing.Plugins
{
    public record RkUglifyConfiguration
    {
        public bool Enable { get; init; } = false;
    }
    
    public class RkUglifyModule
        : RkModule, IRkDocumentProcessorModule
    {
        public bool IsEnabled { get; private set; }

        public RkUglifyModule(Site site)
            : base(typeof(RkUglifyModule), site)
        {
            Site.ProjectConfig.InjectProperty<RkUglifyConfiguration>("modules.uglify");
        }

        public override void AcceptConfiguration(ProjectConfigurationManager config)
        {
            IsEnabled = config.Get<RkUglifyConfiguration>().Enable;
        }

        public void PostProcessDocument(string uri, ref IntermediateGenerationResult result)
        {
            if (!IsEnabled || result.Meta.Extension != "html")
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