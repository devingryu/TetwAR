using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TAR
{
    public class Map : MonoBehaviour
    {
        private Type[] blockGroups = {
            typeof(ABlockGroup),
            typeof(IBlockGroup),
            typeof(LBlockGroup),
            typeof(SquareBlockGroup),
            typeof(TFBlockGroup),
            typeof(SBlockGroup),
            };
        private GameManager gm;
        public Grid grid;
        [HideInInspector]
        public BlockGroup current;
        [HideInInspector]
        public int next;
        [SerializeField]
        private Transform blockParent;
        private List<Type> blockQueue = new();
        private int lastType = -1;

        private void Start() {
            gm = GameManager.Inst;
            gm.Init(this);
            grid.Init(new(5,15,5));
            CreateNew();
        }
        public void CreateNew()
        {
            while(blockQueue.Count < 5)
                blockQueue.Add(PickRandom());
            current = (BlockGroup) Activator.CreateInstance(blockQueue[0]);
            for(int i=0;i<4;i++)
                blockQueue[i] = blockQueue[i+1];
            blockQueue[4] = PickRandom();
            current.Init(blockParent);
        }
        public void DownOne()
            => current.DownOne();
        private Type PickRandom()
        {
            int newType;
            while((newType = UnityEngine.Random.Range(0,blockGroups.Length)) == lastType);
            lastType = newType;
            return blockGroups[newType];
        }
    }
}
