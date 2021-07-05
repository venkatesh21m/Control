using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public List<Object> objects = new List<Object>();

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
public struct Object
{
    public ObstacleType obstacleType;
    public Vector3 pos;
}

