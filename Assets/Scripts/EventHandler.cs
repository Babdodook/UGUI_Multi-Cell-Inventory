using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            SelectedItem = item;
        else
            SelectedItem = null;
    }

    public void CheckSlotGrid(Vert v, Hor h)
    {
        UI_Item item = SelectedItem.GetComponent<UI_Item>();

        int x = item.SIZE.X;
        int y = item.SIZE.Y;

        int max_xValue;
        int min_xValue;
        int max_yValue;
        int min_yValue;

        // 짝
        if (x % 2 == 0)
        {
            max_xValue = x / 2;
            min_xValue = x - 1 / 2;
        }
        // 홀
        else
        {
            max_xValue = min_xValue = x / 2;
        }

        // 짝
        if (y % 2 == 0)
        {
            max_yValue = y / 2;
            min_yValue = y - 1 / 2;
        }
        // 홀
        else
        {
            max_yValue = min_yValue = y / 2;
        }

        switch (v)
        {
            case Vert.UP:

                break;
            case Vert.DOWN:

                break;
        }

        switch (h)
        {
            case Hor.RIGHT:

                break;
            case Hor.LEFT:

                break;
        }


    }
}
