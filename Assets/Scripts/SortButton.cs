using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 정렬 버튼
public class SortButton : MonoBehaviour
{
    public void OnClick()
    {
        EventHandler.instance.ClickedSortButton();
    }
}
