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
    }
}
