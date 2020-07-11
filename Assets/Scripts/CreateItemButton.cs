using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateItemButton : MonoBehaviour
{
    public void OnClick()
    {
        EventHandler.instance.ClickedCreateItemBtn();
    }
}
