using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 생성에서 카테고리 버튼
public class GenerateBtn : MonoBehaviour
{
    public ITEM_TYPE type;

    public void OnClick()
    {
        EventHandler.instance.ClickedViewBtn(type);
    }
}
