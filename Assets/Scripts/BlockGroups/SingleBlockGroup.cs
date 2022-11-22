using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class SingleBlockGroup : BlockGroup
    {
        protected override Vector3Int[] initCoords => new Vector3Int[]{new(5,0,5),new(5,1,5),new(5,2,5)};
        
    }
}
