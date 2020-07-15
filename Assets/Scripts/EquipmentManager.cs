using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 장비창 매니저
public class EquipmentManager : MonoBehaviour
{
    // 장착한 아이템들
    Dictionary<string, Transform> EquipmentDictionary = new Dictionary<string, Transform>();
    // 장비 슬롯들
    Dictionary<ITEM_TYPE, Transform> EquipPanelDictionary = new Dictionary<ITEM_TYPE, Transform>(new ItemTypeComparer());
    public Transform Equipment;

    public void Awake()
    {
        UI_EquipmentSlot[] child = Equipment.GetComponentsInChildren<UI_EquipmentSlot>();

        for (int i = 0; i < child.Length; i++)
        {
            ITEM_TYPE key = child[i].GetComponent<UI_EquipmentSlot>().type;
            Transform value = child[i].transform;
            EquipPanelDictionary.Add(key, value);

        }
    }

    // 장비창 아이템 장착 중인지 검사
    public void CheckSlot(Transform eSlot)
    {
        UI_EquipmentSlot slotScript = eSlot.GetComponent<UI_EquipmentSlot>();

        
        // 이미 아이템 장착중
        if(slotScript.isEquiped)
        {
            // 아이템 드래그 중일때
            if(EventHandler.instance.SelectedItem != null)
            {
                // 타입이 동일하다면, 스왑해야한다.
                if(slotScript.type == EventHandler.instance.GetItemType())
                {
                    SwapItem(slotScript, slotScript.itemCode);
                }
                else if(slotScript.type == ITEM_TYPE.Max)
                {
                    // 빈슬롯
                    SetItem(slotScript);
                }
            }
            // 드래그중 아닐때
            else
            {
                // 빈슬롯 아님
                if(slotScript.type != ITEM_TYPE.Max)
                {
                    GetItem(slotScript, slotScript.itemCode);
                }
            }
        }
        // 아이템 장착중 아님
        else
        {
            // 아이템 드래그 중일때
            if(EventHandler.instance.SelectedItem != null)
            {
                if(slotScript.type == EventHandler.instance.GetItemType())
                    SetItem(slotScript);
            }
        }
    }

    // 장착중인 아이템과 드래그 중인 아이템 교체
    void SwapItem(UI_EquipmentSlot slotScript, string itemCode)
    {
        // 현재 장착중인 아이템 가져오고 딕셔너리에서 제거
        Transform tempItem = EquipmentDictionary[itemCode];
        EquipmentDictionary.Remove(itemCode);

        // 슬롯에 드래그 중인 아이템 코드 세팅
        slotScript.itemCode = EventHandler.instance.GetItemCode();
        slotScript.isEquiped = true;

        // 딕셔너리에 드래그 중인 아이템 추가
        string key = slotScript.itemCode;
        Transform value = EventHandler.instance.SelectedItem;
        EquipmentDictionary.Add(key, value);

        // 현재 드래그 중인 아이템의 위치는 슬롯의 위치로
        // 장착중이던 아이템은 드래그로 변경
        EventHandler.instance.SelectedItem.position = slotScript.transform.position;
        EventHandler.instance.SelectedItem.SetParent(EquipPanelDictionary[EventHandler.instance.GetItemType()]);
        EventHandler.instance.SetSelectedItem(null);
        EventHandler.instance.SetSelectedItem(tempItem);
    }

    // 장비슬롯에 아이템 마우스로 옮기기
    void GetItem(UI_EquipmentSlot slotScript, string itemCode)
    {
        // 장착중인 아이템 가져오고 딕셔너리에서 지우기
        Transform tempItem = EquipmentDictionary[itemCode];
        EquipmentDictionary.Remove(itemCode);

        // 장비 슬롯 초기화
        slotScript.itemCode = null;
        slotScript.isEquiped = false;

        // 드래그로 변경
        EventHandler.instance.SetSelectedItem(tempItem);
    }

    // 장비 슬롯에 아이템 놓기
    void SetItem(UI_EquipmentSlot slotScript)
    {
        // 슬롯에 아이템 코드 세팅, 장착중으로 변경
        slotScript.itemCode = EventHandler.instance.GetItemCode();
        slotScript.isEquiped = true;

        // 딕셔너리에 드래그 중인 아이템 추가
        string key = slotScript.itemCode;
        Transform value = EventHandler.instance.SelectedItem;
        EquipmentDictionary.Add(key, value);

        EventHandler.instance.SelectedItem.position = slotScript.transform.position;
        EventHandler.instance.SelectedItem.SetParent(EquipPanelDictionary[EventHandler.instance.GetItemType()]);
        EventHandler.instance.SetSelectedItem(null);
    }
}
