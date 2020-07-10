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
    public int x;
    public int y;
    public SLOT_STATE m_slotState;
    public Image frontImg;
    public Color prevColor;

    public void SetColor(Color _color)
    {
        frontImg.color = _color;
        Color alpha = frontImg.color;
        alpha.a = 0.3f;
        frontImg.color = alpha;
    }
}
