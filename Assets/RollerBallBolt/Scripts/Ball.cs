using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollerBallBolt
{
    public class Ball : Bolt.EntityBehaviour<ITransformState>
    {
        /// <summary>
        /// transformをIPlayerStateのTranformに割り当てます。
        /// </summary>
        public override void Attached()
        {
            state.SetTransforms(state.Transform, transform);
        }
    }
}
