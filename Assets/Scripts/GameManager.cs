using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

namespace TAR
{
    public class GameManager : Singleton<GameManager>
    {
        public Transform cam;
        [SerializeField]
        private GameObject holdText;
        private Sprite[] holdSprites;
        [SerializeField]
        private Image holdImage;
        [SerializeField]
        private GameObject gameOverUI;
        [SerializeField]
        private GameObject nextTurnUI;
        [SerializeField]
        private ItemButton itemButton;
        private float timer = 0f;
        private float targetTime = 2f;
        private int mask;
        private Dictionary<Type,int> spriteMapper = new(){
            {typeof(ABlockGroup),0},
            {typeof(IBlockGroup),1},
            {typeof(LBlockGroup),2},
            {typeof(SquareBlockGroup),3},
            {typeof(TFBlockGroup),4},
            {typeof(SBlockGroup),5},
            {typeof(TBlockGroup),6},
            {typeof(TIBlockGroup),7},
        };
        [HideInInspector]
        public bool isRunning = false;
        private RaycastHit hit;
        // Multi Area
        [HideInInspector]
        private int currentPlayer;
        [HideInInspector]
        private int maxPlayer;
        [HideInInspector]
        public Map[] map;
        public Map CMap => map[currentPlayer];
        
        private int[] score = {0,0};
        private String scoreText => maxPlayer>1?$"{score[0]}:{score[1]}":$"점수: {score[0]}";
        [SerializeField]
        private TextMeshProUGUI textObject;
        
        [HideInInspector]
        public bool canHold = true;
        private int itemToBeUsed = 0;
        private void Awake() 
        {
            holdSprites = ResourceDictionary.GetAll<Sprite>("Images/BlockSprites");
        }
        public void Init(Map map) 
        {
            currentPlayer = 0;
            maxPlayer = 1;
            this.map = new Map[1] { map };
            mask = 1 << LayerMask.NameToLayer("BaseBlock");
            textObject.text = scoreText;
            isRunning = true;
        }
        public void Init(Map map1, Map map2)
        {
            currentPlayer = 0;
            maxPlayer = 2;
            this.map = new Map[2] { map1, map2 };
            mask = 1 << LayerMask.NameToLayer("BaseBlock");
            textObject.text = scoreText;
            isRunning = true;
        }

