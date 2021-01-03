using System.IO;
using ReaperKing.Core;

namespace ReaperKing.Larvae
{
    internal sealed class BakeCommand : BaseCommandWithSite
    {
        public override int Execute()
        {
            LoadRecipeAssembly();
            LoadProjectConfiguration(new()
            {
                ContentRoot = new FileInfo(ProjectFilename).Directory?.FullName,
                AssemblyRoot = RecipeAssemblyName,
                DeploymentPath = PathUtils.EnsureRooted("public"),
            });

            return 0;
        }
    }
}