using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace TAR
{
    public class BlockCaptureManager : MonoBehaviour
    {
        private Type[] blockGroups = {
            typeof(ABlockGroup),
            typeof(IBlockGroup),
            typeof(LBlockGroup),
            typeof(SquareBlockGroup),
            typeof(TFBlockGroup),
            typeof(SBlockGroup),
            typeof(TBlockGroup),
            typeof(TIBlockGroup)
            };
        void Start()
        {
            StartCoroutine(capture());
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        private IEnumerator capture()
        {
            for(int num=0;num<blockGroups.Length;num++)
            {
                var current = (BlockGroup) Activator.CreateInstance(blockGroups[num]);
                var margin = new Vector3Int(0,0,0);
                var baseBlock = ResourceDictionary.Get<GameObject>("Prefabs/BaseBlock");
                var BlockShape = baseBlock.transform.localScale;
                var blocks = new List<Block>(4);
                foreach( var b in current._InitCoords )
                {
                    var coord = b+margin;
                    Vector3 location = new(coord.x * BlockShape.x, coord.y * BlockShape.y, coord.z * BlockShape.z);
                    var newBlock = Instantiate(baseBlock,location,Quaternion.identity).GetComponent<Block>();
                    blocks.Add(newBlock);

                    var renderer = newBlock.GetComponent<MeshRenderer>();
                    var newMat = new Material(renderer.material.shader);
                    newMat.SetTexture("_MainTex", renderer.material.GetTexture("_MainTex"));
                    newMat.color = current._BlockColor;
                    renderer.material = newMat;
                }
                yield return null;
                ScreenCapture.CaptureScreenshot($"screenshot{num}.png");
                Debug.Log($"Captured {num}");
                yield return new WaitForSeconds(1);
                foreach(var b in blocks)
                    Destroy(b.gameObject);
                blocks.Clear();
            }
        }
    }
    
    
}
