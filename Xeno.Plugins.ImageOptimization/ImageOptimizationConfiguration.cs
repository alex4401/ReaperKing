/*!
 * This file is a part of Xeno, and the project's repository may be found at https://github.com/alex4401/rk.
 *
 * The project is free software: you can redistribute it and/or modify it under the terms of the GNU General Public
 * License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
 * version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with this program. If not, see
 * http://www.gnu.org/licenses/.
 */

using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace Xeno.Plugins
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public record ImageOptimizationConfiguration
    {
        public PngConfiguration Png { get; } = new();
        public JpegConfiguration Jpeg { get; } = new();
        
        public record PngConfiguration
        {
            public bool UseOxipng { get; } = true;
            public string OxipngBinaryPath { get; } = "oxipng";
            public int PngCompressionLevel { get; } = 3;
        }
        
        public record JpegConfiguration
        {
            public bool UseRecompress { get; } = true;
            public string RecompressBinaryPath { get; } = "jpeg-recompress";
            public string RecompressAlgorithm { get; } = "ssim";
            public int MinQualityLevel { get; } = 60;
        }
    }
}