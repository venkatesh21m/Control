using System.Collections.Generic;
using UnityEngine;
namespace Rudrac.Control
{
    [System.Serializable]
    public class LevelData
    {
        public List<objectinLevel> objects = new List<objectinLevel>();

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void LoadFromJsom(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }

    }

    [System.Serializable]
    public struct objectinLevel
    {
        public ObstacleType obstacleType;
        public Vector3 pos;
    }
}
