using Rudrac.Control.UI;
using System;

namespace Rudrac.Control
{
    [Serializable]
    public class Actions
    {
        public static Action SignInComplete;
        public static Action<int> StartGame;
        public static Action GameOver;
        public static Action LevelCleared;
        public static Action CollectableCollected;
        public static Action<int> NextLevel;
        public static Action<UiState> ChangeUIState;
    }
}
