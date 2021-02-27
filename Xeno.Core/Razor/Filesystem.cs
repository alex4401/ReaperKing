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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using RazorLight.Razor;

namespace Xeno.Core.Razor
{
    public struct RazorIncludePathInfo
	{
		public string Namespace;
		public string RealRoot;
	}
	
    public class RazorScopedFilesystemProject : RazorLightProject
	{
		public const string DefaultExtension = ".cshtml";
		
		public string Extension { get; set; }
		private readonly List<RazorIncludePathInfo> _mounts = new();
		private readonly Dictionary<string, PhysicalFileProvider> _providers = new();

		public RazorScopedFilesystemProject(string defaultRoot, string extension = DefaultExtension)
		{
			Extension = extension ?? throw new ArgumentNullException(nameof(extension));
			Mount(defaultRoot);
		}

		public void Mount(RazorIncludePathInfo info)
		{
			if (!Directory.Exists(info.RealRoot))
			{
				throw new DirectoryNotFoundException($"Directory {info.RealRoot} not found");
			}

			MountUnsafe(info);
		}

		public void Mount(string path)
		{
			Mount(new RazorIncludePathInfo
			{
				Namespace = "",
				RealRoot = path,
			});
		}

		public void MountUnsafe(RazorIncludePathInfo info)
		{
			if (Directory.Exists(info.RealRoot))
			{
				_mounts.Insert(0, info);
				if (!_providers.ContainsKey(info.RealRoot))
				{
					_providers[info.RealRoot] = new PhysicalFileProvider(info.RealRoot);
				}
			}
		}

		public void MountUnsafe(string path)
		{
			MountUnsafe(new RazorIncludePathInfo
			{
				Namespace = "",
				RealRoot = path,
			});
		}

		public void DestroyNamespace(string ns)
		{
			_mounts.RemoveAll(info => info.Namespace == ns);
		}

		public void DestroyNamespace(string ns, string path)
		{
			_mounts.RemoveAll(info => info.Namespace == ns && info.RealRoot == path);
		}

		public void DiscardRoot(string path)
		{
			_mounts.RemoveAll(info => info.RealRoot == path);
		}

		public (string, List<RazorIncludePathInfo>) FindMountsForPath(string path)
		{
			var result = new List<RazorIncludePathInfo>();
			var parts = path.Split('/', 2, StringSplitOptions.RemoveEmptyEntries);
			string pathNs = "";
			
			// Find if the input falls into a namespace
			foreach (var info in _mounts)
			{
				if (info.Namespace == parts[0])
				{
					pathNs = info.Namespace;
					break;
				}
			}
			
			// If not namespaced, return all the mounts
			if (pathNs == "")
			{
				return (path, _mounts);
			}
			
			// Remove the namespace from the path
			path = parts[1];
			
			// Find mounts for the namespace
			foreach (var info in _mounts)
			{
				if (info.Namespace != pathNs)
				{
					continue;
				}
				
				result.Add(info);
			}
			
			return (path, result);
		}

		public override Task<RazorLightProjectItem> GetItemAsync(string templateKey)
		{
			if (!templateKey.EndsWith(Extension))
			{
				templateKey = templateKey + Extension;
			}

			FileSystemRazorProjectItem item = null;
			var mounts = FindMountsForPath(templateKey);
			templateKey = mounts.Item1;
			
			foreach (var mount in mounts.Item2)
			{
				string absolutePath = NormalizeKey(templateKey, mount.RealRoot);
				PhysicalFileProvider provider = _providers[mount.RealRoot];
				item = new FileSystemRazorProjectItem(templateKey, new FileInfo(absolutePath));

				if (item.Exists)
				{
					// HACK: this is a dirty hack to fix some caching that doesn't work with scoped paths
                    var cancelToken = new CancellationToken(true);
					var changeToken = new CancellationChangeToken(cancelToken);
					item.ExpirationToken = changeToken; //provider.Watch(templateKey);
					break;
				}
			}
			
			return Task.FromResult((RazorLightProjectItem) item);
		}

		protected string NormalizeKey(string templateKey, string root)
		{
			if (string.IsNullOrEmpty(templateKey))
			{
				throw new ArgumentNullException(nameof(templateKey));
			}

			var absolutePath = templateKey;
			if (!absolutePath.StartsWith(root, StringComparison.OrdinalIgnoreCase))
			{
				if (templateKey[0] == '/' || templateKey[0] == '\\')
				{
					templateKey = templateKey.Substring(1);
				}

				absolutePath = Path.Combine(root, templateKey);
			}

			absolutePath = absolutePath.Replace('\\', '/');

			return absolutePath;
		}

		public override Task<IEnumerable<RazorLightProjectItem>> GetImportsAsync(string templateKey)
		{
			return Task.FromResult(Enumerable.Empty<RazorLightProjectItem>());
		}
    }
}