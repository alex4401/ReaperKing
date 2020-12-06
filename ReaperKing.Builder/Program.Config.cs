using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.Logging;
using ReaperKing.Core;

using UntypedProjectConfig = System.Collections.Generic.Dictionary<object, object>;

namespace ReaperKing.Builder
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public sealed partial class Program
    {
        private static readonly string[] DefaultEnvironmentNames = {
            "default",
            "base",
        };
        private static readonly string[] SupportedApiVersions = {
            "ReaperKing/v2",
        };

        /**
         * Loads project configuration into memory.
         * Main project configuration is always loaded first.
         * 
         * See notes for SetProjectEnvironment about performance
         * concerns.
         */
        private void LoadProjectConfig()
        {
            BuildConfig = Config.Get<BuildConfiguration>();

            VerifyProjectConfigFile(ProjectFilename + ".yaml");
            Config.LoadFile(ProjectFilename + ".yaml");
            SetProjectEnvironment(EnvironmentName);
            
            Config.Override<ImmutableRuntimeConfiguration>(RuntimeConfig);
        }

        private UntypedProjectConfig VerifyProjectConfigFile(string path)
        {
            UntypedProjectConfig project = ParsingUtils.ReadYamlFile<UntypedProjectConfig>(path);
            
            // Ensure that the ApiVersion property is present and
            // valid.
            if (!(project.ContainsKey("apiVersion")
                  && project["apiVersion"] is string apiVersion
                  && SupportedApiVersions.Contains(apiVersion)))
            {
                Log.LogCritical($"The project configuration at \"{path}\" does not use a supported schema.");
                Log.LogCritical("The following are the supported versions:");
                foreach (string version in SupportedApiVersions)
                {
                    Log.LogCritical($"- {version}\n");
                }
                
                Environment.Exit(1);
            }

            return project;
        }

        /**
         * Loads a project environment configuration while taking
         * care of the basing.
         * 
         * Rather expensive, as a single call may parse each
         * tree node (file) twice, but there's not much reason
         * to optimize it at the moment as this method is
         * expected to be rarely executed in program's lifetime.
         */
        private void SetProjectEnvironment(string environmentName)
        {
            // Check if this is an attempt to load the base project
            // configuration, and return early if it is.
            if (DefaultEnvironmentNames.Contains(environmentName))
            {
                return;
            }
            
            // Load requested project to check its parent config.
            string filename = $"{ProjectFilename}.{environmentName}.yaml";
            UntypedProjectConfig environment = VerifyProjectConfigFile(filename);

            // Load base configuration if one is specified.
            if (environment.ContainsKey("inherits")
                && environment["inherits"] is string baseConfig
                && !String.IsNullOrEmpty(baseConfig))
            {
                SetProjectEnvironment(baseConfig);
            }

            // Load requested project while overriding existing
            // fields.
            Config.LoadFile(filename);
            
            // Append pre-build commands
            if (BuildConfig.ExtraBeforeBuild.Count > 0)
            {
                BuildConfig.BeforeBuild.AddRange(BuildConfig.ExtraBeforeBuild);
                BuildConfig.ExtraBeforeBuild.Clear();
            }
        }
    }
}