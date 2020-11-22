using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

using ReaperKing.Core;

namespace ReaperKing.Plugins
{
    public partial class RkImageOptimizationModule : RkResourceProcessorModule
    {
        public bool UseOxipng { get; } = true;
        public string OxipngBinaryPath { get; } = "oxipng";
        public int PngCompressionLevel { get; } = 3;
        
        private bool _invokeOxipng(string source, string target)
        {
            if (!UseOxipng)
            {
                return false;
            }
            
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = OxipngBinaryPath,
                    Arguments = $"--strip safe -o {PngCompressionLevel} \"{source}\" --out \"{target}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            
            process.Start();
            process.WaitForExit();
            return process.ExitCode == 0;
        }
        
        private bool _hasAlphaChannel(string filePath)
        {
            Image im = Image.FromFile(filePath);
            if ((im.Flags & (int)ImageFlags.HasAlpha) == 0)
            {
                return false;
            }

            Bitmap bitmap = new Bitmap(im);
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    if (pixel.A != 255)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}