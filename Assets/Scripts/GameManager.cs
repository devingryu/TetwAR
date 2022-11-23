using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class GameManager : Singleton<GameManager>
    {
        public Map map;
        private float timer = 0f;
        private float targetTime = 1f;
        
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
    }
}
