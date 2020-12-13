namespace Noglin.Ark
{
    public static class ApiVersion
    {
        public const string Main = "Noglin.Ark";

        public static class V1
        {
            public const string ModSpecification = Main + ".ModSpecification/v1";
            public const string ProcessorConfiguration = Main + ".ProcessorConfiguration/v1";
        }

        public static class External
        {
            public const string SpawningGroups = "spawn_groups";
            public const string Species = "species";
            public const string NpcSpawningManagers = "maps_npc_spawns";
        }
    }
}