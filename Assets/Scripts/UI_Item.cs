using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSize
{
    public int X;
    public int Y;
}

public class UI_Item : MonoBehaviour, IPointerClickHandler
{
    public Item itemInfo;
    public ItemSize SIZE;
    //public ITEM_TYPE type;
    //public string code;
    //public int x;
    //public int y;
    public bool m_isSelected = false;

    private void Awake()
    {

        //itemInfo.code = code;
        //itemInfo.type = type;
        //SIZE.X = x;
        //SIZE.Y = y;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //m_isSelected = !m_isSelected;
        EventHandler.instance.SetSelectedItem(this.transform);
    }

    private void Update()
    {
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
