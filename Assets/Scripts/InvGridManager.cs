using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

// 아이템과 아이템을 장착중인 슬롯을 보관할 컨테이너
public class ItemContainer
{
    public Transform item;
    public List<Transform> slots;

    public ItemContainer()
    {
        item = null;
        slots = null;
    }
}

public class InvGridManager : MonoBehaviour
{
    public Transform SlotPrototype;
    public Transform SlotGrid;
    public Transform Items;

    public int GRIDSIZE_X;
    public int GRIDSIZE_Y;
    Vector3 slotOriginPosition;

    // 슬롯배열
    public Transform[,] SlotArray;
    // 아이템 배치검사 중인 슬롯
    public List<Transform> SlotOnItem;
    // 슬롯에 배치된 아이템들
    public Dictionary<string, ItemContainer> ItemDictionary = new Dictionary<string, ItemContainer>();

    // 아이템 스왑용
    Transform tempItem;
    List<Transform> tempSlotList;

    bool canPlaceItem;
    bool canSwapItem;
    bool outOfRange;
    UI_Slot slotScript;

    private void Awake()
    {
        slotOriginPosition = SlotPrototype.position;
        SlotArray = new Transform[GRIDSIZE_Y, GRIDSIZE_X];
        SlotOnItem = new List<Transform>();
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

                if (x == 0 || x == GRIDSIZE_X - 1 ||
                    y == 0 || y == GRIDSIZE_X - 1) 
                {
                    item.GetComponent<UI_Slot>().isEdge = true;
                }
                else
                    item.GetComponent<UI_Slot>().isEdge = false;

                item.SetParent(SlotGrid);
                item.gameObject.SetActive(true);
                SlotArray[y, x] = item;
            }
        }
    }

    public void SetItemOnGrid(int leftX, int rightX, int downY, int upY)
    {
        SlotOnItem.Clear();
        canPlaceItem = false;
        canSwapItem = false;
        outOfRange = false;

        // 아이템 사이즈에 맞는 슬롯들을 찾아서
        FindSlot(leftX, rightX, downY, upY);
        // 슬롯의 상태를 검사한다.
        CheckSlotState();
    }

    // 그리드에서 아이템 사이즈에 맞는 슬롯을 찾는다
    void FindSlot(int leftX, int rightX, int downY, int upY)
    {
        // 인벤토리 그리드 벗어난 경우
        if (leftX < 0 || rightX >= GRIDSIZE_X ||
            upY < 0 || downY >= GRIDSIZE_Y)
        {
            outOfRange = true;
        }
        else
        {
            outOfRange = false;
        }

        for (int y = 0; y < GRIDSIZE_Y; y++) 
        {
            for (int x = 0; x < GRIDSIZE_X; x++) 
            {
                // 리스트에 추가
                if ((leftX <= x && x <= rightX) &&
                    (upY <= y && y <= downY))
                {
                    // 아이템 놓으려는 슬롯들 리스트에 추가
                    SlotOnItem.Add(SlotArray[y, x]);
                }
            }
        }

    }

    // 슬롯의 상태를 검사한다.
    void CheckSlotState()
    {
        // 2개 이상의 아이템이 겹치는지 미리 검사
        Color inUseColor;
        if (isOverlapped())
        {
            inUseColor = Color.red;
            canPlaceItem = false;
            canSwapItem = false;
        }
        else
        {
            inUseColor = Color.green;
            canPlaceItem = true;
        }

        
        for (int i = 0; i < SlotOnItem.Count; i++)
        {
            // 지금 사용중인 색깔 저장해놓음
            slotScript = SlotOnItem[i].GetComponent<UI_Slot>();
            slotScript.prevColor = slotScript.frontImg.color;

            // 인벤토리 그리드 범위 벗어난 경우엔 색깔 표시 안함
            if(outOfRange)
            {
                canPlaceItem = false;
                continue;
            }

            // 슬롯이 사용중이아니라면.. 파란색으로 표시
            if (slotScript.m_slotState == SLOT_STATE.notUse)
            {
                slotScript.SetColor(Color.blue);
            }
            // 슬롯 사용중이라면.. 미리 검사된 색상 표시
            else if (slotScript.m_slotState == SLOT_STATE.inUse)
            {
                slotScript.SetColor(inUseColor);
            }
        }
    }

    // 다른 아이템과 겹치는지 검사
    bool isOverlapped()
    {
        List<Transform> inUseSlots = new List<Transform>();

        for (int i = 0; i < SlotOnItem.Count; i++)
        {
            slotScript = SlotOnItem[i].GetComponent<UI_Slot>();
            // 사용중인 슬롯만 검사하도록 가려냄
            if (slotScript.m_slotState == SLOT_STATE.inUse)
            {
                inUseSlots.Add(SlotOnItem[i]);
            }
        }

        if (inUseSlots.Count == 0)
        {
            canSwapItem = false;
            return false;
        }

        slotScript = inUseSlots[0].GetComponent<UI_Slot>();
        string itemCode = slotScript.itemCode;
        for (int i = 0; i < inUseSlots.Count; i++)
        {
            slotScript = inUseSlots[i].GetComponent<UI_Slot>();
            // 아이템 코드가 다른 경우 최소 2개이상의 아이템이 겹치는 상태
            // 아이템 놓을 수 없음
            if (slotScript.itemCode != itemCode)
                return true;
        }

        // 놓을수 있음
        canSwapItem = true;
        return false;
    }

    // 이전 색상으로 되돌리기
    public void ChangeColorToPrevColor()
    {
        for (int i = 0; i < SlotOnItem.Count; i++) 
        {
            slotScript = SlotOnItem[i].GetComponent<UI_Slot>();
            slotScript.frontImg.color = slotScript.prevColor;
        }

        // 리스트 초기화
        SlotOnItem.Clear();
    }

    string GetSwapCode()
    {
        for (int i = 0; i < SlotOnItem.Count; i++) 
        {
            if (SlotOnItem[i].GetComponent<UI_Slot>().itemCode != null)
            {
                return SlotOnItem[i].GetComponent<UI_Slot>().itemCode;
            }
        }

        return null;
    }


    // 슬롯에 아이템 배치하기
    public void SetItemOnSlot()
    {
        // 배치 가능
        if(canPlaceItem)
        {            
            GetItemOnSlot(GetSwapCode());

            // 컨테이너 생성
            ItemContainer value = new ItemContainer();

            for (int i = 0; i < SlotOnItem.Count; i++) 
            {
                slotScript = SlotOnItem[i].GetComponent<UI_Slot>();
                slotScript.SetInUse(EventHandler.instance.GetItemCode());
            }

            // 아이템과 슬롯정보 저장
            value.item = EventHandler.instance.SelectedItem;
            value.slots = new List<Transform>(SlotOnItem);
            string key = EventHandler.instance.GetItemCode();
            // 딕셔너리에 아이템 추가
            ItemDictionary.Add(key, value);
            EventHandler.instance.SelectedItem.position = GetPivotPosition();
            EventHandler.instance.SelectedItem.SetParent(Items);
            EventHandler.instance.SetSelectedItem(null);

            if (canSwapItem)
            {
                EventHandler.instance.SetSelectedItem(tempItem);
                SlotOnItem = new List<Transform>(tempSlotList);
            }
        }
    }

    // 슬롯에 있는 아이템 마우스로 가져오기
    public void GetItemOnSlot(string itemCode)
    {
        if (itemCode == null)
            return;

        if (EventHandler.instance.SelectedItem == null)
            canSwapItem = false;

        // 아이템 가져옴
        ItemContainer container = ItemDictionary[itemCode];
        if (canSwapItem)
            tempItem = container.item;
        else
            EventHandler.instance.SetSelectedItem(container.item);
        
        for (int i = 0; i < container.slots.Count; i++)
        {
            slotScript = container.slots[i].GetComponent<UI_Slot>();
            slotScript.SetNotUse();
        }

        if(canSwapItem)
            tempSlotList = new List<Transform>(container.slots);
        else
            SlotOnItem = new List<Transform>(container.slots);

        // 아이템 딕셔너리에서 제거
        ItemDictionary.Remove(itemCode);
    }

    // 아이템을 놓으려는 슬롯들의 중앙 좌표를 구한다
    Vector3 GetPivotPosition()
    {
        Vector3 StartPosition = SlotOnItem[0].position;
        Vector3 EndPosition = SlotOnItem[SlotOnItem.Count - 1].position;

        Vector3 DesiredPosition =
            new Vector3( StartPosition.x + (EndPosition.x - StartPosition.x) / 2,
                         StartPosition.y + (EndPosition.y - StartPosition.y) / 2, 0);

        return DesiredPosition;
    }

    // 정렬하기(타입순서대로)
    public void Sort()
    {
        SlotOnItem.Clear();

        // 슬롯 아이템 코드 클리어
        for (int y = 0; y < GRIDSIZE_Y; y++) 
        {
            for (int x = 0; x < GRIDSIZE_X; x++)
            {
                slotScript = SlotArray[y, x].GetComponent<UI_Slot>();
                slotScript.itemCode = null;
            }
        }

        Stack<Transform> Items = new Stack<Transform>();
        UI_Item itemScript;

        

        for (int i = 0; i < (int)ITEM_TYPE.Max; i++) 
        {
            // 아이템 딕셔너리에서 찾아봄
            foreach (KeyValuePair<string, ItemContainer> items in ItemDictionary)
            {
                itemScript = items.Value.item.GetComponent<UI_Item>();
                // 찾는 타입이 맞으면
                if (itemScript.itemInfo.type == (ITEM_TYPE)i)
                {
                    // 스택에 쌓아둠
                    Items.Push(items.Value.item);
                }
            }
        }

        ItemSize size = new ItemSize();
        size.X = 0;
        size.Y = 0;
        for (int j = 0; j < Items.Count; j++)
        {
            itemScript = Items.Pop().GetComponent<UI_Item>();

            // 그리드 사이즈 체크
            if( size.X + itemScript.SIZE.X-1 < GRIDSIZE_X &&
                size.Y + itemScript.SIZE.Y-1 < GRIDSIZE_Y)
            {
                for (int startY = size.Y; startY < size.Y + itemScript.SIZE.Y - 1; startY++) 
                {

                }
            }
        }
    }
}
