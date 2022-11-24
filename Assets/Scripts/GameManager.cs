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
            map.CreateNew();
        }
        [ContextMenu("XY회전")]
        void XYClock()
        {
            map.current.Rotate(BlockGroup.Rotation.XYClock);
        }
        [ContextMenu("XZ회전")]
        void XZClock()
        {
            map.current.Rotate(BlockGroup.Rotation.XZClock);
        }
        [ContextMenu("YZ회전")]
        void YZClock()
        {
            map.current.Rotate(BlockGroup.Rotation.YZClock);
        }
    }
}
