using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class LBlockGroup : BlockGroup
    {
        private Vector3Int[] initCoords = new Vector3Int[]{new(1,1,0),new(0,1,0),new(0,0,0),new(0,-1,0)};
        protected override Vector3Int[] InitCoords {
            get => initCoords;
            set {
                initCoords = value;
            }
        }
        private Vector3Int centerPos = new(5,1,5);
        protected override Vector3Int CenterPos {
            get => centerPos;
            set {
                centerPos = value;
            }
        }
        
    }
}
