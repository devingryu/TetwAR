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
        private Type holdBlock = null;
        private int lastType = -1;
        private LineRenderer lr;

        private void Start() {
            lr = GetComponent<LineRenderer>();

            gm = GameManager.Inst;
            gm.Init(this);
            grid.Init(new(5,15,5));
            CreateNew();

            var bound = grid.GridPositionBound;
            Vector3[] eightPoint = {
                new(bound[0].x,bound[0].y,bound[0].z),
                new(bound[0].x,bound[0].y,bound[1].z),
                new(bound[0].x,bound[1].y,bound[0].z),
                new(bound[0].x,bound[1].y,bound[1].z),
                new(bound[1].x,bound[0].y,bound[0].z),
                new(bound[1].x,bound[0].y,bound[1].z),
                new(bound[1].x,bound[1].y,bound[0].z),
                new(bound[1].x,bound[1].y,bound[1].z),
            };
            Vector3[] linePoints = {
                eightPoint[0],eightPoint[2],eightPoint[3],eightPoint[1],
                eightPoint[3],eightPoint[7],eightPoint[5],eightPoint[7],
                eightPoint[6],eightPoint[4],eightPoint[6],eightPoint[2]
            };
            lr.useWorldSpace = false;
            lr.positionCount = 12;
            lr.SetPositions(linePoints);
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
        public void BlockHold()
        {
            if(holdBlock == null)
            {
                holdBlock = current.GetType();
                current.Dispose();
                CreateNew();
            }
            else
            {
                var temp = current.GetType();
                current.Dispose();
                current = (BlockGroup) Activator.CreateInstance(holdBlock);
                current.Init(blockParent);
                holdBlock = temp;
            }
        }
    }
}
