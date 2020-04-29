using UnityEngine;
using MLAgents;
using MLAgents.Sensors;

public class Robot : Agent

{
    [Header("速度"), Range(0, 1000)]
    public float speed = 10;

    //機器人鋼體
    private Rigidbody rigRobot;

    //足球鋼體
    private Rigidbody rigBall;

    private void Start()
    {
        rigRobot = GetComponent<Rigidbody>();
        rigBall = GameObject.Find("Ball").GetComponent<Rigidbody>();
    }

    //事件開始時重新設定機器人和足球位置
    public override void OnEpisodeBegin()
    {
        rigRobot.velocity = Vector3.zero;
        rigRobot.angularVelocity = Vector3.zero;
        rigBall.velocity = Vector3.zero;
        rigBall.angularVelocity = Vector3.zero;

        //隨機機器人位置
        Vector3 posRobot = new Vector3(Random.Range(-2f, 2f),0.1f, Random.Range(0f, 3f));
        transform.position = posRobot;

        //隨機足球位置
        Vector3 posBall = new Vector3(Random.Range(0f, 0f), 0.1f, Random.Range(1f, 2f));
        rigBall.position = posBall;

        //球尚未進入球門
        Ball.complete = false;
    }

    //收集觀測資料
    public override void CollectObservations(VectorSensor sensor)
    {
        //加入觀測資料:機器人.足球座標.機器人加速度x.z
        sensor.AddObservation(transform.position);
        sensor.AddObservation(rigBall.position);
        sensor.AddObservation(rigRobot.velocity.x);
        sensor.AddObservation(rigRobot.velocity.z);

    }

    //動作:控制機器人與回饋
    public override void OnActionReceived(float[] vectorAction)
    {
        //使用參數控制機器人
        Vector3 control = Vector3.zero;
        control.x = vectorAction[0];
        control.z = vectorAction[1];
        rigRobot.AddForce(control  *  speed);

        //成功:加1分並結束
        if (Ball.complete)
        {
            SetReward(1);
            EndEpisode();
        }

        //機器人或足球掉到地板下方.失敗:扣1分並結束
        if (transform.position.y < 0 || rigBall.position.y < 0)
        {
            SetReward(-1);
            EndEpisode();
        }
    }

    //探索:讓開發者測試環境
    public override void Heuristic(float[] actionsOut)
    {
        //提供開發者控制的方式
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertica");
        
    }
}
