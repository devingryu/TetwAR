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
            };
        private GameManager gm;
        public Grid grid;
        [HideInInspector]
        public BlockGroup current;
        [HideInInspector]
        public int next;
        [SerializeField]
        private Transform blockParent;

        private void Start() {
            gm = GameManager.Inst;
            gm.Init(this);
            grid.Init(new(6,15,6));
            CreateNew();
        }
        public void CreateNew()
        {
            var rand = UnityEngine.Random.Range(0,blockGroups.Length);
            current = (BlockGroup) Activator.CreateInstance(blockGroups[rand]);
            current.Init(blockParent);
        }
        public void DownOne()
        {
            current.DownOne();
        }
    }
}
