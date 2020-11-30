using System;

namespace ReaperKing.Anhydrate.Models
{
    public record FooterInfo
    {
        public string CopyrightMessage { get; init; }
        public string[] Paragraphs { get; init; } = Array.Empty<string>();
    }
}