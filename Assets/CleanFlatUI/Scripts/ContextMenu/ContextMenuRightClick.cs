using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace RainbowArt.CleanFlatUI
{
    public class ContextMenuRightClick : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        ContextMenu contextMenu;   

        [SerializeField]
        RectTransform areaScope;
       
        void Start()
        {
            contextMenu.gameObject.SetActive(false);    
            contextMenu.OnValueChanged.AddListener(ContextMenuValueChanged); 
        }

        public void OnPointerClick(PointerEventData eventData)
        {            
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Vector2 mousePos = Vector2.zero;
                RectTransform contextMenuRect = contextMenu.gameObject.GetComponent<RectTransform>();
                RectTransform contextMenuParentRect = contextMenuRect.parent as RectTransform;
                if(RectTransformUtility.ScreenPointToLocalPointInRectangle(contextMenuParentRect, Input.mousePosition, eventData.enterEventCamera, out mousePos))
                {
                    contextMenu.Show(mousePos, areaScope); 
                }                    
            }           
        }

        void ContextMenuValueChanged(int index)
        {            
            Debug.Log("ContextMenu value changed, index:"+index);
        } 
    }
}