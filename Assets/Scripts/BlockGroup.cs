using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public abstract class BlockGroup 
    {
        public Block[] blocks;
        protected GameObject baseBlock;
        protected abstract Vector3Int[] initCoords {get;}
        protected Transform blockParent;
        private Grid grid;
        public void Init(Transform blockParent) 
        {
            grid = GameManager.Inst.map.grid;
            baseBlock = ResourceDictionary.Get<GameObject>("Prefabs/BaseBlock");
            this.blockParent = blockParent;
            OnInit();
        }
        protected virtual void OnInit() 
        {
            blocks = new Block[initCoords.Length];
            for(int i=0;i<initCoords.Length;i++)
            {
                blocks[i] = GameObject.Instantiate(baseBlock, Vector3.zero, Quaternion.identity, blockParent).GetComponent<Block>().Init(initCoords[i]);
            }
        }
        public void DownOne()
        {
            Vector3Int up = Vector3Int.up;
            foreach(var b in blocks)
            {
                if ((b.Coord+up).y >= grid.GridBound.y || grid.GetBlocks(b.Coord+up) != null )
                {
                    GameManager.Inst.OnTurnEnd();
                    return;
                }
            }
            foreach(var b in blocks)
            {
                b.Coord += up;
            }
        }
    }
}
