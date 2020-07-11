using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBtn : MonoBehaviour
{
    public ITEM_TYPE type;

    public void OnClick()
    {
        EventHandler.instance.ClickedViewBtn(type);
    }
}
