using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class Block : MonoBehaviour
    {
        public Vector3Int coord;
        public virtual Color32 color { get; private set; } = Color.white;
        public void Init(Vector3Int coord)
        {
            this.coord = coord;
                
            OnInit();
        }
        protected virtual void OnInit() {}
    }
}
