using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class GameManager : Singleton<GameManager>
    {
        public Map map;
        
        private void Start()
        {
            map.CreateNew();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
