using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public abstract class FloatBlockGroup : BlockGroup
    {
        protected virtual Vector3 Margin {get;set;} = new(0.5f,0.5f,0.5f);
        protected abstract Vector3[] ICoords {get;set;}

        public override void Rotate(Rotation r)
        {
            var iNewCoords = new Vector3[InitCoords.Length];
            var newCoords = new Vector3Int[InitCoords.Length];
            switch(r)
            {
                case Rotation.XYClock:
                    for(int i=0;i<ICoords.Length;i++)
                    {
                        var ic = new Vector3(-ICoords[i].y,ICoords[i].x,ICoords[i].z);
                        var c = new Vector3Int((int)(-ICoords[i].y+Margin.x),(int)(ICoords[i].x+Margin.y),(int)(ICoords[i].z+Margin.z));
                        if(!grid.isCoordSane(CenterPos+c)) return;
                        newCoords[i] = c;
                        iNewCoords[i] = ic;
                    }
                break;
                case Rotation.XYCounterClock:
                    for(int i=0;i<ICoords.Length;i++)
                    {
                        var c = new Vector3Int((int)(ICoords[i].y+Margin.x),(int)(-ICoords[i].x+Margin.y),(int)(ICoords[i].z+Margin.z));
                        var ic = new Vector3(ICoords[i].y,-ICoords[i].x,ICoords[i].z);
                        if(!grid.isCoordSane(CenterPos+c)) return;
                        newCoords[i] = c;
                        iNewCoords[i] = ic;
                    }
                break;
                case Rotation.XZClock:
                    for(int i=0;i<ICoords.Length;i++)
                    {
                        var c = new Vector3Int((int)(ICoords[i].z+Margin.x),(int)(ICoords[i].y+Margin.y),(int)(-ICoords[i].x+Margin.z));
                        var ic = new Vector3(ICoords[i].z,ICoords[i].y,-ICoords[i].x);
                        if(!grid.isCoordSane(CenterPos+c)) return;
                        newCoords[i] = c;
                        iNewCoords[i] = ic;
                    }
                break;
                case Rotation.XZCounterClock:
                    for(int i=0;i<ICoords.Length;i++)
                    {
                        var c = new Vector3Int((int)(-ICoords[i].z+Margin.x),(int)(ICoords[i].y+Margin.y),(int)(ICoords[i].x+Margin.z));
                        var ic = new Vector3(-ICoords[i].z,ICoords[i].y,ICoords[i].x);
                        if(!grid.isCoordSane(CenterPos+c)) return;
                        newCoords[i] = c;
                        iNewCoords[i] = ic;
                    }
                break;
                case Rotation.YZClock:
                    for(int i=0;i<ICoords.Length;i++)
                    {
                        var c = new Vector3Int((int)(ICoords[i].x+Margin.x),(int)(-ICoords[i].z+Margin.y),(int)(ICoords[i].y+Margin.z));
                        var ic = new Vector3(ICoords[i].x,-ICoords[i].z,ICoords[i].y);
                        if(!grid.isCoordSane(CenterPos+c)) return;
                        newCoords[i] = c;
                        iNewCoords[i] = ic;
                    }
                break;
                case Rotation.YZCounterClock:
                    for(int i=0;i<ICoords.Length;i++)
                    {
                        var c = new Vector3Int((int)(ICoords[i].x+Margin.x),(int)(ICoords[i].z+Margin.y),(int)(-ICoords[i].y+Margin.z));
                        var ic = new Vector3(ICoords[i].x,ICoords[i].z,-ICoords[i].y);
                        if(!grid.isCoordSane(CenterPos+c)) return;
                        newCoords[i] = c;
                        iNewCoords[i] = ic;
                    }
                break;
            }
            InitCoords = newCoords;
            ICoords = iNewCoords;
            for(int i=0;i<blocks.Count;i++)
                blocks[i].Coord = CenterPos + InitCoords[i];
        }
    }
}
