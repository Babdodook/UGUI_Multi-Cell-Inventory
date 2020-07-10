using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventHandler : MonoBehaviour
{
    public static EventHandler instance;

    public InvGridManager sc_InvGridManager;
    // 현재 선택중인 아이템(드래그중인)
    public Transform SelectedItem = null;

    private void Awake()
    {
        instance = this;
    }

    // 아이템 선택했을때
    public void SetSelectedItem(Transform item)
    {
        if (SelectedItem == null)
        {
            item.GetComponent<Image>().raycastTarget = false;
            item.GetComponent<UI_Item>().m_isSelected = true;
            SelectedItem = item;
        }
        else
        {
            SelectedItem.GetComponent<UI_Item>().m_isSelected = false;
            SelectedItem = null;
        }
    }

    // 색상 원래대로 되돌리기
    public void SetPrevColor()
    {
        sc_InvGridManager.ChangeColorToPrevColor();
    }

    // 인벤 그리드에서 아이템사이즈에 맞는 슬롯 좌표 구하기
    public void GetItemSizeOnGrid(Vert v, Hor h, UI_Slot parentSlot)
    {
        // 아이템 드래그중 아니면 검사 안함
        if (SelectedItem == null)
            return;

        UI_Item item = SelectedItem.GetComponent<UI_Item>();

        int x = item.SIZE.X;
        int y = item.SIZE.Y;

        int maxX, minX;
        int maxY, minY;

        int upY, downY;
        int rightX, leftX;
        upY = downY = rightX = leftX = 0;

        // 짝
        if (x % 2 == 0)
        {
            maxX = x / 2;
            minX = (x - 1) / 2;
        }
        // 홀
        else
        {
            maxX = minX = x / 2;
        }

        // 짝
        if (y % 2 == 0)
        {
            maxY = y / 2;
            minY = (y - 1) / 2;
        }
        // 홀
        else
        {
            maxY = minY = y / 2;
        }

        switch (v)
        {
            case Vert.UP:
                upY = parentSlot.y - maxY;
                downY = parentSlot.y + minY;

                break;
            case Vert.DOWN:
                upY = parentSlot.y - minY;
                downY = parentSlot.y + maxY;

                break;
        }

        switch (h)
        {
            case Hor.RIGHT:
                leftX = parentSlot.x - minX;
                rightX = parentSlot.x + maxX;
                
                break;
            case Hor.LEFT:
                leftX = parentSlot.x - maxX;
                rightX = parentSlot.x + minX;
                
                break;
        }

        sc_InvGridManager.SetItemOnGrid(leftX, rightX, downY, upY);
    }

    // 슬롯 클릭했을때
    public void ClickedSlot(UI_Slot parentSlot)
    {
        // 현재 드래그 중인 아이템이 없는 경우는??
        // 클릭한 슬롯에 아이템이 있는지 검사
        if(SelectedItem == null)
        {
            if (parentSlot.itemCode == null)
                return;

            sc_InvGridManager.GetItemOnSlot(parentSlot.itemCode);
        }
        // 드래그 중인 아이템 있음
        // 슬롯에 배치해야함
        else if(SelectedItem != null)
        {
            sc_InvGridManager.SetItemOnSlot();
        }
    }

    public string GetItemCode()
    {
        if (SelectedItem == null)
            return null;

        return SelectedItem.GetComponent<UI_Item>().GetItemCode();
    }
}
