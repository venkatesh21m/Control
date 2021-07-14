using UnityEngine;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;


public class AI_AGENT : Agent
{

    [Header("Specific to Ball3D")]
    public GameObject ball;
    public GameObject leftPoint;
    public GameObject rightPoint;
    [Space] public Transform leftConnector;
    public Transform rightConnector;
    public Transform Goal;
    [Space]
    public Vector3 ballpos, leftpointpos, rightpointpos;

    [Space] [SerializeField] private float Movespeed;
    [Space] [SerializeField] private float distanceDifference = 2f;
    [Space]
    public bool useVecObs;

    [Space]
    public List<Randomiser> obstacles;
    Rigidbody m_BallRb;

    EnvironmentParameters m_ResetParams;
    public override void Initialize()
    {
        Actions.LevelCleared -= GivepositiveReward;
        Actions.GameOver -= GiveNegativeReward; 
        Actions.LevelCleared += GivepositiveReward;
        Actions.GameOver += GiveNegativeReward;
        m_BallRb = ball.GetComponent<Rigidbody>();
        m_ResetParams = Academy.Instance.EnvironmentParameters;
        SetResetParameters();
    }

    //private void OnEnable()
    //{
    //    m_BallRb = ball.GetComponent<Rigidbody>();
    //    Actions.LevelCleared += GivepositiveReward;
    //    Actions.GameOver += GiveNegativeReward;
    //}

    //private void OnDisable()
    //{
    //    Actions.LevelCleared -= GivepositiveReward;
    //    Actions.GameOver -= GiveNegativeReward;
    //}
    private void GiveNegativeReward()
    {
        SetReward(-1f);
        EndEpisode();
    }

    private void GivepositiveReward()
    {
        SetReward(1f);
        EndEpisode();
    }

    public void SetBall()
    {
        //Set the attributes of the ball by fetching the information from the academy
        //m_BallRb.mass = m_ResetParams.GetWithDefault("mass", 1.0f);
        //var scale = m_ResetParams.GetWithDefault("scale", 1.0f);
        //ball.transform.localScale = new Vector3(scale, scale, scale);
    }


    private void SetObstacles()
    {
        foreach (var item in obstacles)
        {
            item.Randomise();
        }
    }



    public void SetResetParameters()
    {
        SetBall();
        SetObstacles();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (useVecObs)
        {
            sensor.AddObservation(leftPoint.transform.position.y);
            sensor.AddObservation(leftPoint.transform.position.y);
            sensor.AddObservation(Goal.position);
            sensor.AddObservation(Vector3.Distance(ball.transform.position, leftPoint.transform.position));
            sensor.AddObservation(Vector3.Distance(ball.transform.position, rightPoint.transform.position));
            sensor.AddObservation(Vector3.Distance(ball.transform.position, Goal.position));
            sensor.AddObservation(m_BallRb.velocity);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            continuousActionsOut[0] = 1;
        }
        else
        {
            continuousActionsOut[0] = 0;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            continuousActionsOut[1] = 1;
        }
        else
        {
            continuousActionsOut[1] = 0;
        } 
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var leftaction = actionBuffers.DiscreteActions[0];
        var rightaction = actionBuffers.DiscreteActions[1];
        if(leftaction > 0)
            leftPoint.transform.Translate((Vector3.up * Movespeed) * Time.deltaTime);
        
        if(rightaction > 0)
            rightPoint.transform.Translate((Vector3.up * Movespeed) * Time.deltaTime);


        float balldistancetoleftpoint = Vector3.Distance(ball.transform.position, leftPoint.transform.position);
        float balldistancetorightpoint = Vector3.Distance(ball.transform.position, rightPoint.transform.position);
        float balldistancetoGoalpoint = Vector3.Distance(ball.transform.position, Goal.position);

        float difference = balldistancetoleftpoint - balldistancetorightpoint;
        if (difference > distanceDifference || difference < -distanceDifference || balldistancetoleftpoint > 4.5f || balldistancetoleftpoint > 4.5f)
        {
            SetReward(-1f);
            EndEpisode();
        }
        //else if (difference > distanceDifference/2 || difference < -distanceDifference/2)
        //{
        //    SetReward(-0.5f);
        //}
        //else
        //{
        //    SetReward(0.1f);
        //}
        if (balldistancetoGoalpoint < 1)
        {
            SetReward(0.1f);
        }

        //if (ball.transform.position.y > 10f)
        //{
        //    SetReward(-1f);
        //    EndEpisode();
        //}
    }

    public override void OnEpisodeBegin()
    {
        leftPoint.transform.localPosition = leftpointpos;
        rightPoint.transform.localPosition = rightpointpos;

        m_BallRb.velocity = new Vector3(0f, 0f, 0f);
        ball.transform.localPosition = ballpos;

        //Reset the parameters when the Agent is reset.
        SetResetParameters();
    }





    private void Update()
    {
        UpdatePlatform();
    }

    void UpdatePlatform()
    {
        leftConnector.LookAt(rightPoint.transform);
        rightConnector.LookAt(leftPoint.transform);
    }


}
