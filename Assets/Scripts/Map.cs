using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class Map : MonoBehaviour
    {
        public Grid grid;
        [HideInInspector]
        public BlockGroup current;
        [HideInInspector]
        public int next;
        [SerializeField]
        private Transform blockParent;

        private void Awake() {
            grid.Init(new(10,15,10));
        }
        public void CreateNew()
        {
            current = new TFBlockGroup();
            current.Init(blockParent);
        }
        public void DownOne()
        {
            current.DownOne();
        }
        
    }
}
