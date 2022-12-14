using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class SBlockGroup : BlockGroup
    {
        protected override Vector3Int[] InitCoords {get;set;} = new Vector3Int[]{new(-1,1,0),new(0,1,0),new(0,0,0),new(1,0,0)};
        protected override Vector3Int CenterPos {get;set;} = new(2,1,2);
        protected override Color blockColor {get;set;} = new Color32(198,116,67,255);
    }
}
