namespace ReaperKing.StaticConfig.Data
{
    public struct NpcSpawnGroup
    {
        public string AnEntryName { get; set; }
        // ReSharper disable InconsistentNaming
        public string NPCsToSpawn { get; set; }
        public string NPCsSpawnOffsets { get; set; }
        public string NPCsToSpawnPercentageChance { get; set; }
        // ReSharper restore InconsistentNaming
    }
}