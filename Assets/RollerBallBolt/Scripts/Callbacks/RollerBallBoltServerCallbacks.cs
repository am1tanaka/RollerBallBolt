using UnityEngine;
using RollerBallBolt;

namespace RollerBallBolt
{

    [BoltGlobalBehaviour(BoltNetworkModes.Server, "Game")]
    public class TutorialServerCallbacks : Bolt.GlobalEventListener
    {
        /// <summary>
        /// このプログラムがシーンを読み込んだ時の処理
        /// </summary>
        /// <param name="map"></param>
        public override void SceneLoadLocalDone(string map)
        {
            // プレイヤー1を生成して、操作を担当します
            BoltEntity be = BoltNetwork.Instantiate(BoltPrefabs.Player);
            be.transform.position = NetworkSceneManager.Instance.GetPlayerPosition(be.prefabId.Value);
            be.TakeControl();

            // ボールを作ります
            be = BoltNetwork.Instantiate(BoltPrefabs.Ball);
            be.transform.position = NetworkSceneManager.Instance.ballPosition.position;
        }

        /// <summary>
        /// クライアントがシーンを読み込んだ時に報告されるコールバック
        /// リモート用のプレイヤーを生成
        /// </summary>
        /// <param name="connection"></param>
        public override void SceneLoadRemoteDone(BoltConnection connection)
        {
            // プレイヤー2を生成して、操作を接続先に任せます
            BoltEntity be = BoltNetwork.Instantiate(BoltPrefabs.Player2);
            be.transform.position = NetworkSceneManager.Instance.GetPlayerPosition(be.prefabId.Value);
            be.AssignControl(connection);
        }
    }
}
