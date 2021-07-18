using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public AI_AGENT AGENT;
    public bool agent = default;
    [SerializeField] private bool goal;
    [SerializeField] private bool collectable;
    private void OnTriggerEnter(Collider other)
    {
        if (goal)
        {
            if (other.CompareTag("Ball"))
            {
                if (agent)
                {
                    AGENT.GivepositiveReward();
                }
                else
                {
                    Actions.LevelCleared?.Invoke();
                }
               // SetReward(-1f);
            }
        }
        else if (collectable)
        {
            if (other.CompareTag("Ball"))
            {
                if (agent)
                {

                }
                else
                {
                    Actions.CollectableCollected?.Invoke();
                    this.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (other.CompareTag("Ball"))
            {
                if (agent)
                {
                    AGENT.GiveNegativeReward();
                }
                else
                {
                    Actions.GameOver?.Invoke();
                }
            }
        }
    }
}
