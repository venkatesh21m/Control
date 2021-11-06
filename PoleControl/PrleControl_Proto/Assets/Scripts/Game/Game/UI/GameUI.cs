using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Rudrac.Control.UI
{
    public class GameUI : MonoBehaviour
    {
        public TMP_Text GameTime;
        public void SetTime(string Time) => GameTime.text = Time;
    }
}
