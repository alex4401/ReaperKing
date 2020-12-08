namespace Noglin.Core.Types
{
    [PackageType(publicTypeName: "Ark/ModInfo/v1", tag: "mod")]
    public struct ModInfo
    {
        public string Name;
        public SteamInfo Steam;
        public RepositoryInfo Repository;
        public CodexFeatures With;
    }
}