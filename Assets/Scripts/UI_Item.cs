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
    public ItemSize SIZE;
    public bool m_isSelected = false;

    private void Awake()
    {
        SIZE.X = 2;
        SIZE.Y = 2;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_isSelected = !m_isSelected;
        EventHandler.instance.SetSelectedItem(this.transform);
    }

    private void Update()
    {
        if(m_isSelected)
            transform.position = Input.mousePosition;
    }
}
