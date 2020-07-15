using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 아이템 사이즈
public class ItemSize
{
    public int X;
    public int Y;
}

// 아이템
public class UI_Item : MonoBehaviour, IPointerClickHandler
{
    // 아이템 데이터
    public Item itemInfo;
    public ItemSize SIZE;
    public bool m_isSelected = false;

    // 아이템 클릭했을때
    public void OnPointerClick(PointerEventData eventData)
    {
        EventHandler.instance.SetSelectedItem(this.transform);
    }

    private void Update()
    {
        // 마우스 포인터 따라다니도록
        if(m_isSelected)
            transform.position = Input.mousePosition;
    }

    public string GetItemCode()
    {
        return itemInfo.code;
    }

    public ITEM_TYPE GetItemType()
    {
        return itemInfo.type;
    }
}
