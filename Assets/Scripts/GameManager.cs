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
        
        private void Start()
        {
            map.CreateNew();
        }

        void Update()
        {
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
            map.current.Rotate(BlockGroup.Rotation.XYClock);
        }
        [ContextMenu("XZ회전")]
        public void XZClock()
        {
            map.current.Rotate(BlockGroup.Rotation.XZClock);
        }
        [ContextMenu("YZ회전")]
        public void YZClock()
        {
            map.current.Rotate(BlockGroup.Rotation.YZClock);
        }
        public void Translate(Vector3Int d)
            => map.current.Translate(d);
        
    }
}
