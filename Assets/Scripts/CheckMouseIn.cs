using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 백그라운드에 마우스 있는지 체크
public class CheckMouseIn : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventHandler.instance.SetPrevColor();
    }
}
