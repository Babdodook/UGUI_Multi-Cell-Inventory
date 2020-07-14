using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortButton : MonoBehaviour
{
    public void OnClick()
    {
        EventHandler.instance.ClickedSortButton();
    }
}
