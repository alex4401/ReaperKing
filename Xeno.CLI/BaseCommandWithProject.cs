using System;
using System.Collections;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Xeno.Core;
using Xeno.Core.Configuration;

using UntypedProjectConfig = System.Collections.Generic.Dictionary<object, object>;

namespace Xeno.CLI
{
    internal abstract class BaseCommandWithProject : BaseCommand
    {
        protected static readonly string ProjectFilename = "project";
        protected static readonly string[] DefaultTargetNames = {
            "default",
            "base",
        };

        public ProjectConfigurationManager Config { get; private set; }
        public XenoConfiguration BuildConfig { get; private set; }

        [Option(LongName = "target")]
        public string TargetName { get; } = "default";

        /**
         * Loads project configuration into memory.
         * Main project configuration is always loaded first.
         * 
         * See notes for SetProjectTargetValues about performance
         * concerns.
        */
        protected void LoadProjectConfiguration()
        {
            if (Config.SchemaManager.IsValid("*"))
            {
                Config.UpdateTypesFromSchemaManager("*");
            }

            Log.LogDebug("Loading project configuration along with target values");
            Config.LoadFile(ProjectFilename + ".yaml");
            SetProjectTargetValues(TargetName);
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
        private void SetProjectTargetValues(string target)
        {
            // Check if this is an attempt to load the base project
            // configuration, and return early if it is.
            if (((IList) DefaultTargetNames).Contains(target))
            {
                return;
            }
            
            // Load requested project to check its parent config.
            string filename = $"{ProjectFilename}.{target}.yaml";
            Log.LogDebug($"Loading target configuration for \"{target}\"");
            UntypedProjectConfig targetInfo = ParsingUtils.ReadYamlFile<UntypedProjectConfig>(filename);

            // Load base configuration if one is specified.
            if (targetInfo.ContainsKey("inherits")
                && targetInfo["inherits"] is string baseConfig
                && !String.IsNullOrEmpty(baseConfig))
            {
                SetProjectTargetValues(baseConfig);
            }

            // Load requested project while overriding existing
            // fields.
            Config.LoadFile(filename);
        }

        public override void BeforeExecution()
        {
            Config = new(ApplicationLogging.Factory);
            Config.InjectProperty<XenoConfiguration>("larvae");
            
            Config.InitType<XenoConfiguration>();
            Config.InitType<ImmutableRuntimeConfiguration>();
            
            BuildConfig = Config.Get<XenoConfiguration>();
        }
    }
}