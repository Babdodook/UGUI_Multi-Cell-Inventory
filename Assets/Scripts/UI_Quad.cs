using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Vert
{
    UP=0,
    DOWN,

    Max,
}

public enum Hor
{
    RIGHT = 0,
    LEFT,

    Max,
}

public class UI_Quad : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    UI_Slot parentSlot;

    private void Awake()
    {
        parentSlot = GetComponentInParent<UI_Slot>();
    }

    // 마우스 포인터 들어옴
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 포인터 옮겼으니 이전에 검사했던 슬롯 색상 원래대로 되돌리기
        EventHandler.instance.SetPrevColor();

        // 현재 마우스 포인터 위치 검사
        CheckMousePoint();
    }

    // 클릭했을때
    public void OnPointerClick(PointerEventData eventData)
    {
        EventHandler.instance.ClickedSlot(parentSlot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(parentSlot.isEdge)
            EventHandler.instance.SetPrevColor();
    }

    void CheckMousePoint()
    {
        Vert v = Vert.Max;
        Hor h = Hor.Max;

        // 4분면중에서 어느 면에 포인터가 위치하고 있는가?
        switch (name)
        {
            case "1":
                v = Vert.UP;
                h = Hor.LEFT;

                break;
            case "2":
                v = Vert.UP;
                h = Hor.RIGHT;

                break;
            case "3":
                v = Vert.DOWN;
                h = Hor.LEFT;

                break;
            case "4":
                v = Vert.DOWN;
                h = Hor.RIGHT;

                break;
        }

        EventHandler.instance.GetItemSizeOnGrid(v, h, parentSlot);
    }
}
