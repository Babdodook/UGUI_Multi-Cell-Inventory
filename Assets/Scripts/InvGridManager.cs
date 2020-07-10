using System.Collections;
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
                item.GetComponent<UI_Slot>().x = x;
                item.GetComponent<UI_Slot>().y = y;
                item.GetComponentInChildren<Text>().text = y + "," + x;
                item.position = new Vector3(slotOriginPosition.x + x * 30, slotOriginPosition.y - y * 30, 0);

                item.SetParent(SlotGrid);
                item.gameObject.SetActive(true);
                SlotArray[y, x] = item;
            }
        }
    }

    public void SetItemOnGrid(int leftX, int rightX, int downY, int upY)
    {
        CheckGrid(leftX, rightX, downY, upY);
    }

    bool CheckGrid(int leftX, int rightX, int downY, int upY)
    {
        // 인벤토리 그리드 벗어난 경우
        if (leftX < 0 || rightX > GRIDSIZE_X ||
            upY < 0 || downY > GRIDSIZE_Y)
            return false;

        UI_Slot slotScirpt;
        for (int y = 0; y < GRIDSIZE_Y; y++) 
        {
            for (int x = 0; x < GRIDSIZE_X; x++) 
            {
                if ((leftX <= x && x <= rightX) &&
                    (upY <= y && y <= downY))
                {
                    slotScirpt = SlotArray[y, x].GetComponent<UI_Slot>();
                    // 슬롯이 사용중이아니라면..파란색으로 표시
                    if(slotScirpt.m_slotState == SLOT_STATE.notUse)
                    {
                        //print(y + "," + x + " 실행");
                        slotScirpt.SetColor(Color.blue);
                    }
                    // 슬롯 사용중이라면..?
                    else if(slotScirpt.m_slotState == SLOT_STATE.inUse)
                    {

                    }
                }
            }
        }

        return true;
    }
}
