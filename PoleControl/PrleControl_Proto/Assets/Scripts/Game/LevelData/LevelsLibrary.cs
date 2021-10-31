using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;


namespace Rudrac.Control
{
    [CreateAssetMenu(fileName = "Levels Library", menuName = "Rudrac/Control/Level Library")]
    public class LevelsLibrary : ScriptableObject
    {
        public List<Chapter> chapters;


        public string GetChapterName(int index)
        {
            return chapters[index].chapterName;
        }
        public string GetChapterDescription(int index)
        {
            return chapters[index].chapterDescription;
        }

        public int GetChapterLevelsCount(int index)
        {
            return chapters[index].levelDatas.Count;
        }


        public Level_Data GetLevelData(int chapter, int level)
        {
            return chapters[chapter].levelDatas[level];
        }

    }

    [System.Serializable]
    public struct Chapter
    {
        [FormerlySerializedAs("ChapterName")] public string chapterName;
        [FormerlySerializedAs("ChapterDescription")] public string chapterDescription;
        public List<Level_Data> levelDatas;
    }


    [System.Serializable]
    public struct Level_Data
    {
        public string levelName;
        public int levelID;
        public string description;
        public bool isCleared;
        [Space]
        public int bestScore;
        [FormerlySerializedAs("Asset")] public AssetReference asset;
    }
}