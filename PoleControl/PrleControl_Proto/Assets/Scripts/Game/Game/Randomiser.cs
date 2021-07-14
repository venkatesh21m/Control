using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;

public class Randomiser : MonoBehaviour
{
    [SerializeField] private ObstacleType ObstacleType;

    public ObstacleType _ObstacleType => ObstacleType;
    public ObstacleType GetObstacleType()
    {
        return ObstacleType;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Actions.LevelCleared += Randomise;
        switch (ObstacleType)
        {
            case ObstacleType.Static:
                //Randomise();
                break;
            case ObstacleType.MovableX:
                MovableObstacleX(2.5f);
                break;
            case ObstacleType.MovableY:
                //Randomise();
                MovableObstacleY(2.5f);
                break;
            case ObstacleType.Rotatable:
                RotateObstacle();
                break;
            case ObstacleType.MoveXAndRotate:
                RotateObstacle();
                MovableObstacleX(5f);
                break;
            case ObstacleType.MoveYAndRotate:
                RotateObstacle();
                MovableObstacleY(5f);
                break;
            default:
                break;
        }
    }


    private void OnDestroy() {
        Actions.LevelCleared -= Randomise;
    }

    public void Randomise()
    {
        Vector3 pos = transform.localPosition;
        pos.x = Random.Range(-2, 2);
        pos.y = Random.Range(2, 5);
        transform.localPosition = pos;
    }

    void MovableObstacleX(float time)
    {
        var transform1 = transform;
        var pos = transform1.position;
        if (Random.value > .5f)
        {
            transform1.position = new Vector3(2, pos.y, pos.z);
            var movableSequence = DOTween.Sequence();
            movableSequence.Append(transform.DOMove(new Vector3(-2f, pos.y, pos.z), time)).SetEase(Ease.Linear)
                .Append(transform.DOMove(new Vector3(2f, pos.y, pos.z), time)).SetEase(Ease.Linear).SetLoops(-1).SetEase(Ease.Linear);
        }
        else
        {
            transform1.position = new Vector3(-2, pos.y, pos.z);
            var movableSequence = DOTween.Sequence();
            movableSequence.Append(transform.DOMove(new Vector3(2f, pos.y, pos.z), time)).SetEase(Ease.Linear)
                .Append(transform.DOMove(new Vector3(-2f, pos.y, pos.z), time)).SetEase(Ease.Linear).SetLoops(-1).SetEase(Ease.Linear);
        }
    }

    private void MovableObstacleY(float time)
    {
        var transform1 = transform;
        Vector3 pos = transform1.position;
        if (Random.value > .5f)
        {
            transform1.position = new Vector3(pos.x, pos.y + 1, pos.z);
            var movableSequence = DOTween.Sequence();
            movableSequence.Append(transform.DOMove(new Vector3(pos.x, pos.y - 1, pos.z), time))
            .Append(transform.DOMove(new Vector3(pos.x, pos.y + 1, pos.z), time)).SetLoops(-1).SetEase(Ease.Linear);
        }
        else
        {
            transform1.position = new Vector3(pos.x, pos.y - 1, pos.z);
            var movableSequence = DOTween.Sequence();
            movableSequence.Append(transform.DOMove(new Vector3(pos.x, pos.y + 1, pos.z), time))
                .Append(transform.DOMove(new Vector3(pos.x, pos.y - 1, pos.z), time)).SetLoops(-1).SetEase(Ease.Linear);
        }
    }

    void RotateObstacle()
    {
        Randomise();
        transform.DORotate(new Vector3(0, 0, 180), 5).SetEase(Ease.Linear).SetLoops(-1);
    }
}

[Serializable]
public enum ObstacleType
{
    Static = 0,
    MovableX = 1,
    MovableY = 2,
    Rotatable = 3,
    MoveXAndRotate = 4,
    MoveYAndRotate = 5,
    collectable = 6,
    Goal = 7,
}
