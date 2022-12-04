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
        protected Vector3Int cameraPoint;
        public Vector3Int CameraPoint {
            get => cameraPoint;
            set {
                cameraPoint = value;
                refreshHint();
            }
        }
        protected virtual Color blockColor {get;set;} = Color.white;
        protected Transform blockParent;
        protected Grid grid;
        protected static Vector3Int[] rotationMargin = {Vector3Int.back,Vector3Int.forward,Vector3Int.left,Vector3Int.right};
        
        private Vector3Int[] translationComb = {
            new(0,-1,0),new(-1,0,0),new(1,0,0),new(0,1,0),new(0,0,-1),new(0,0,1),

            new(1,1,0),new(1,-1,0),new(-1,1,0),new(-1,-1,0),
            new(1,0,1),new(1,0,-1),new(-1,0,1),new(-1,0,-1),
            new(0,1,1),new(0,1,-1),new(0,-1,1),new(0,-1,-1),

            new(1,1,1),new(1,1,-1),new(1,-1,1),new(1,-1,-1),
            new(-1,1,1),new(-1,1,-1),new(-1,-1,1),new(-1,-1,-1)
            };

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
                blocks.Add(GameObject.Instantiate(baseBlock, Vector3.zero, blockParent.rotation, blockParent).GetComponent<Block>().Init(CenterPos+InitCoords[i],blockColor));
            }
        }
        public void DownFull()
        {
            /*
            var maxDown = checkMaxDownY() * Vector3Int.up;
            
            for(int i=0;i<blocks.Count;i++){
                blocks[i].Coord+=maxDown;
                blocks[i].ColliderEnabled = true;
                grid.SetBlocks(blocks[i].Coord,blocks[i]);
            }
            foreach(var b in hintBlocks)
                Transform.Destroy(b.gameObject);
            GameManager.Inst.OnTurnEnd();
            */
        }
        public void DownOne()
        {
            Vector3Int up = Vector3Int.up;
            if(CheckIfSane(blocks, up)){
                CenterPos += up;
                for(int i=0;i<blocks.Count;i++)
                    blocks[i].Coord = CenterPos + InitCoords[i];
            } else {
                for(int i=0;i<blocks.Count;i++){
                    blocks[i].ColliderEnabled = true;
                    grid.SetBlocks(blocks[i].Coord,blocks[i]);
                }
                foreach(var b in hintBlocks)
                    Transform.Destroy(b.gameObject);
                GameManager.Inst.OnTurnEnd();
                return;
            }
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
        private int checkMaxDownY(Vector3Int[] bs)
        {
            var up = Vector3Int.up;
            for (int k=1;;k++)
                if(!CheckIfSane(bs, up * k))
                    return k-1;
        }
        protected void refreshHint()
        {
            if(hintBlocks == null)
            {
                hintBlocks = new List<Block>(InitCoords.Length);
                for(int i=0;i<InitCoords.Length;i++)
                {
                    hintBlocks.Add(GameObject.Instantiate(baseBlock, Vector3.zero, blockParent.rotation, blockParent).GetComponent<Block>().Init(CenterPos+InitCoords[i],blockColor,true));
                }
            }
            var bestFit = GetBestFit(CameraPoint);
            if (bestFit == null) return;

            var tPos = getTranslatedPos((Vector3Int) bestFit);
            var maxDownY = checkMaxDownY(tPos);
            
            var blockCoords = blocks.Select( b => b.Coord );
            for(int i=0;i<hintBlocks.Count;i++)
            {
                var p = tPos[i] + (Vector3Int.up * maxDownY);
                hintBlocks[i].Coord = p;
                hintBlocks[i].gameObject.SetActive(!blockCoords.Contains(p));
            }
        }
        protected Vector3Int[] getTranslatedPos(Vector3Int point)
        {
            Vector3Int[] v = new Vector3Int[InitCoords.Length];
            for(int i=0;i<4;i++)
                v[i] = InitCoords[i] + point;
            return v;
        }
        protected Vector3Int? GetBestFit(Vector3Int centerPoint)
        {
            if(grid.isCoordSane(getTranslatedPos(centerPoint))) return centerPoint;
            foreach(var v in translationComb)
            {
                if(grid.isCoordSane(getTranslatedPos(centerPoint+v))) return centerPoint+v;
            }
            return null;
        }
        public void PlaceOnHint()
        {
            if(hintBlocks == null) return;
            for(int i=0;i<blocks.Count;i++){
                blocks[i].Coord = hintBlocks[i].Coord;
                blocks[i].ColliderEnabled = true;
                grid.SetBlocks(blocks[i].Coord,blocks[i]);
            }
            foreach(var b in hintBlocks)
                Transform.Destroy(b.gameObject);
            GameManager.Inst.OnTurnEnd();
        }
    }
}
