using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Actions 
{
    public static Action SignInComplete;
   public static Action<int> StartGame;
   public static Action GameOver;
   public static Action LevelCleared;
   public static Action CollectableCollected;
   public static Action<int> NextLevel;
}
