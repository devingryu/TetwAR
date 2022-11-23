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
        protected abstract Vector3Int CenterPos {get;set;}
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
                blocks.Add(GameObject.Instantiate(baseBlock, Vector3.zero, Quaternion.identity, blockParent).GetComponent<Block>().Init(CenterPos+initCoords[i]));
            }
            //blocks.Sort((a,b) => (a.Coord.y > b.Coord.y) ? -1 : 1);
        }
        public void DownOne()
        {
            Vector3Int up = Vector3Int.up;
            if(CheckIfSane(blocks, up)){
                CenterPos += up;
                for(int i=0;i<blocks.Count;i++)
                    blocks[i].Coord = CenterPos + initCoords[i];
            } else {
                GameManager.Inst.OnTurnEnd();
                foreach(var it in blocks)
                    it.isCurrent = false;
                return;
            }
        }
        private bool CheckIfSane(List<Block> lst, Vector3Int disp)
        {
            foreach(var b in lst)
                if ((b.Coord+disp).y >= grid.GridBound.y || grid.GetBlocks(b.Coord+disp) != null)
                    return false;
            return true;
        }
    }
}
