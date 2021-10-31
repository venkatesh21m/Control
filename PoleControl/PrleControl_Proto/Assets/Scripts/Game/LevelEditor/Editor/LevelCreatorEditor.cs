using UnityEditor;
using UnityEngine;


namespace Rudrac.Control
{
    [CustomEditor(typeof(LevelCreator))]
    public class LevelCreatorEditor : Editor
    {

        SerializedObject so;
        SerializedProperty PropVerticalDistance;
        SerializedProperty PropVerticalDistanceAdition;
        SerializedProperty PropInstancedObjects;
        SerializedProperty PropPathToSave;
        SerializedProperty ProplevelName;

        LevelCreator LevelCreator;
        private void OnEnable()
        {
            LevelCreator = (LevelCreator)target;
            so = serializedObject;
            PropVerticalDistance = so.FindProperty("VerticalDistance");
            PropVerticalDistanceAdition = so.FindProperty("VerticalDistanceAdition");
            PropInstancedObjects = so.FindProperty("instancedObjects");
            PropPathToSave = so.FindProperty("PathToSave");
            ProplevelName = so.FindProperty("levelName");
        }




        public override void OnInspectorGUI()
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
                LevelCreator.SaveLevel();
            }
            if (GUILayout.Button("Load"))
            {
                LevelCreator.LoadLevel();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ResetButton()
        {
            if (GUILayout.Button("Reset"))
            {
                LevelCreator.ResetLevel();
            }
        }

        private void ObjectInstantiationButtons()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            if (GUILayout.Button(ObstacleType.Static.ToString()))
            {
                LevelCreator.CreateObstacle(ObstacleType.Static);
            }
            if (GUILayout.Button(ObstacleType.MovableX.ToString()))
            {
                LevelCreator.CreateObstacle(ObstacleType.MovableX);
            }
            if (GUILayout.Button(ObstacleType.MovableY.ToString()))
            {
                LevelCreator.CreateObstacle(ObstacleType.MovableY);
            }
            if (GUILayout.Button(ObstacleType.Rotatable.ToString()))
            {
                LevelCreator.CreateObstacle(ObstacleType.Rotatable);
            }
            if (GUILayout.Button(ObstacleType.MoveXAndRotate.ToString()))
            {
                LevelCreator.CreateObstacle(ObstacleType.MoveXAndRotate);
            }
            if (GUILayout.Button(ObstacleType.MoveYAndRotate.ToString()))
            {
                LevelCreator.CreateObstacle(ObstacleType.MoveYAndRotate);
            }
            if (GUILayout.Button(ObstacleType.collectable.ToString()))
            {
                LevelCreator.CreateObstacle(ObstacleType.collectable);
            }
            if (GUILayout.Button(ObstacleType.Goal.ToString()))
            {
                LevelCreator.CreateObstacle(ObstacleType.Goal);
            }
            EditorGUILayout.EndVertical();

        }
    }
}
