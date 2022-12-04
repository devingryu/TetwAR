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
                if(coord != null && !isCurrent) grid.UnsetBlocks(coord);
                coord = value;
                transform.localPosition = grid.Coord2Pos(coord);
                if(!isCurrent) grid.SetBlocks(coord,this);
            }
        }
        public virtual Color color { get; private set; } = Color.white;
        private Grid grid;
        public bool isCurrent = true;
        public bool isHint;
        private bool colliderEnabled = false;
        public bool ColliderEnabled {
            get => colliderEnabled;
            set {
                colliderEnabled = value;
                gameObject.GetComponent<BoxCollider>().enabled = value;
                //Debug.Log($"BoxCollider:{value}");
            }
        }
        public Block Init(Vector3Int coord, Color color, bool isHint = false, bool colliderEnabled = false)
        {
            grid = GameManager.Inst.map.grid;
            Coord = coord;
            this.isHint = isHint;
            this.color = isHint?new Color(color.r,color.g,color.b,0.6f):color;
            ColliderEnabled = colliderEnabled;

            var renderer = GetComponent<MeshRenderer>();
            var newMat = new Material(renderer.material.shader);
            newMat.SetTexture("_MainTex", renderer.material.GetTexture("_MainTex"));
            newMat.color = this.color;
            renderer.material = newMat;
            
            OnInit();
            return this;
        }
        protected virtual void OnInit() {}
    }
}
