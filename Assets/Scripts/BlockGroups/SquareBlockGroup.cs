using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class SquareBlockGroup : FloatBlockGroup
    {
        protected override Vector3[] ICoords {get;set;} = new Vector3[]{new(0.5f,0.5f,-0.5f),new(0.5f,-0.5f,-0.5f),new(-0.5f,0.5f,-0.5f),new(-0.5f,-0.5f,-0.5f),};
        protected override Vector3Int[] InitCoords {get;set;} = new Vector3Int[]{new(0,0,0),new(1,0,0),new(1,1,0),new(0,1,0),};
        protected override Vector3Int CenterPos {get;set;} = new(2,1,3);
        protected override Color blockColor {get;set;} = new Color32(241,211,87,255);
    }
}
