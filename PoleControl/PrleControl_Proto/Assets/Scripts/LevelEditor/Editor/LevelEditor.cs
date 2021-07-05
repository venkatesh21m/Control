using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

[CustomEditor(typeof(LevelCreator))]
public class LevelEditor : EditorWindow, ISavable
{
    [MenuItem("puzzle Game/Level creator %#a")]
    public static void NewMenuOption()=> GetWindow<LevelEditor>();

    public int VerticalDistanceAdition = 2;
    public int VerticalDistance = 0;
    public string PathToSave;
    public string levelName;
    public List<GameObject> instancedObjects = new List<GameObject>();
    public LevelData LevelData;


    SerializedObject so;
    SerializedProperty PropVerticalDistance;
    SerializedProperty PropVerticalDistanceAdition;
    SerializedProperty PropInstancedObjects;
    SerializedProperty PropPathToSave;
    SerializedProperty ProplevelName;


    private void OnEnable()
    {
        so = new SerializedObject(this);
        PropVerticalDistance = so.FindProperty("VerticalDistance");
        PropVerticalDistanceAdition = so.FindProperty("VerticalDistanceAdition");
        PropInstancedObjects = so.FindProperty("instancedObjects");
        PropPathToSave = so.FindProperty("PathToSave");
        ProplevelName = so.FindProperty("levelName");
    }



    private void OnGUI()
    {
        so.Update();
        EditorGUILayout.PropertyField(PropVerticalDistance);
        EditorGUILayout.PropertyField(PropVerticalDistanceAdition);

        EditorGUILayout.Space(20);
        GUILayout.Label("Buttons to create Objects", EditorStyles.boldLabel);
        ObjectInstantiationButtons();
        EditorGUILayout.Space(20);

        GUILayout.Label("Instanced Objects", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(PropInstancedObjects);
        EditorGUILayout.Space(5);
        EditorGUILayout.PropertyField(ProplevelName);
        EditorGUILayout.PropertyField(PropPathToSave);
        SaveAndLoadButtons();

        EditorGUILayout.Space(50);
        ResetButton();

        so.ApplyModifiedProperties();
    }

    private void SaveAndLoadButtons()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        if (GUILayout.Button("Save"))
        {
            SaveLevel();
        }
        if (GUILayout.Button("Load"))
        {
            LoadLevel();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void ResetButton()
    {
        if (GUILayout.Button("Reset"))
        {
           ResetLevel();
        }
    }
    Color c;
    private void ObjectInstantiationButtons()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        if (GUILayout.Button(ObstacleType.Static.ToString()))
        {
            CreateObstacle(ObstacleType.Static);
        }
        if (GUILayout.Button(ObstacleType.MovableX.ToString()))
        {
            CreateObstacle(ObstacleType.MovableX);
        }
        if (GUILayout.Button(ObstacleType.MovableY.ToString()))
        {
            CreateObstacle(ObstacleType.MovableY);
        }
        if (GUILayout.Button(ObstacleType.Rotatable.ToString()))
        {
            CreateObstacle(ObstacleType.Rotatable);
        }
        if (GUILayout.Button(ObstacleType.MoveXAndRotate.ToString()))
        {
            CreateObstacle(ObstacleType.MoveXAndRotate);
        }
        if (GUILayout.Button(ObstacleType.MoveYAndRotate.ToString()))
        {
            CreateObstacle(ObstacleType.MoveYAndRotate);
        }
        c = GUI.backgroundColor;
        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button(ObstacleType.collectable.ToString()))
        {
            CreateObstacle(ObstacleType.collectable);
        }
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button(ObstacleType.Goal.ToString()))
        {
            CreateObstacle(ObstacleType.Goal);
        }
        GUI.backgroundColor = c;
        EditorGUILayout.EndVertical();

    }

    #region Functionality methods
    public void CreateObstacle(ObstacleType type)
    {
        GameObject obj = Resources.Load<GameObject>(type.ToString());
        obj = Instantiate(obj);
        VerticalDistance += VerticalDistanceAdition;
        obj.transform.position = new Vector2(UnityEngine.Random.Range(-2.0f,2.0f), VerticalDistance);

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
            Object ob = new Object();
            ob.obstacleType = item.GetComponent<Randomiser>()._ObstacleType;
            ob.pos = item.transform.position;
            levelData.objects.Add(ob);
        }

        PopulateSaveData(levelData);
    }
    public void LoadLevel()
    {
        LevelData levelData = new LevelData();
        string fullpath = Path.Combine(Path.Combine(Application.persistentDataPath, PathToSave), levelName);
        string jsondata = File.ReadAllText(fullpath);
        levelData.LoadFromJsom(jsondata);
        LoadSaveData(levelData);
    }

    #endregion

    //Saving and Loading
    public void LoadSaveData(LevelData levelData)
    {
        ResetLevel();
        foreach (var item in levelData.objects)
        {
            GameObject obj = Resources.Load<GameObject>(item.obstacleType.ToString());
            obj = Instantiate(obj);
            obj.transform.position = item.pos;

            instancedObjects.Add(obj);
        }
    }

    public void PopulateSaveData(LevelData levelData)
    {
        string jsontosave = levelData.ToJson();
        string fullpath = Path.Combine(Path.Combine(Application.persistentDataPath, PathToSave), levelName);
        File.WriteAllText(fullpath, jsontosave);
        Debug.Log("FIle saved to " + fullpath);
    }
}
