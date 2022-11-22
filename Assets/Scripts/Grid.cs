using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class Grid : MonoBehaviour
    {
        Block[,,] blocks;
        public void Init(Vector3Int bound)
        {
            blocks = new Block[bound.x,bound.y,bound.z];
            
        }
    }
}
