using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace Rudrac.Control
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private AssetReference[] _levels;
        [SerializeField] private AssetReference[] _obstacle;

        public static int currentLevel = 1;
        public static int levelScore;


        private const string CurrentLevel = nameof(currentLevel);


        // Start is called before the first frame update
        void Start()
        {
            Actions.NextLevel += SetNextLevel;
            Actions.LevelCleared += disableAllObstacles;
            Actions.GameOver += disableAllObstacles;
            Actions.CollectableCollected += CollectableCollected;
            Actions.StartGame += SetNextLevel;
        }

        private void CollectableCollected()
        {
            levelScore++;
        }

        #region Level loader

        private void disableAllObstacles()
        {
            List<Randomiser> activeObstacles = FindObjectsOfType<Randomiser>(true).ToList();
            foreach (var activeObstacle in activeObstacles)
            {
                Addressables.ReleaseInstance(activeObstacle.gameObject);
            }
            activeObstacles.Clear();
        }

        private void SetNextLevel(int level)
        {
            currentLevel = level;
            PlayerPrefs.SetInt(CurrentLevel, currentLevel);

            StartCoroutine(SetUpLevel());
        }

        private IEnumerator SetUpLevel()
        {
            levelScore = 0;
            LevelData levelData = new LevelData();
            var level = Addressables.LoadAssetAsync<TextAsset>(_levels[currentLevel]);
            yield return level;
            string jsondata = level.Result.ToString();
            levelData.LoadFromJsom(jsondata);
            LoadLevel(levelData);
        }

        private void LoadLevel(LevelData levelData)
        {
            foreach (var item in levelData.objects)
            {
                Addressables.InstantiateAsync(_obstacle[((int)item.obstacleType)], item.pos, Quaternion.identity);
            }
        }

        #endregion
    }
}
