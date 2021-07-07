using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform leftEnd, RightEnd;
    [Space] [SerializeField] private Transform leftConnector, rightConnector;

    [Space] [SerializeField] private Transform Ball; 
    [Space] [SerializeField] private float Movespeed;

    [Space] [SerializeField] private Inputtype inputtype;

    private  Touch Touch;
    private  Touch StartTouch;

    private bool active = true;
    private Rigidbody ballRB;

    private Vector2 StartTouchpos;

    private void Awake()
    {
        Actions.StartGame += StartGame;
    }

    private void StartGame()
    {
        this.enabled = true;
    }

    private void OnEnable()
    {
        ballRB = Ball.GetComponent<Rigidbody>();
        
        Actions.GameOver += ResetPlayer;
        Actions.LevelCleared += ResetPlayer;
        Actions.NextLevel += enableBall;
    }
 
    private void OnDisable()
    {
        Actions.GameOver -= ResetPlayer;
        Actions.LevelCleared -= ResetPlayer;
        Actions.NextLevel -= enableBall;
    }

    void Update()
    {
        if(!active) return;

        if (inputtype == Inputtype.OnlyUp)
        {
            // Player Input
            if (Input.GetMouseButton(0))
            {
                if (Input.mousePosition.x < Screen.width / 2)
                {
                    leftEnd.Translate((Vector3.up * Movespeed) * Time.deltaTime);
                }
                else
                {
                    RightEnd.Translate((Vector3.up * Movespeed) * Time.deltaTime);
                }

            }
        }
        else if(inputtype == Inputtype.UpAndDown)
        {
            #region Touch controls

            if (Input.touchCount > 0)
            {
                Touch = Input.GetTouch(0);
                if (Touch.phase == TouchPhase.Began)
                {
                    StartTouchpos = Touch.position;
                }
                else if (Touch.phase == TouchPhase.Moved)
                {
                    Vector2 currentPos = Touch.position;
                    if (currentPos.x < Screen.width / 2)
                    {
                        if (currentPos.y > StartTouchpos.y)
                        {
                            leftEnd.Translate((Vector3.up * Movespeed) * Time.deltaTime);
                        }
                        else
                        {
                            leftEnd.Translate((Vector3.down * Movespeed) * Time.deltaTime);
                        }
                    }
                    else
                    {
                        if (currentPos.y > StartTouchpos.y)
                        {
                            RightEnd.Translate((Vector3.up * Movespeed) * Time.deltaTime);
                        }
                        else
                        {
                            RightEnd.Translate((Vector3.down * Movespeed) * Time.deltaTime);
                        }
                    }
                }
            }
            #endregion
        }

        //updating the platform angle of each side
        UpdatePlatform();
    }

    void UpdatePlatform()
    {
        leftConnector.LookAt(RightEnd);
        rightConnector.LookAt(leftEnd);
    }

    private void ResetPlayer()
    {
        Vector3 pos = RightEnd.localPosition;
        pos.y = 0;
        RightEnd.localPosition = pos; 
        
        pos = leftEnd.localPosition;
        pos.y = 0;
        leftEnd.localPosition = pos;

        pos = Ball.localPosition;
        pos.x = 0;
        pos.y = 0.5f;
        Ball.localPosition = pos;

       StopBall();

       // Invoke(nameof(enableBall),0.5f);
    }
    
    private void StopBall()
    {
        if(!ballRB)
            ballRB = Ball.GetComponent<Rigidbody>();
        ballRB.velocity = Vector3.zero;
        ballRB.useGravity = false;
        active = false;
        UpdatePlatform();
    }

    private void enableBall( int l)
    {
        active = true;
        ballRB.useGravity = true;
        ballRB.velocity = Vector3.zero;
    }

}

[Serializable]
public enum Inputtype
{
    OnlyUp,
    UpAndDown
}