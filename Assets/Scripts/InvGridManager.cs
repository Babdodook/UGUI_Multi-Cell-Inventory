﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class InvGridManager : MonoBehaviour
{
    public Transform SlotPrototype;
    public Transform SlotGrid;

    int GRIDSIZE_X = 10;
    int GRIDSIZE_Y = 10;
    Vector3 slotOriginPosition;

    // 슬롯배열
    public Transform[,] SlotArray;

    private void Awake()
    {
        slotOriginPosition = SlotPrototype.position;
        SlotArray = new Transform[GRIDSIZE_Y, GRIDSIZE_X];
        CreateInvGrid();
    }

    // 인벤토리 슬롯 생성
    void CreateInvGrid()
    {
        SlotPrototype.gameObject.SetActive(false);

        for (int y = 0; y < GRIDSIZE_Y; y++)
        {
            for (int x = 0; x < GRIDSIZE_X; x++)
            {
                var item = Instantiate(SlotPrototype);
                item.name = "Slot_" + y + "_" + x;
                item.GetComponentInChildren<Text>().text = y + "," + x;
                item.position = new Vector3(slotOriginPosition.x + x * 30, slotOriginPosition.y - y * 30, 0);

                item.SetParent(SlotGrid);
                item.gameObject.SetActive(true);
                SlotArray[y, x] = item;
            }
        }
    }
}