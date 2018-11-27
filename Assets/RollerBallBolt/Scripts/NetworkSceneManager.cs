using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollerBallBolt
{
    public struct OBJECT_RECORD
    {
        public int id;
        public Vector3 position;
    }

    public class NetworkSceneManager : MonoBehaviour
    {
        public static NetworkSceneManager Instance
        {
            get;
            private set;
        }

        public List<OBJECT_RECORD> playerPositions {
            get;
            private set;
        }
        public OBJECT_RECORD ballPosition
        {
            get;
            private set;
        }

        private void Awake()
        {
            Instance = this;

            playerPositions = new List<OBJECT_RECORD>();

            GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
            OBJECT_RECORD or;
            foreach(GameObject go in gos)
            {
                or.id = go.GetComponent<BoltEntity>().prefabId.Value;
                or.position = go.transform.position;
                playerPositions.Add(or);
                Destroy(go);
            }

            GameObject ball = GameObject.FindGameObjectWithTag("Ball");
            or.id = ball.GetComponent<BoltEntity>().prefabId.Value;
            or.position = ball.transform.position;
            ballPosition = or;
            Destroy(ball);
        }

        /// <summary>
        /// BoltEntity.prefabId.Valueを受け取って、該当する
        /// プレハブIDで記録された座標を返します。
        /// </summary>
        /// <param name="id">プレハブID</param>
        /// <returns>座標を返します</returns>
        public Vector3 GetPlayerPosition(int id)
        {
            foreach(OBJECT_RECORD or in playerPositions)
            {
                if (or.id == id)
                {
                    return or.position;
                }
            }
            return Vector3.zero;
        }
    }
}
