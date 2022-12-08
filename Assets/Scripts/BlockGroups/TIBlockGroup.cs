using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class TIBlockGroup : FloatBlockGroup
    {
        protected override Vector3[] ICoords {get;set;} = new Vector3[]{new(0.5f,0.5f,-0.5f),new(-0.5f,0.5f,-0.5f),new(-0.5f,0.5f,0.5f),new(-0.5f,1.5f,0.5f),};
        protected override Vector3Int[] InitCoords {get;set;} = new Vector3Int[]{new(1,0,-1),new(0,0,-1),new(0,0,0),new(0,1,0),};
        protected override Vector3Int CenterPos {get;set;} = new(2,1,2);
        protected override Vector3 Margin {get;set;} = new(0.5f,-0.5f,-0.5f);
        protected override Color blockColor {get;set;} = new Color32(0,190,210,255);
    }
}