        void Update()
        {
            if (!isRunning) return;
            timer += Time.deltaTime;
            if(timer >= targetTime)
            {
                timer = 0f;
                CMap.DownOne();
            }

            if(Physics.Raycast(cam.position, cam.forward, out hit, 1000f, mask))
            {
                var block = hit.transform.GetComponent<Block>();
                if(block.mapID != currentPlayer) return;

                var norm = hit.transform.InverseTransformDirection(hit.normal);
                var v3i = Vector3Int.RoundToInt(norm);
                if (v3i.sqrMagnitude != 1) return;
                v3i = new(v3i.x,-v3i.y,v3i.z);

                CMap.current.CameraPoint = hit.transform.GetComponent<Block>().Coord + v3i;
            }
        }
        public void OnTurnEnd()
        {
            var newPlayer = maxPlayer - currentPlayer - 1;
            ItemUseHandler(newPlayer);
            foreach(var m in map)
                m.grid.CheckRemove();
            map[newPlayer].CreateNew();
            canHold = true;
            currentPlayer = newPlayer;
            RefreshHoldImage();
            RefreshItemImage();
            if (maxPlayer == 2)
            {
                isRunning = false;
                nextTurnUI.gameObject.SetActive(true);
                nextTurnUI.GetComponent<GameOverUI>().SetText($"Player {currentPlayer + 1}");
            }
        }
        [ContextMenu("XY회전")]
        public void XYClock()
        {
            if(!isRunning) return;
            SoundManager.Inst.PlayEffect("ClickButton");
            CMap.current.Rotate(BlockGroup.Rotation.XYClock);
        }
        [ContextMenu("XZ회전")]
        public void XZClock()
        {
            if(!isRunning) return;
            SoundManager.Inst.PlayEffect("ClickButton");
            CMap.current.Rotate(BlockGroup.Rotation.XZClock);
        }
        [ContextMenu("YZ회전")]
        public void YZCounterClock()
        {
            if(!isRunning) return;
            SoundManager.Inst.PlayEffect("ClickButton");
            CMap.current.Rotate(BlockGroup.Rotation.YZCounterClock);
        }
        public void Translate(Vector3Int d)
        {
            if(!isRunning) return;
            CMap.current.Translate(d);
        }
        public void Forward()
        {
            if (!isRunning) return;
            CMap.current.Translate(Vector3Int.forward); ;
        }
        public void Back()
        {
            if (!isRunning) return;
            CMap.current.Translate(Vector3Int.back); ;
        }
        public void Right()
        {
            if (!isRunning) return;
            CMap.current.Translate(Vector3Int.right); ;
        }
        public void Left()
        {
            if (!isRunning) return;
            CMap.current.Translate(Vector3Int.left); ;
        }
        public void DownFull()
        {
            if (!isRunning) return;
            CMap.current.DownFull();
        }
        public void DownOne()
        {
            if (!isRunning) return;
            CMap.current.DownOne();
        }
        public void PlaceOnHint()
        {
            if (!isRunning) return;
            SoundManager.Inst.PlayEffect("PlaceBlock");
            CMap.current.PlaceOnHint();
        }
        public void OnGameOver(int gameOverPlayer)
        {
            Debug.Log("GAME OVER!");
            isRunning = false;
            gameOverUI.gameObject.SetActive(true);
            if (maxPlayer == 1)
                gameOverUI.GetComponent<GameOverUI>().SetText("Game Over");
            else
                gameOverUI.GetComponent<GameOverUI>().SetText($"Player {2 - (1 * gameOverPlayer)} win!");
        }
        public void BlockHold()
        {
            if(!isRunning || !canHold) return;
            canHold = false;
            CMap.BlockHold();
            SoundManager.Inst.PlayEffect("ClickButton");
            RefreshHoldImage();
            // holdImage.sprite = holdSprites[spriteMapper[holdType]];
            // holdImage.enabled = true;
            // holdText.SetActive(false);
        }
        private void RefreshHoldImage()
        {
            var holdBlockType = CMap.HoldBlock;
            if(holdBlockType == null)
            {
                holdImage.enabled = false;
                holdText.SetActive(true);
            }
            else
            {
                holdImage.sprite = holdSprites[spriteMapper[holdBlockType]];
                holdImage.enabled = true;
                holdText.SetActive(false);
            }
        }
        public void ScoreIncrement(int mapID, int val)
        {
            if( 0 > mapID || mapID >= maxPlayer) return;
            score[mapID] = score[mapID] + val;
            if(textObject != null)
                textObject.text = scoreText;
        }
        public void PickItem(int mapID)
        {
            if(maxPlayer <= 1 || 0 > mapID || mapID >= maxPlayer) return;
            map[mapID].itemInfo = UnityEngine.Random.Range(1,4);
            RefreshItemImage();
        }
        private void RefreshItemImage()
        {
            if(itemButton == null) return;
            itemButton.ItemIndex = CMap.itemInfo;
        }
        public void OnItemButtonPressed()
        {
            itemToBeUsed = CMap.itemInfo;
            CMap.itemInfo = 0;
            RefreshItemImage();
        }
        private void ItemUseHandler(int newPlayer)
        {
            switch(itemToBeUsed)
            {
                case 1:
                    map[newPlayer].RandomizeHold();
                break;
                case 2:
                    map[newPlayer].SetUpcomingBlock(map[currentPlayer].current.GetType());
                break;
                case 3:
                    var blockCoords = map[currentPlayer].current.blocks.Select( b => b.Coord ).ToArray();
                    map[newPlayer].CreateBlocks(blockCoords);
                break;
            }
            itemToBeUsed = 0;
        }
    }
}
