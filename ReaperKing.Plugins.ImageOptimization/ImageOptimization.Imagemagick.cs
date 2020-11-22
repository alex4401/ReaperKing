using System.Diagnostics;

using ReaperKing.Core;

namespace ReaperKing.Plugins
{
    public partial class RkImageOptimizationModule : RkResourceProcessorModule
    {
        private bool _invokeConvert(string source, string target)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "convert",
                    Arguments = $"\"{source}\" \"{target}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            
            process.Start();
            process.WaitForExit();
            return process.ExitCode == 0;
        }
    }
}