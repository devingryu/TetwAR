using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TAR
{//231 94 216
// 149 195 72
// 201 67 74
// 241 211 87
// 96 78 179
    public abstract class BlockGroup 
    {
        public List<Block> blocks;
        public List<Block> hintBlocks;
        protected GameObject baseBlock;
        protected abstract Vector3Int[] InitCoords {get;set;}
        protected abstract Vector3Int CenterPos {get;set;}
        protected virtual Color blockColor {get;set;} = Color.white;
        protected Transform blockParent;
        protected Grid grid;
        protected static Vector3Int[] rotationMargin = {Vector3Int.back,Vector3Int.forward,Vector3Int.left,Vector3Int.right};

        public void Init(Transform blockParent) 
        {
            grid = GameManager.Inst.map.grid;
            baseBlock = ResourceDictionary.Get<GameObject>("Prefabs/BaseBlock");
            this.blockParent = blockParent;
            OnInit();
        }
        protected virtual void OnInit() 
        {
            blocks = new List<Block>(InitCoords.Length);
            for(int i=0;i<InitCoords.Length;i++)
            {
                blocks.Add(GameObject.Instantiate(baseBlock, Vector3.zero, Quaternion.identity, blockParent).GetComponent<Block>().Init(CenterPos+InitCoords[i],blockColor));
            }
            refreshHint();
            //blocks.Sort((a,b) => (a.Coord.y > b.Coord.y) ? -1 : 1);
        }
        public void DownOne()
        {
            Vector3Int up = Vector3Int.up;
            if(CheckIfSane(blocks, up)){
                CenterPos += up;
                for(int i=0;i<blocks.Count;i++)
                    blocks[i].Coord = CenterPos + InitCoords[i];
            } else {
                for(int i=0;i<blocks.Count;i++)
                    grid.SetBlocks(blocks[i].Coord,blocks[i]);
                GameManager.Inst.OnTurnEnd();
                return;
            }
            refreshHint();
        }
        protected bool CheckIfSane(List<Block> lst, Vector3Int disp)
        {
            foreach(var b in lst)
                if (!grid.isCoordSane(b.Coord+disp))
                    return false;
            return true;
        }
        protected bool CheckIfSane(Vector3Int[] coords, Vector3Int disp)
        {
            foreach(var c in coords)
                if (!grid.isCoordSane(c + disp))
                    return false;
            return true;
        }
        public void Translate(Vector3Int d)
        {
            var newCoords = new Vector3Int[InitCoords.Length];
            for(int i=0;i<blocks.Count;i++)
            {
                var c = blocks[i].Coord + d;
                if(!grid.isCoordSane(c)) return;
                newCoords[i] = c;
            }
            CenterPos += d;
            for(int i=0;i<blocks.Count;i++)
                blocks[i].Coord = newCoords[i];
            refreshHint();
        }
        public virtual void Rotate(Rotation r)
        {
            var flag = false;
            var newCoords = new Vector3Int[InitCoords.Length];
            switch(r)
            {
                case Rotation.XYClock:
                    for(int i=0;i<InitCoords.Length;i++)
                    {
                        var c = new Vector3Int(-InitCoords[i].y,InitCoords[i].x,InitCoords[i].z);
                        if(!grid.isCoordSane(CenterPos+c)) flag = true;
                        newCoords[i] = c;
                    }
                break;
                case Rotation.XYCounterClock:
                    for(int i=0;i<InitCoords.Length;i++)
                    {
                        var c = new Vector3Int(InitCoords[i].y,-InitCoords[i].x,InitCoords[i].z);
                        if(!grid.isCoordSane(CenterPos+c)) flag = true;
                        newCoords[i] = c;
                    }
                break;
                case Rotation.XZClock:
                    for(int i=0;i<InitCoords.Length;i++)
                    {
                        var c = new Vector3Int(InitCoords[i].z,InitCoords[i].y,-InitCoords[i].x);
                        if(!grid.isCoordSane(CenterPos+c)) flag = true;
                        newCoords[i] = c;
                    }
                break;
                case Rotation.XZCounterClock:
                    for(int i=0;i<InitCoords.Length;i++)
                    {
                        var c = new Vector3Int(-InitCoords[i].z,InitCoords[i].y,InitCoords[i].x);
                        if(!grid.isCoordSane(CenterPos+c)) flag = true;
                        newCoords[i] = c;
                    }
                break;
                case Rotation.YZClock:
                    for(int i=0;i<InitCoords.Length;i++)
                    {
                        var c = new Vector3Int(InitCoords[i].x,-InitCoords[i].z,InitCoords[i].y);
                        if(!grid.isCoordSane(CenterPos+c)) flag = true;
                        newCoords[i] = c;
                    }
                break;
                case Rotation.YZCounterClock:
                    for(int i=0;i<InitCoords.Length;i++)
                    {
                        var c = new Vector3Int(InitCoords[i].x,InitCoords[i].z,-InitCoords[i].y);
                        if(!grid.isCoordSane(CenterPos+c)) flag = true;
                        newCoords[i] = c;
                    }
                break;
            }
            if(flag)
            {
                foreach(var m in rotationMargin)
                    if(CheckIfSane(newCoords,CenterPos+m))
                    {
                        CenterPos += m;
                        flag = false;
                        break;
                    }
                if(flag) return;
            }
            InitCoords = newCoords;
            for(int i=0;i<blocks.Count;i++)
                blocks[i].Coord = CenterPos + InitCoords[i];
            refreshHint();
        }
        public enum Rotation {
            XYClock,XYCounterClock,XZClock,XZCounterClock,YZClock,YZCounterClock
        }
        protected void refreshHint()
        {
            if(hintBlocks == null)
            {
                hintBlocks = new List<Block>(InitCoords.Length);
                for(int i=0;i<InitCoords.Length;i++)
                {
                    hintBlocks.Add(GameObject.Instantiate(baseBlock, Vector3.zero, Quaternion.identity, blockParent).GetComponent<Block>().Init(CenterPos+InitCoords[i],blockColor,true));
                }
            }
            var up = Vector3Int.up;
            var blockCoords = blocks.Select( b => b.Coord );
            for(int i=1;;i++)
            {
                if(!CheckIfSane(blocks, up * i))
                {
                    for(int j=0;j<hintBlocks.Count;j++)
                    {
                        var p = InitCoords[j] + CenterPos + up * (i-1);
                        hintBlocks[j].Coord = p;
                        hintBlocks[j].gameObject.SetActive(!blockCoords.Contains(p));
                    }
                    break;
                }
            }
        }
    }
}
