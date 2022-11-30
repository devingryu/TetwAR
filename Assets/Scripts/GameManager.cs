using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class GameManager : Singleton<GameManager>
    {
        public Map map;
        private float timer = 0f;
        private float targetTime = 0.7f;
        
        public void Init(Map map) 
        {
            this.map = map;
            Debug.Log("Map attached!");
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
