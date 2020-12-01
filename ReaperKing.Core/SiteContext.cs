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

using System.IO;

namespace ReaperKing.Core
{
    public struct SiteContext
    {
        public Site Site;
        public string PathPrefix;

        #region Resource Management
        public string CopyFileToLocation(string inputFile, string uri)
            => Site.CopyFileToLocation(inputFile, uri);

        public string CopyResource(string inputFile, string uri)
            => Site.CopyResource(inputFile, uri);

        public string CopyVersionedResource(string inputFile, string uri)
            => Site.CopyVersionedResource(inputFile, uri);
        #endregion
        
        #region Template Default Include Path Management
        public void AddTemplateDefaultIncludePath(string root)
            => Site.AddTemplateDefaultIncludePath(root);
        public TemplateDefaultMount TryAddTemplateDefaultIncludePath(string root)
            => new TemplateDefaultMount(Site, root);
        public TemplateDefaultMount TryAddTemplateDefaultIncludePaths(string[] roots)
            => new TemplateDefaultMount(Site, roots);
        public void RemoveTemplateDefaultIncludePath(string root)
            => Site.RemoveTemplateDefaultIncludePath(root);
        #endregion
        #region Template Scoped Include Path Management
        public void AddTemplateIncludeNamespace(string ns, string root)
            => Site.AddTemplateIncludeNamespace(ns, root);
        public TemplateNamespaceMount TryAddTemplateIncludeNamespace(string ns, string root)
            => new TemplateNamespaceMount(Site, ns, root);
        public void RemoveTemplateNamespace(string ns)
            => Site.RemoveTemplateNamespace(ns);
        public void RemoveTemplateNamespace(string ns, string root)
            => Site.RemoveTemplateNamespace(ns, root);
        #endregion

        #region Building Methods
        public void BuildPage(IDocumentGenerator generator, string uri = null)
        {
            if (PathPrefix != null)
            {
                uri = Path.Join(PathPrefix, uri);
            }
            
            Site.BuildPage(generator, uri);
        }
        public void BuildWithProvider(ISiteContentProvider provider, string uri = null)
        {
            if (PathPrefix != null)
            {
                uri = uri == null ? PathPrefix : Path.Join(PathPrefix);
            }
            
            Site.BuildWithProvider(provider, uri);
        }
        #endregion

        public bool IsConstantDefined(string id)
        {
            return Site.IsProjectConstantDefined(id);
        }

        public string GetRootUri()
        {
            return Site.ProjectConfig.Paths.Root != "/"
                    ? Path.Join(Site.ProjectConfig.Paths.Root, PathPrefix)
                    : PathPrefix;
        }
    }
}