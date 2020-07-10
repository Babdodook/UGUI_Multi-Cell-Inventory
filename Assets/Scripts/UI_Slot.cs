using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SLOT_STATE
{
    notUse,
    inUse,
}

public class UI_Slot : MonoBehaviour
{
    public int x, y;
    public SLOT_STATE m_slotState;
    public Image frontImg;
    public Color prevColor;
    public string itemCode;

    private void Awake()
    {
        itemCode = null;
        prevColor = frontImg.color;
    }

    public void SetColor(Color _color)
    {
        frontImg.color = _color;
        Color alpha = frontImg.color;
        alpha.a = 0.3f;
        frontImg.color = alpha;
    }

    public void SetNotUse()
    {
        // 사용하지 않는 상태로 바꾸기
        m_slotState = SLOT_STATE.notUse;

        // 색상 하얀색
        SetColor(Color.white);
        prevColor = frontImg.color;
        
        // 아이템 코드 초기화
        itemCode = null;
    }

    public void SetInUse(string _itemCode)
    {
        // 슬롯 사용중으로 바꾸기
        m_slotState = SLOT_STATE.inUse;

        // 색상 파란색
        SetColor(Color.blue);
        prevColor = frontImg.color;
        
        itemCode = _itemCode;
    }
}
