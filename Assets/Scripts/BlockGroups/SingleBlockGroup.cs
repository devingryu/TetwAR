using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class SingleBlockGroup : BlockGroup
    {
        protected override Vector3Int[] initCoords => new Vector3Int[]{new(0,-1,0),new(0,0,0),new(0,1,0)};
        private Vector3Int centerPos = new(5,1,5);
        protected override Vector3Int CenterPos {
            get => centerPos;
            set {
                centerPos = value;
            }
        }
        
    }
}
