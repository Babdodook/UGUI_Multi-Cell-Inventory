using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 타입
public enum ITEM_TYPE
{
    Head,
    Body,
    Weapon,
    WeaponSub,
    Belt,
    Bottoms,
    Footwear,

    Max
}

public class Item
{
    public string name;
    public string code;
    public ITEM_TYPE type;
}
