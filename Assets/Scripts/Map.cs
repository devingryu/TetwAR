using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TAR
{
    public class Map : MonoBehaviour
    {
        Vector3 lastLoc;
        float threshold = 0.03f;
        float[] offset = {0f,0f,0f};
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

        private void Start() {
            gm = GameManager.Inst;
            gm.Init(this);
            grid.Init(new(6,15,6));
            CreateNew();
        }
        private void Update() {
            if(lastLoc == null)
            {
                lastLoc = transform.position;
                return;
            }
            var disp = lastLoc - transform.position;
            for(int i=0;i<3;i+=2)
            {
                offset[i] += disp[i];
                if(offset[i] < -threshold || offset[i] > threshold)
                {
                    int sign = offset[i]<0?-1:1;
                    switch(i)
                    {
                        case 0: 
                            current.Translate(new Vector3Int(1,0,0) * sign);
                            offset[0] = 0f;
                        break;
                        case 2:
                            current.Translate(new Vector3Int(0,0,1) * sign);
                            offset[2] = 0f;
                        break;
                        default: break;
                    }
                }
            }
            lastLoc = transform.position;
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
