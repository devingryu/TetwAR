using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TAR
{
    [RequireComponent(typeof(GraphicRaycaster))]
    public class Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private Image image;
        [SerializeField]
        private Sprite[] sprites;
        [SerializeField]
        private UnityEvent onClick;
        private bool isDown;
        private void Awake() {
            image = GetComponent<Image>();
        }
        public bool IsDown {
            get => isDown;
            set {
                isDown = value;
                image.sprite = sprites[value?1:0];
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
