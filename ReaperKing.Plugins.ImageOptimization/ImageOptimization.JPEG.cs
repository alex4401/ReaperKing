using System.Diagnostics;

using ReaperKing.Core;

namespace ReaperKing.Plugins
{
    public partial class RkImageOptimizationModule : RkResourceProcessorModule
    {
        public bool UseMozJpeg { get; } = true;
        public string JpegRecompressBinaryPath { get; } = "jpeg-recompress";
        public int JpegMinCompressionLevel { get; } = 60;
        
        private bool _invokeJpegRecompress(string source, string target)
        {
            if (!UseMozJpeg)
            {
                return false;
            }
            
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = JpegRecompressBinaryPath,
                    Arguments = $"-s -a -q high -m smallfry -n {JpegMinCompressionLevel} -l 10 \"{source}\" \"{target}\"",
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