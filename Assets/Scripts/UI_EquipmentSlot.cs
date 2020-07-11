using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string itemCode;
    public bool isEquiped;
    public ITEM_TYPE type;

    private void Awake()
    {
        itemCode = null;
        isEquiped = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventHandler.instance.ClickedEquipmentSlot(this.transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
