using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 생성 버튼
public class CreateItemButton : MonoBehaviour
{
    public void OnClick()
    {
        EventHandler.instance.ClickedCreateItemBtn();
    }
}
