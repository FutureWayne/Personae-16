using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace RainbowArt.CleanFlatUI
{
    public class TooltipInDetermine : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        Tooltip tooltip;     
           
        RectTransform cachedRect;
        Camera cachedEnterEventCamera;

        void Start ()
        {
            cachedRect = GetComponent<RectTransform>();            
            tooltip.gameObject.SetActive(false); 
            UpdatePosition();
        }
        
        void Update()
        {
            if(tooltip.gameObject.activeSelf == true && cachedEnterEventCamera != null)
            {
                Vector2 localMousePos;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(cachedRect, Input.mousePosition, cachedEnterEventCamera, out localMousePos))
                {
                    UpdatePosition();
                }
            }
        }

        void UpdatePosition()
        {
            RectTransform tooltipRect = tooltip.GetComponent<RectTransform>();
            RectTransform tooltipParentRect = tooltipRect.parent as RectTransform;
            if(tooltipParentRect == null)
            {
                return;
            }
            Vector2 mousePos;
            bool success = RectTransformUtility.ScreenPointToLocalPointInRectangle(tooltipParentRect, Input.mousePosition, cachedEnterEventCamera, out mousePos);
            Vector3 position = new Vector3(mousePos.x,mousePos.y,0);   
            tooltip.SetTooltipPosition(position,0,0);            
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            cachedEnterEventCamera = eventData.enterEventCamera;    
            tooltip.ShowTooltip();                       
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            cachedEnterEventCamera = null;
            tooltip.HideTooltip();  
        }      
    }
}