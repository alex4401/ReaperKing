using ReaperKing.Core;

namespace ReaperKing.Generation.ARK
{
    [RkProjectProperty("site", "ark")]
    public record BuildConfigurationArk
    {
        public bool ShowExperimentalNotice { get; init; } = false;
        public bool ShowCampaignG2021 { get; init; } = false;
        public bool GenerateInis { get; init; } = false;
        public bool GenerateSupportTables { get; init; } = false;
    }
}