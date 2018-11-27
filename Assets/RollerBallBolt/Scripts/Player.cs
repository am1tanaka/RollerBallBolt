using System.Collections;
using System.Collections.Generic;
using Bolt;
using UnityEngine;

public class Player : Bolt.EntityBehaviour<IPlayerState> {

    [Tooltip("送られてきた操作を速度に変換するレート。Mouse XとMouse Yは-1～1に正規化されているので、動作環境の解像度は考えなくて構いません。"), SerializeField]
    float input2Speed = 20f;

    Rigidbody rb;

    float _x;
    float _y;

    #region System Loop

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        PollMouse();
    }

    #endregion System Loop

    #region Bolt Events

    /// <summary>
    /// transformをIPlayerStateのTranformに割り当てます。
    /// </summary>
    public override void Attached()
    {
        state.SetTransforms(state.Transform, transform);
    }

    /// <summary>
    /// 操作権を持つプレイヤー(Player1はホスト、
    /// Player2はクライアント)で呼び出されます。
    /// 操作をBoltEntityに渡します。
    /// </summary>
    public override void SimulateController()
    {
        IRollerBallBoltCommandInput input = RollerBallBoltCommand.Create();

        Vector3 data = new Vector3(_x, _y, 0);
        input.Mouse = data;

        entity.QueueInput(input);
    }

    /// <summary>
    /// オブジェクトのオーナーで呼び出されます。
    /// これはPlayer1, 2ともにホストで呼び出されます。
    /// 入力を受け取って動かします。
    /// Player2からは、クライアントに結果を送信します。
    /// </summary>
    /// <param name="command">送られてきた操作コマンド</param>
    /// <param name="resetState">操作権を持っていたらtrue</param>
    public override void ExecuteCommand(Command command, bool resetState)
    {
        RollerBallBoltCommand cmd = (RollerBallBoltCommand)command;

        if (resetState)
        {
            // Player2。送られてきたコマンドのデータを反映させます
            transform.position = cmd.Result.Position;
        }
        else
        {
            // ホストとクライアントの双方で呼び出されます
            // 現在の座標を送信します
            cmd.Result.Position = transform.position;

            // 入力を使ってオブジェクトを動かします
            Vector3 vel = rb.velocity;
            vel.x = cmd.Input.Mouse.x;
            vel.z = cmd.Input.Mouse.y;
            rb.velocity = vel * input2Speed;
        }
    }

    #endregion Bolt Events

    #region My Methods

    void PollMouse()
    {
        _x = Input.GetAxis("Mouse X");
        _y = Input.GetAxis("Mouse Y");
    }

    #endregion My Methods

}
