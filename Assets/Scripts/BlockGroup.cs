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
        public void Init(Transform blockParent) 
        {
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
        
    }
}
