using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTypeComparer : IEqualityComparer<ITEM_TYPE>
{
    bool IEqualityComparer<ITEM_TYPE>.Equals(ITEM_TYPE x, ITEM_TYPE y)
    {
        return (int)x == (int)y;
    }

    int IEqualityComparer<ITEM_TYPE>.GetHashCode(ITEM_TYPE obj)
    {
        return ((int)obj).GetHashCode();
    }

}
