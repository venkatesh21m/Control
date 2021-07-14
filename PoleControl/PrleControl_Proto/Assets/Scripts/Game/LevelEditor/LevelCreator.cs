using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelCreator : MonoBehaviour, ISavable
{

    public int VerticalDistanceAdition = 2;
    public int VerticalDistance = 0;
    public List<GameObject> instancedObjects = new List<GameObject>();
    public LevelData LevelData;
    public string PathToSave; 
    public string levelName;



    public void CreateObstacle(ObstacleType type)
    {
        GameObject obj = Resources.Load<GameObject>(type.ToString());
        obj = Instantiate(obj);
        VerticalDistance += VerticalDistanceAdition;
        obj.transform.position = new Vector2(0, VerticalDistance);

        instancedObjects.Add(obj);
    }


    public void ResetLevel()
    {
        VerticalDistance = 0;
        foreach (var item in instancedObjects)
        {
            DestroyImmediate(item);
        }
        instancedObjects.Clear();
    }


    public void SaveLevel()
    {
        LevelData levelData = new LevelData();
        foreach (var item in instancedObjects)
        {
            objectinLevel ob = new objectinLevel();
            ob.obstacleType = item.GetComponent<Randomiser>()._ObstacleType;
            ob.pos = transform.position;
            levelData.objects.Add(ob);
        }

        PopulateSaveData(levelData);
    }
    public void LoadLevel()
    {
       
    }





    //Saving and Loading
    public void LoadSaveData(LevelData levelData)
    {
        
    }

    public void PopulateSaveData(LevelData levelData)
    {
        string jsontosave = levelData.ToJson();
        string fullpath = Path.Combine(Path.Combine(Application.persistentDataPath, PathToSave), levelName);
        File.WriteAllText(fullpath, jsontosave);
        Debug.Log("FIle saved to " + fullpath);
    }

}
