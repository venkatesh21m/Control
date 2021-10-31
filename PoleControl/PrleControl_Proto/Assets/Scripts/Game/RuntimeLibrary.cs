using UnityEngine;

namespace Rudrac.Control
{

    public class RuntimeLibrary : MonoBehaviour
    {

        public static RuntimeLibrary Instatce;
        private void Awake()
        {
            Instatce = this;
        }


        public LevelsLibrary LevelsLibrary;

    }
}