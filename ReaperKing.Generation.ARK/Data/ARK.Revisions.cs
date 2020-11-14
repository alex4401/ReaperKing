using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ShellProgressBar;

namespace ReaperKing.Generation.ARK.Data
{
    public partial class DataManagerARK
    {
        private IEnumerable<ModInfo.Revision> _initModRevisions(ChildProgressBar pbar, string modId, string baseMessage)
        {
            var searchPath = Path.Join(GetDataDirectoryPath(), modId);
            string[] revFiles = Directory.GetFiles(searchPath, "revision.yaml", SearchOption.AllDirectories);
            pbar.MaxTicks += revFiles.Length;

            Array.Sort(revFiles);

            foreach(string filepath in revFiles)
            {
                string revisionPath = filepath.Remove(0, GetDataDirectoryPath().Length);
                revisionPath = revisionPath.Remove(revisionPath.Length - 5, 5);
                pbar.Tick($"{baseMessage}: {modId}, rev {revisionPath}");
                
                var revision = ReadYamlFile<ModInfo.Revision>(revisionPath, "revision");
                revision.PathOnDisk = revisionPath.Remove(revisionPath.Length - 8, 8);

                if (revision.Tag == RevisionTag.ModUpdate
                    || revision.Tag == RevisionTag.ModInitDataUpdate)
                {
                    var initDataPath = Path.Join(revision.PathOnDisk, "ModInitialization");
                    var initData = ReadJsonFile<ModInitializationData>(initDataPath);
                    revision.InitData = initData;
                }
                
                yield return revision;
            }
            
            pbar.Tick(baseMessage);
        }

        public IEnumerable<Tuple<int, ModInfo.Revision>> FindModRevisionsByTag(string modId, RevisionTag tag)
        {
            var revList = LoadedMods[modId].Revisions;
            for (int index = revList.Count - 1; index >= 0; index--)
            {
                var revision = revList[index];
                
                if (revision.Tag == tag)
                {
                    yield return new Tuple<int, ModInfo.Revision>(index, revision);
                }
            }
        }

        public IEnumerable<Tuple<int, ModInfo.Revision>> FindModRevisionsByTags(string modId, RevisionTag[] tags)
        {
            var revList = LoadedMods[modId].Revisions;
            for (int index = revList.Count - 1; index >= 0; index--)
            {
                var revision = revList[index];
                
                if (((IList) tags).Contains(revision.Tag))
                {
                    yield return new Tuple<int, ModInfo.Revision>(index, revision);
                }
            }
        }

        public Dictionary<string, int> MapLegacyRevisionsToMaps(string modId)
        {
            var worldRevMap = new Dictionary<string, int>();
            
            // Map out old-style revisions to worlds.
            foreach (var pair in DataManagerARK.Instance.FindModRevisionsByTag(modId, RevisionTag.ModUpdateLegacy))
            {
                var index = pair.Item1;
                var revision = pair.Item2;
                
                foreach (string worldRef in revision.Contents)
                {
                    if (!worldRevMap.ContainsKey(worldRef))
                    {
                        worldRevMap[worldRef] = index;
                    }
                }
            }

            return worldRevMap;
        }
    }
}