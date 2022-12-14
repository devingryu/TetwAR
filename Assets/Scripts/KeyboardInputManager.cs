using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAR
{
    public class KeyboardInputManager : Singleton<KeyboardInputManager>
    {
        [SerializeField]
        private GameManager gm;
        private float tTimer=0f;
        private float tTimerTarget=0.1f;

        private void Start() {
            gm = GameManager.Inst;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z)) {
                gm.YZCounterClock();
            } if (Input.GetKeyDown(KeyCode.X)) {
                gm.XYClock();
            } if(Input.GetKeyDown(KeyCode.C)) {
                gm.XZClock();
            } if(Input.GetKeyDown(KeyCode.Space)) {
                gm.CMap.current.DownFull();
            }

            tTimer += Time.deltaTime;
            if((tTimer = tTimer>tTimerTarget?tTimerTarget:tTimer)<tTimerTarget) return;
            tTimer = 0f;
            if (Input.GetKey(KeyCode.UpArrow)) {
                gm.CMap.current.Translate(Vector3Int.forward);
            } if (Input.GetKey(KeyCode.RightArrow)) {
                gm.CMap.current.Translate(Vector3Int.right);
            } if (Input.GetKey(KeyCode.DownArrow)) {
                gm.CMap.current.Translate(Vector3Int.back);
            } if (Input.GetKey(KeyCode.LeftArrow)) {
                gm.CMap.current.Translate(Vector3Int.left);
            }
        }
    }
}
