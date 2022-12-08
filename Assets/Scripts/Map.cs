using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace TAR
{
    public class Map : MonoBehaviour
    {
        public int ID;
        [SerializeField]
        private bool isMulti = false;
        private Type[] blockGroups = {
            typeof(ABlockGroup),
            typeof(IBlockGroup),
            typeof(LBlockGroup),
            typeof(SquareBlockGroup),
            typeof(TFBlockGroup),
            typeof(SBlockGroup),
            typeof(TBlockGroup),
            typeof(TIBlockGroup),
            };
        private GameManager gm;
        public Grid grid;
        [HideInInspector]
        public BlockGroup current;
        [HideInInspector]
        public int next;
        [SerializeField]
        private Transform blockParent;
        private List<Type> blockQueue = new(8);
        private Type holdBlock = null;
        public Type HoldBlock => holdBlock;
        private int lastType = -1;
        private LineRenderer lr;
        public int itemInfo = 0;
        
        private void Start() 
        {
            if(!isMulti)
                OnStart(0);
        }
        public void OnStart(int id)
        {
            ID = id;
            lr = GetComponent<LineRenderer>();

            gm = GameManager.Inst;
            if(!isMulti)
                gm.Init(this);
            grid.Init(new(5,15,5), this);
            if(!isMulti)
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
            if(blockQueue.Count <= 0)
                FillQueue();
            current = (BlockGroup) Activator.CreateInstance(blockQueue[blockQueue.Count-1]);
            blockQueue.RemoveAt(blockQueue.Count-1);
            current.Init(blockParent,this);
        }
        public void DownOne()
            => current.DownOne();
        private void FillQueue()
        {
            blockQueue.Clear();
            var randomized = blockGroups.OrderBy(_ => UnityEngine.Random.Range(-10f,10f));
            blockQueue.AddRange(randomized);
        }
        private Type PickRandom()
        {
            int newType;
            while((newType = UnityEngine.Random.Range(0,blockGroups.Length)) == lastType);
            lastType = newType;
            return blockGroups[newType];
        }
        public Type BlockHold()
        {
            var temp = current.GetType();
            if(holdBlock == null)
            {
                holdBlock = temp;
                current.Dispose();
                CreateNew();
            }
            else
            {
                current.Dispose();
                current = (BlockGroup) Activator.CreateInstance(holdBlock);
                current.Init(blockParent,this);
                holdBlock = temp;
            }
            return temp;
        }
        public void RandomizeHold()
        {
            if(holdBlock == null) return;
            holdBlock = PickRandom();
        }
        public void SetUpcomingBlock(Type type)
        {
            if(blockQueue.Count <= 0)
                FillQueue();
            blockQueue[blockQueue.Count-1] = type;
        }
        public void CreateBlocks(Vector3Int[] coords)
        {
            if(grid == null) return;
            var baseBlock = ResourceDictionary.Get<GameObject>("Prefabs/BaseBlock");
            Color color = new Color32(130, 130, 130, 255);
            foreach (var c in coords)
            {
                if(grid.GetBlocks(c) == null)
                {
                    grid.SetBlocks(c,
                        GameObject.Instantiate(baseBlock, Vector3.zero, blockParent.rotation, blockParent).GetComponent<Block>().Init(c,color,this,colliderEnabled:true)
                    );
                }
            }
        }
    }
}
