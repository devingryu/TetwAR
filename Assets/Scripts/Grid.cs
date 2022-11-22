using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class Grid : MonoBehaviour
    {
        Block[,,] blocks;
        public Vector3Int GridBound {get;private set;}
        private readonly Vector3 GridCenterPoint = new(0f,0f,0f);
        private Vector3 GridZeroPoint; // 좌하단이 Zero
        private Vector3 BlockShape;
        private bool en = false;
        public void Init(Vector3Int bound)
        {
            GridBound = bound;
            blocks = new Block[bound.x,bound.y,bound.z];
            BlockShape = ResourceDictionary.Get<GameObject>("Prefabs/BaseBlock").transform.localScale;
            
            float[] zeroPoint = new float[3];
            for(int i=0;i<3;i++)
                zeroPoint[i] = GridCenterPoint[i] - ((bound[i]%2==0)?(bound[i]/2-0.5f):(bound[i]/2)) * BlockShape[i];
            GridZeroPoint = new Vector3(zeroPoint[0],BlockShape[1]*(bound.y-1),zeroPoint[2]);

            en = true;
        }
        public Vector3 Coord2Pos(Vector3Int coord)
        {
            return GridZeroPoint + 
                new Vector3(
                    BlockShape.x * coord.x,
                    -BlockShape.y * coord.y,
                    BlockShape.z * coord.z
                );
        }

        public void SetBlocks(Vector3Int coord, Block value)
            => blocks[coord.x,coord.y,coord.z] = value;
        public void UnsetBlocks(Vector3Int coord)
            => blocks[coord.x,coord.y,coord.z] = null;
        public Block GetBlocks(Vector3Int coord)
            => blocks[coord.x,coord.y,coord.z];
        // public void MoveBlock(Vector3Int coord, Vector3Int newCoord)
        // {
        //     blocks[newCoord.x,newCoord.y,newCoord.z] = blocks[coord.x,coord.y,coord.z];
        //     blocks[coord.x,coord.y,coord.z] = null;
        // }
    }
}
