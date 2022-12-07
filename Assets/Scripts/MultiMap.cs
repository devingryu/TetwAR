using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class MultiMap : MonoBehaviour
    {
        [SerializeField]
        private Map[] maps;
        private void Start()
        {
            GameManager.Inst.Init(maps[0],maps[1]);
            maps[0].OnStart(0);
            maps[1].OnStart(1);

            maps[0].CreateNew();
        }
    }
}
