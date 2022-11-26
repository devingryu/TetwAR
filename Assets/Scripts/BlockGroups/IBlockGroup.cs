using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class IBlockGroup : FloatBlockGroup
    {
        protected override Vector3[] ICoords {get;set;} = new Vector3[]{new(0.5f,-1.5f,-0.5f),new(0.5f,-0.5f,-0.5f),new(0.5f,0.5f,-0.5f),new(0.5f,1.5f,-0.5f),};
        protected override Vector3Int[] InitCoords {get;set;} = new Vector3Int[]{new(1,-1,0),new(1,0,0),new(1,1,0),new(1,2,0),};
        protected override Vector3Int CenterPos {get;set;} = new(4,1,5);
        protected override Color blockColor {get;set;} = new Color32(149,195,72,255);
    }
}
