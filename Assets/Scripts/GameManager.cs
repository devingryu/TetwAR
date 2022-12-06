using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace TAR
{
    public class GameManager : Singleton<GameManager>
    {
        
        public Transform cam;
        public TextMeshProUGUI text;
        [HideInInspector]
        public Map map;
        [SerializeField]
        private Image holdImage;
        [SerializeField]
        private GameObject holdText;
        private Sprite[] holdSprites;
        private Dictionary<Type,int> spriteMapper = new(){
            {typeof(ABlockGroup),0},
            {typeof(IBlockGroup),1},
            {typeof(LBlockGroup),2},
            {typeof(SquareBlockGroup),4},
            {typeof(TFBlockGroup),5},
            {typeof(SBlockGroup),3}
        };
        private float timer = 0f;
        private float targetTime = 2f;
        private int mask;
        private RaycastHit hit;
        private int score = 0;
        public int Score {
            get => score;
            set {
                score = value;
                // TODO: Update UI on score change
            }
        }
        public bool isRunning = false;
        private void Awake() 
        {
            holdSprites = ResourceDictionary.GetAll<Sprite>("Images/BlockSprites");
        }
        public void Init(Map map) 
        {
            this.map = map;
            mask = 1 << LayerMask.NameToLayer("BaseBlock");
            isRunning = true;
            // map.CreateNew();
        }

        void Update()
        {
            if (!isRunning) return;
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
            if(!isRunning) return;
            SoundManager.Inst.PlayEffect("ClickButton");
            map.current.Rotate(BlockGroup.Rotation.XYClock);
        }
        [ContextMenu("XZ회전")]
        public void XZClock()
        {
            if(!isRunning) return;
            SoundManager.Inst.PlayEffect("ClickButton");
            map.current.Rotate(BlockGroup.Rotation.XZClock);
        }
        [ContextMenu("YZ회전")]
        public void YZCounterClock()
        {
            if(!isRunning) return;
            SoundManager.Inst.PlayEffect("ClickButton");
            map.current.Rotate(BlockGroup.Rotation.YZCounterClock);
        }
        public void Translate(Vector3Int d)
        {
            if(!isRunning) return;
            map.current.Translate(d);
        }
        public void Forward()
        {
            if (!isRunning) return;
            map.current.Translate(Vector3Int.forward); ;
        }
        public void Back()
        {
            if (!isRunning) return;
            map.current.Translate(Vector3Int.back); ;
        }
        public void Right()
        {
            if (!isRunning) return;
            map.current.Translate(Vector3Int.right); ;
        }
        public void Left()
        {
            if (!isRunning) return;
            map.current.Translate(Vector3Int.left); ;
        }
        public void DownFull()
        {
            if (!isRunning) return;
            map.current.DownFull();
        }
        public void DownOne()
        {
            if (!isRunning) return;
            map.current.DownOne();
        }
        public void PlaceOnHint()
        {
            if (!isRunning) return;
            SoundManager.Inst.PlayEffect("PlaceBlock");
            map.current.PlaceOnHint();
        }
        public void OnGameOver()
        {
            Debug.Log("GAME OVER!");
            isRunning = false;
        }
        public void BlockHold()
        {
            if(!isRunning) return;
            var holdType = map.BlockHold();
            SoundManager.Inst.PlayEffect("ClickButton");
            holdImage.sprite = holdSprites[spriteMapper[holdType]];
            holdImage.enabled = true;
            holdText.SetActive(false);
        }
    }
}
