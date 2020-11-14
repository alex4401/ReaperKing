using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using RazorLight.Razor;

namespace ReaperKing.Core.Razor
{
    public class RazorScopedFilesystemProject : RazorLightProject
	{
		public const string DefaultExtension = ".cshtml";
		private readonly IFileProvider _fileProvider;
		
		public string Extension { get; set; }
		private readonly List<string> _roots = new List<string>();
		private readonly Dictionary<string, PhysicalFileProvider> _providers = new Dictionary<string, PhysicalFileProvider>();

		public RazorScopedFilesystemProject(string[] roots)
			: this(roots, DefaultExtension)
		{
		}

		public RazorScopedFilesystemProject(string[] roots, string extension)
		{
			Extension = extension ?? throw new ArgumentNullException(nameof(extension));

			foreach (var root in roots)
			{
				AddRoot(root);
			}
		}

		public void AddRoot(string root)
		{
			if (!Directory.Exists(root))
			{
				throw new DirectoryNotFoundException($"Root directory {root} not found");
			}
			
			_roots.Insert(0, root);
			_providers[root] = new PhysicalFileProvider(root);
		}

		public void AddOptionalRoot(string root)
		{
			if (Directory.Exists(root))
			{
				_roots.Insert(0, root);
				_providers[root] = new PhysicalFileProvider(root);
			}
		}

		public void RemoveRoot(string root)
		{
			if (_roots.Contains(root))
			{
				_roots.Remove(root);
				_providers.Remove(root);
			}
		}

		public override Task<RazorLightProjectItem> GetItemAsync(string templateKey)
		{
			if (!templateKey.EndsWith(Extension))
			{
				templateKey = templateKey + Extension;
			}

			FileSystemRazorProjectItem item = null;
			foreach (var root in _roots)
			{
				var provider = _providers[root];
				string absolutePath = NormalizeKey(templateKey, root);
				item = new FileSystemRazorProjectItem(templateKey, new FileInfo(absolutePath));

				if (item.Exists)
				{
					// HACK: this is a dirty hack to fix apparent caching behavior with scoped paths
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