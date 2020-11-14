namespace ReaperKing.StaticConfig.Data
{
    public struct PrimalGameData
    {
        public string ModName { get; set; }
        public string ModDescription { get; set; }
        // ReSharper disable once InconsistentNaming
        public NpcSpawnEntriesContainerAddition[] TheNPCSpawnEntriesContainerAdditions { get; set; }
    }
}