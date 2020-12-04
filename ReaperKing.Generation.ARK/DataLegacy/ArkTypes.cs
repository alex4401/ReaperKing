/*!
 * This file is a part of the open-sourced engine modules for
 * https://alex4401.github.io, and those modules' repository may be found
 * at https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it
 * under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */

using System.Collections.Generic;

namespace ReaperKing.Generation.ARK.Data
{
    public enum RevisionTag
    {
        None,
        ModUpdateLegacy,
        ModUpdate,
        ModInitDataUpdate,
    }

    public struct ModInfo
    {
        public string Name;
        public string InternalId;

        public string Git;
        public ulong SteamId;
        public string OgImageResource;

        public bool PublicIssueTracker;
        public bool WithEpicIni;
        public bool ExcludeFromSitemaps;

        public List<Revision> Revisions;

        public struct Revision
        {
            public RevisionTag Tag;
            public string Date;
            public int SingletonRef;
            public string[] Contents;

            public string PathOnDisk;

            public ModInitializationData InitData;
        }
    }

    public struct MapInfo
    {
        public string Name;
        public string InternalId;
        public string PersistentLevel;

        public GeoInfo Geo;
        public TopomapInfo Topomap;

        public struct GeoInfo
        {
            public int[] Origins;
            public float[] Scales;
        }

        public struct TopomapInfo
        {
            public string Name;
            public string Version;
            public float[] Borders;
        }
    }

    public struct ModInitializationData
    {
        public NestSpotList[] LiveNestSpotDefinitions { get; set; }
    }

    public struct PrimalGameData
    {

    }

    public struct WorldLocation3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

    public struct WorldLocation5
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Lat;
        public float Long;
    }

    public struct NestSpotList
    {
        public string Level { get; set; }
        public WorldLocation3[] Locations { get; set; }
    }
}