using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class Grid : MonoBehaviour
    {
        private Map map;
        public Block[,,] blocks;
        public Vector3Int GridBound { get; private set; }
        public Vector3[] GridPositionBound { get; private set; }
        private readonly Vector3 GridCenterPoint = new(0f, 0f, 0f);
        private Vector3 GridZeroPoint; // 좌하단이 Zero
        private Vector3 BlockShape;
        private bool en = false;
        [SerializeField]
        private Transform baseParent;
        public void Init(Vector3Int bound, Map map)
        {
            this.map = map;
            GridBound = bound;
            blocks = new Block[bound.x, bound.y, bound.z];
            var baseBlock = ResourceDictionary.Get<GameObject>("Prefabs/BaseBlock");
            BlockShape = baseBlock.transform.localScale;

            float[] zeroPoint = new float[3];
            for (int i = 0; i < 3; i++)
                zeroPoint[i] = GridCenterPoint[i] - ((bound[i] % 2 == 0) ? (bound[i] / 2 - 0.5f) : (bound[i] / 2)) * BlockShape[i];
            GridZeroPoint = new Vector3(zeroPoint[0], BlockShape[1] * (bound.y - 1), zeroPoint[2]);

            var rotation = map.transform.rotation;
            Color color = new Color32(74, 47, 0, 255);
            for (int i = 0; i < bound.x; i++)
                for (int j = 0; j < bound.z; j++)
                    Instantiate(baseBlock, Vector3.zero, rotation, baseParent).GetComponent<Block>().Init(new(i, bound.y, j), color, map, colliderEnabled: true);

            GridPositionBound = new Vector3[2] {
                Coord2Pos(new(0,bound.y-1,0))-BlockShape/2,
                Coord2Pos(new(bound.x-1,0,bound.z-1))+BlockShape/2
            };
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
            => blocks[coord.x, coord.y, coord.z] = value;
        public void UnsetBlocks(Vector3Int coord)
            => blocks[coord.x, coord.y, coord.z] = null;
        public Block GetBlocks(Vector3Int coord)
            => blocks[coord.x, coord.y, coord.z];
        public bool isCoordSane(Vector3Int coord)
            => (
                (0 <= coord.x && coord.x < GridBound.x) &&
                (0 <= coord.y && coord.y < GridBound.y) &&
                (0 <= coord.z && coord.z < GridBound.z) &&
                blocks[coord.x, coord.y, coord.z] == null
            );
        public bool isCoordSane(Vector3Int[] coords)
        {
            foreach (var v in coords)
                if (!isCoordSane(v)) return false;
            return true;
        }
        public void MoveBlock(Vector3Int coord, Vector3Int newCoord)
        {
            blocks[newCoord.x, newCoord.y, newCoord.z] = blocks[coord.x, coord.y, coord.z];
            blocks[coord.x, coord.y, coord.z] = null;
            blocks[newCoord.x, newCoord.y, newCoord.z].Coord = newCoord;
        }

        public void CheckRemove()
        {
            for (int i = 0; i < GridBound.y; i++)
            {
                if (CheckFloor(i))
                    RemoveFloor(i);
            }
        }

        public bool CheckFloor(int y)
        {
            for (int j = 0; j < GridBound.x; j++)
            {
                for (int k = 0; k < GridBound.z; k++)
                {
                    if (blocks[j, y, k] == null)
                        return false;
                }
            }
            return true;
        }

        public void RemoveFloor(int y)
        {
            for (int j = 0; j < GridBound.x; j++)
            {
                for (int k = 0; k < GridBound.z; k++)
                {
                    Destroy(blocks[j, y, k].gameObject);
                    UnsetBlocks(new Vector3Int(j, y, k));
                }
            }
            SoundManager.Inst.PlayEffect("BreakFloor");
            GameManager.Inst.PickItem(map.ID);
            GameManager.Inst.ScoreIncrement(map.ID);
            DownUpperFloor(y);
        }

        public void DownUpperFloor(int y)
        {
            for (int i = y - 1; i >= 0; i--)
            {
                for (int j = 0; j < GridBound.x; j++)
                {
                    for (int k = 0; k < GridBound.z; k++)
                    {
                        if (blocks[j, i, k] != null)
                        {
                            MoveBlock(blocks[j, i, k].Coord, blocks[j, i, k].Coord + new Vector3Int(0, 1, 0));
                        }
                    }
                }
            }
        }
    }
}
