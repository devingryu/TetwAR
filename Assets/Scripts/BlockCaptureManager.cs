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
            };
        void Start()
        {
            int num = 5;
            var current = (BlockGroup) Activator.CreateInstance(blockGroups[num]);
            var margin = new Vector3Int(0,0,0);
            var baseBlock = ResourceDictionary.Get<GameObject>("Prefabs/BaseBlock");
            var BlockShape = baseBlock.transform.localScale;

            foreach( var b in current._InitCoords )
            {
                var coord = b+margin;
                Vector3 location = new(coord.x * BlockShape.x, coord.y * BlockShape.y, coord.z * BlockShape.z);
                var newBlock = Instantiate(baseBlock,location,Quaternion.identity).GetComponent<Block>();

                var renderer = newBlock.GetComponent<MeshRenderer>();
                var newMat = new Material(renderer.material.shader);
                newMat.SetTexture("_MainTex", renderer.material.GetTexture("_MainTex"));
                newMat.color = current._BlockColor;
                renderer.material = newMat;
            }

            ScreenCapture.CaptureScreenshot($"screenshot{num}.png");
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
