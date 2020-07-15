using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 장비 슬롯
public class UI_EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    public string itemCode;
    public bool isEquiped;
    public ITEM_TYPE type;

    private void Awake()
    {
        itemCode = null;
        isEquiped = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventHandler.instance.ClickedEquipmentSlot(this.transform);
    }
}
