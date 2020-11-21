using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ReaperKing.Core.Plugins
{
    public partial class RkImageOptimizationModule : RkResourceProcessorModule
    {
        private bool _invokeConvert(string source, string target)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/usr/bin/convert",
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