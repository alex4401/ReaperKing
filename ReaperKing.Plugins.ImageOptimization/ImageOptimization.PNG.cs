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

using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

using ReaperKing.Core;

namespace ReaperKing.Plugins
{
    public partial class RkImageOptimizationModule
        : RkModule, IRkResourceProcessorModule
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