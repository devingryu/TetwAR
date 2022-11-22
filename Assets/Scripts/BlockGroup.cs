using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public abstract class BlockGroup 
    {
        public List<Block> blocks;
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
            blocks = new List<Block>(initCoords.Length);
            for(int i=0;i<initCoords.Length;i++)
            {
                blocks.Add(GameObject.Instantiate(baseBlock, Vector3.zero, Quaternion.identity, blockParent).GetComponent<Block>().Init(initCoords[i]));
            }
            blocks.Sort((a,b) => (a.Coord.y > b.Coord.y) ? -1 : 1);
        }
        public void DownOne()
        {
            Vector3Int up = Vector3Int.up;
            Block temp;
            foreach(var b in blocks)
            {
                if ((b.Coord+up).y >= grid.GridBound.y || ((temp = grid.GetBlocks(b.Coord+up)) != null && !temp.isCurrent))
                {
                    GameManager.Inst.OnTurnEnd();
                    foreach(var it in blocks)
                        it.isCurrent = false;
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
