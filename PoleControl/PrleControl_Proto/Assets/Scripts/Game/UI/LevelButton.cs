using UnityEngine;

namespace Rudrac.Control.UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text text;
        int levelindex;
        public void SetUpLevelButton(int number)
        {
            levelindex = number;
            text.text = "Level:" + number;
        }

        public void LevelButtonClicked()
        {
            Actions.StartGame?.Invoke(levelindex);
        }

    }
}
