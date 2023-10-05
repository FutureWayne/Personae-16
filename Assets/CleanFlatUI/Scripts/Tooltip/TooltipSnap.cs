using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace RainbowArt.CleanFlatUI
{
    public class TooltipSnap : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        TooltipSpecial tooltip;    
        
        RectTransform areaRect;
        Camera cachedEnterEventCamera;
        
        void Start ()
        {
            areaRect = GetComponent<RectTransform>();            
            tooltip.gameObject.SetActive(false); 
            UpdatePosition();
        }
        void Update()
        {
            if(tooltip.gameObject.activeSelf == true && cachedEnterEventCamera != null)
            {
                Vector2 localMousePos;
                if(RectTransformUtility.ScreenPointToLocalPointInRectangle(areaRect, Input.mousePosition, cachedEnterEventCamera, out localMousePos))
                {
                    UpdatePosition();
                }
            }
        }

        void UpdatePosition()
        {
            Vector2 mousePos = Vector2.zero;
            RectTransform tooltipRect = tooltip.gameObject.GetComponent<RectTransform>();
            RectTransform tooltipParentRect = tooltipRect.parent as RectTransform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(tooltipParentRect, Input.mousePosition, cachedEnterEventCamera, out mousePos);
            tooltip.InitTooltip(mousePos,areaRect);                       
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