using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class Block : MonoBehaviour
    {
        private Vector3Int coord;
        public Vector3Int Coord {
            get => coord;
            set {
                if(coord != null) grid.UnsetBlocks(coord);
                coord = value;
                transform.localPosition = grid.Coord2Pos(coord);
                grid.SetBlocks(coord,this);
            }
        }
        public virtual Color32 color { get; private set; } = Color.white;
        private Grid grid;
        public bool isCurrent = true;
        public Block Init(Vector3Int coord)
        {
            grid = GameManager.Inst.map.grid;
            Coord = coord;
            
            OnInit();
            return this;
        }
        protected virtual void OnInit() {}
    }
}
