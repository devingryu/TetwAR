using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TAR
{
    [RequireComponent(typeof(GraphicRaycaster))]
    public class ItemButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private Image image;
        private Sprite[] sprites;
        [SerializeField]
        private UnityEvent onClick;
        
        private void Awake() {
            image = GetComponent<Image>();
            sprites = ResourceDictionary.GetAll<Sprite>("Images/Items");
        }
        private int itemIndex;
        public int ItemIndex {
            get => itemIndex;
            set {
                itemIndex = value;
                image.sprite = sprites[isDown?itemIndex*2+1:itemIndex*2];
            }
        }
        private bool isDown;
        public bool IsDown {
            get => isDown;
            set {
                isDown = value;
                image.sprite = sprites[value?itemIndex*2+1:itemIndex*2];
                if(value) onClick.Invoke();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsDown = false;
        }
    }
}
