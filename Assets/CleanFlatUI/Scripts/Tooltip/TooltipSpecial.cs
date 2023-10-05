using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace RainbowArt.CleanFlatUI
{
    [ExecuteAlways]
    public class TooltipSpecial : MonoBehaviour
    {
        [SerializeField]
        Text description;

        [SerializeField]
        Animator animator; 

        float parentWidth;
        float parentHeight;

        enum Origin
        {
            Top = 0,
            RightTop,
            LeftTop,
            Bottom,
            RightBottom,
            LeftBottom         
        }           
        Origin origin = Origin.Top;     
         
        bool bDelayedUpdate = false;

        public string DescriptionValue
        {
            get
            {
                if(description != null)
                {
                    return description.text;
                }
                return "";               
            }
            set
            {
                if(description != null)
                {
                    description.text = value;
                }  
            }
        } 

        public void InitTooltip(Vector2 mousePosition, RectTransform areaRect)
        { 
            UpdateHeight();                 
            UpdatePosition(mousePosition,areaRect);
        }

        public void ShowTooltip()
        {
            gameObject.SetActive(true);    
            if(animator != null)
            {
                animator.enabled = false;
                animator.gameObject.transform.localScale = Vector3.one;
                animator.gameObject.transform.localEulerAngles = Vector3.zero;
            }   
            PlayAnimation(); 
        }

        public void HideTooltip()
        {
            gameObject.SetActive(false);    
        }   

        void Update()
        {
            if(bDelayedUpdate)
            {
                bDelayedUpdate = false;
                UpdateHeight();
            }
        }
          
        void UpdateHeight()
        {
            if(description != null)
            {
                RectTransform tooltipRect = GetComponent<RectTransform>();
                float finalHeight = description.preferredHeight + 40;
                tooltipRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, finalHeight);
            }
        }

        void UpdatePosition(Vector2 mousePosition, RectTransform areaRect)
        {
            RectTransform tooltipRect = GetComponent<RectTransform>();      
            tooltipRect.localPosition = new Vector3(mousePosition.x,mousePosition.y,0);                                
            Vector3[] corners = new Vector3[4];
            tooltipRect.GetWorldCorners(corners);
            Vector3[] cornersInArea = new Vector3[4];
            for(int i = 0; i < 4; i++)
            {
                cornersInArea[i] = areaRect.InverseTransformPoint(corners[i]); 
            } 
            if(cornersInArea[2].x >= areaRect.rect.xMax)
            {
                if(cornersInArea[2].y >= areaRect.rect.yMax)
                {
                    origin = Origin.LeftBottom;
                }
                else
                {
                    origin = Origin.LeftTop;
                }
            }
            else if(cornersInArea[0].x <= areaRect.rect.xMin)
            {
                if(cornersInArea[2].y >= areaRect.rect.yMax)
                {
                    origin = Origin.RightBottom;
                }
                else
                {
                    origin = Origin.RightTop;
                }
            }
            else 
            {
                if(cornersInArea[2].y >= areaRect.rect.yMax)
                {
                    origin = Origin.Bottom;
                }
                else
                {
                    origin = Origin.Top;
                }                
            }            
            float tooltipPosX = tooltipRect.anchoredPosition3D.x;
            float tooltipPosY = tooltipRect.anchoredPosition3D.y;
            float tooltipWidth = tooltipRect.rect.width;
            float tooltipHeight = tooltipRect.rect.height;
            switch (origin)
            {
                case Origin.Top:
                {
                    break;
                }
                case Origin.RightTop:
                {
                    tooltipPosX = tooltipPosX + tooltipWidth/2;
                    break;
                }
                case Origin.LeftTop:
                {
                    tooltipPosX = tooltipPosX - tooltipWidth/2;
                    break;
                }
                case Origin.Bottom:
                {
                    tooltipPosY = tooltipPosY - tooltipHeight;                    
                    break;
                } 
                case Origin.RightBottom:
                {
                    tooltipPosX = tooltipPosX + tooltipWidth/2;
                    tooltipPosY = tooltipPosY - tooltipHeight;
                    break;
                } 
                case Origin.LeftBottom:
                {
                    tooltipPosX = tooltipPosX - tooltipWidth/2;
                    tooltipPosY = tooltipPosY - tooltipHeight; 
                    break;
                } 
            }             
            Vector3 tooltipPos = new Vector3(tooltipPosX,tooltipPosY,0);            
            tooltipRect.anchoredPosition3D = tooltipPos;  
        }

        void PlayAnimation()
        {
            if(animator != null)
            {
                if(animator.enabled == false)
                {
                    animator.enabled = true;
                }
                animator.Play("Transition",0,0);  
            }            
        }          

        #if UNITY_EDITOR
        protected void OnValidate()
        {
            bDelayedUpdate = true;
        }
        #endif
    }
}