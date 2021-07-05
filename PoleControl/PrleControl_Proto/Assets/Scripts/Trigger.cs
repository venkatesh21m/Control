using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{

    [SerializeField] private bool goal;
    [SerializeField] private bool collectable;
    private void OnTriggerEnter(Collider other)
    {
        if (goal)
        {
            if(other.CompareTag("Ball"))
                Actions.LevelCleared?.Invoke();
        }
        else if (collectable)
        {
            if (other.CompareTag("Ball"))
            {
                Actions.CollectableCollected?.Invoke();
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            if(other.CompareTag("Ball"))
                Actions.GameOver?.Invoke();
        }
    }
}
