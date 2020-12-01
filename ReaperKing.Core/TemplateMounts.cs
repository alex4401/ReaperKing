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

namespace ReaperKing.Core
{
    /**
     * Registers a mount on default include namespace for its
     * lifetime.
     */
    public struct TemplateDefaultMount : IDisposable
    {
        private Site _site;
        private string[] _roots;

        public TemplateDefaultMount(Site site, string root)
            : this(site, new [] { root })
        { }

        public TemplateDefaultMount(Site site, string[] roots)
        {
            _site = site;
            _roots = roots;

            foreach (var root in _roots)
            {
                _site.TryAddTemplateDefaultIncludePath(root);
            }
        }
        
        public void Dispose()
        {
            foreach (var root in _roots)
            {
                _site.RemoveTemplateDefaultIncludePath(root);
            }

            _roots = null;
            _site = null;
        }
    }
    
    /**
     * Registers a mount on a specific include namespace for its
     * lifetime.
     */
    public readonly struct TemplateNamespaceMount : IDisposable
    {
        private readonly Site _site;
        private readonly string _ns;
        private readonly string _root;

        public TemplateNamespaceMount(Site site, string ns, string root)
        {
            (_site, _ns, _root) = (site, ns, root);
            _site.TryAddTemplateIncludeNamespace(ns, root);
        }

        public void Dispose()
        {
            _site.RemoveTemplateNamespace(_ns, _root);
        }
    }
}