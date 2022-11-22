using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class GameManager : Singleton<GameManager>
    {
        public Map map;
        public float timer = 0f;
        public float targetTime = 0.3f;
        
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
