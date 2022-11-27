using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class SingleBlockGroup : BlockGroup
    {
        protected override Vector3Int[] InitCoords {get;set;} = new Vector3Int[]{new(0,0,0)};
        protected override Vector3Int CenterPos {get;set;} = new(3,1,3);
        
    }
}
