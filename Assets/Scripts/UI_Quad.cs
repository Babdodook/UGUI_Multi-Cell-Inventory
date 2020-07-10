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

public class UI_Quad : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    UI_Slot parentSlot;

    private void Awake()
    {
        parentSlot = GetComponentInParent<UI_Slot>();
    }

    // 마우스 포인터 들어옴
    public void OnPointerEnter(PointerEventData eventData)
    {
        CheckMousePoint();
    }

    // 나감
    public void OnPointerExit(PointerEventData eventData)
    {

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
                //print("1번들어옴");
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

        EventHandler.instance.SetItemSizeOnGrid(v, h, parentSlot);
    }
}
