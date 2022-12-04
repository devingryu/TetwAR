using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TAR
{
    public class GameManager : Singleton<GameManager>
    {
        
        public Transform cam;
        public TextMeshProUGUI text;
        [HideInInspector]
        public Map map;
        private float timer = 0f;
        private float targetTime = 2f;
        private int mask;
        private RaycastHit hit;
        
        public void Init(Map map) 
        {
            this.map = map;
            //Debug.Log("Map attached!");
            mask = 1 << LayerMask.NameToLayer("BaseBlock");
            //map.CreateNew();
        }

        void Update()
        {
            if (map == null) return;
            timer += Time.deltaTime;
            if(timer >= targetTime)
            {
                timer = 0f;
                map.DownOne();
            }

            if(Physics.Raycast(cam.position, cam.forward, out hit, 1000f, mask))
            {
                var norm = hit.transform.InverseTransformDirection(hit.normal);
                var v3i = Vector3Int.RoundToInt(norm);
                if (v3i.sqrMagnitude != 1) return;
                v3i = new(v3i.x,-v3i.y,v3i.z);
                text.text = (hit.transform.GetComponent<Block>().Coord + v3i).ToString();
                map.current.CameraPoint = hit.transform.GetComponent<Block>().Coord + v3i;
            }
        }
        public void OnTurnEnd()
        {
            map.grid.CheckRemove();
            map.CreateNew();
        }
        [ContextMenu("XY회전")]
        public void XYClock()
        {
            if(map == null) return;
            map.current.Rotate(BlockGroup.Rotation.XYClock);
        }
        [ContextMenu("XZ회전")]
        public void XZClock()
        {
            if(map == null) return;
            map.current.Rotate(BlockGroup.Rotation.XZClock);
        }
        [ContextMenu("YZ회전")]
        public void YZClock()
        {
            if(map == null) return;
            map.current.Rotate(BlockGroup.Rotation.YZClock);
        }
        public void Translate(Vector3Int d)
        {
            if(map == null) return;
            map.current.Translate(d);
        }
        public void Forward()
        {
            if (map == null) return;
            map.current.Translate(Vector3Int.forward); ;
        }
        public void Back()
        {
            if (map == null) return;
            map.current.Translate(Vector3Int.back); ;
        }
        public void Right()
        {
            if (map == null) return;
            map.current.Translate(Vector3Int.right); ;
        }
        public void Left()
        {
            if (map == null) return;
            map.current.Translate(Vector3Int.left); ;
        }
        public void DownFull()
        {
            if (map == null) return;
            map.current.DownFull();
        }
        public void DownOne()
        {
            if (map == null) return;
            map.current.DownOne();
        }
    }
}
