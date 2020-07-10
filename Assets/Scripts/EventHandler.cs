using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventHandler : MonoBehaviour
{
    public static EventHandler instance;

    public InvGridManager sc_InvGridManager;
    public Transform SelectedItem = null;

    private void Awake()
    {
        instance = this;
    }

    public void SetSelectedItem(Transform item)
    {
        if (SelectedItem == null)
        {
            item.GetComponent<Image>().raycastTarget = false;

            SelectedItem = item;
        }
        else
            SelectedItem = null;
    }

    // 인벤 그리드에서 아이템사이즈에 맞는 슬롯 좌표 구하기
    public void SetItemSizeOnGrid(Vert v, Hor h, UI_Slot parentSlot)
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

        //print(maxY + "," + maxX + " " + minY + "," + minX);

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

        print("[" + upY + "," + leftX + "] ~ [" + downY + "," + rightX + "]");
        sc_InvGridManager.SetItemOnGrid(leftX, rightX, downY, upY);
    }
}
