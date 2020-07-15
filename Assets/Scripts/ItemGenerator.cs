using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;

// 제이슨 데이터 파싱하기 위한 클래스
class ViewItemInfo
{
    public string Sprite;
    public string Name;
    public string Type;
    public int SizeX;
    public int SizeY;

    public ViewItemInfo(string sprite, string name, string type, int sizeX, int sizeY)
    {
        Sprite = sprite;
        Name = name;
        Type = type;
        SizeX = sizeX;
        SizeY = sizeY;
    }
}

// 아이템 생성 매니저
public class ItemGenerator : MonoBehaviour
{
    public Transform itemPrototype;
    public Transform ScrollViewItemPrototype;
    public Transform ScrollContent;

    Vector3 originPos;

    // 데이터 불러오기용
    List<ViewItemInfo> HeadList = new List<ViewItemInfo>();
    List<ViewItemInfo> BodyList = new List<ViewItemInfo>();
    List<ViewItemInfo> WeaponList = new List<ViewItemInfo>();
    List<ViewItemInfo> WeaponSubList = new List<ViewItemInfo>();
    List<ViewItemInfo> BeltList = new List<ViewItemInfo>();
    List<ViewItemInfo> BottomsList = new List<ViewItemInfo>();
    List<ViewItemInfo> FootwearList = new List<ViewItemInfo>();

    // 아이템 생성용
    Dictionary<string, Transform> CreatedItemsDictionary = new Dictionary<string, Transform>();

    private void Awake()
    {
        originPos = ScrollViewItemPrototype.GetComponent<RectTransform>().anchoredPosition;

        itemPrototype.gameObject.SetActive(false);
        ScrollViewItemPrototype.gameObject.SetActive(false);

        // 아이템 데이터 불러오기
        LoadItemData();
    }

    // 카테고리 선택할시 보여줄 아이템 목록
    List<ViewItemInfo> GetCurrentList(ITEM_TYPE type)
    {
        List<ViewItemInfo> tempList = null;
        switch(type)
        {
            case ITEM_TYPE.Head:
                tempList = HeadList;
                break;
            case ITEM_TYPE.Body:
                tempList = BodyList;
                break;
            case ITEM_TYPE.Weapon:
                tempList = WeaponList;
                break;
            case ITEM_TYPE.WeaponSub:
                tempList = WeaponSubList;
                break;
            case ITEM_TYPE.Belt:
                tempList = BeltList;
                break;
            case ITEM_TYPE.Bottoms:
                tempList = BottomsList;
                break;
            case ITEM_TYPE.Footwear:
                tempList = FootwearList;
                break;
        }

        return tempList;
    }

    // 스크롤에 표시할 아이템 만들기
    public void CreateItemOnScrollView(ITEM_TYPE type)
    {
        // 아이템 타입 리스트 가져오기
        List<ViewItemInfo> infoList = GetCurrentList(type);

        ViewItem[] tempArray = ScrollContent.GetComponentsInChildren<ViewItem>();

        for (int i = 0; i < tempArray.Length; i++)
        {
            if (tempArray[i] != ScrollViewItemPrototype.GetComponent<ViewItem>())
                Destroy(tempArray[i].gameObject);
        }

        for (int i = 0; i < infoList.Count; i++) 
        {
            var item = Instantiate(ScrollViewItemPrototype);

            // 아이템 정보 세팅
            item.GetComponent<ViewItem>().SetInfo(  infoList[i].Sprite,
                                                    infoList[i].Name,
                                                    infoList[i].Type,
                                                    infoList[i].SizeX,
                                                    infoList[i].SizeY );

            item.SetParent(ScrollContent);
            item.gameObject.SetActive(true);
        }
    }

    // 스크롤에 보여줄 아이템 데이터 불러오기
    void LoadItemData()
    {
        string Head_data = File.ReadAllText(Application.streamingAssetsPath + "/HeadItemInfo.json");
        string Body_data = File.ReadAllText(Application.streamingAssetsPath + "/BodyItemInfo.json");
        string Weapon_data = File.ReadAllText(Application.streamingAssetsPath + "/WeaponItemInfo.json");
        string WeaponSub_data = File.ReadAllText(Application.streamingAssetsPath + "/WeaponSubItemInfo.json");
        string Belt_data = File.ReadAllText(Application.streamingAssetsPath + "/BeltItemInfo.json");
        string Bottoms_data = File.ReadAllText(Application.streamingAssetsPath + "/BottomsItemInfo.json");
        string Footwear_data = File.ReadAllText(Application.streamingAssetsPath + "/FootwearItemInfo.json");

        HeadList = JsonConvert.DeserializeObject<List<ViewItemInfo>>(Head_data);
        BodyList = JsonConvert.DeserializeObject<List<ViewItemInfo>>(Body_data);
        WeaponList = JsonConvert.DeserializeObject<List<ViewItemInfo>>(Weapon_data);
        WeaponSubList = JsonConvert.DeserializeObject<List<ViewItemInfo>>(WeaponSub_data);
        BeltList = JsonConvert.DeserializeObject<List<ViewItemInfo>>(Belt_data);
        BottomsList = JsonConvert.DeserializeObject<List<ViewItemInfo>>(Bottoms_data);
        FootwearList = JsonConvert.DeserializeObject<List<ViewItemInfo>>(Footwear_data);
    }

    // 아이템 생성하기
    public void CreateItem(ViewItem _item)
    {
        var item = Instantiate(itemPrototype);
        item.position = itemPrototype.position;
        item.GetComponent<RectTransform>().sizeDelta = new Vector2(30 * _item.sizeX, 30 * _item.sizeY);
        item.GetComponent<Image>().sprite = _item.sprite.sprite;
        UI_Item itemScript = item.GetComponent<UI_Item>();
        itemScript.itemInfo = new Item();
        itemScript.SIZE = new ItemSize();
        itemScript.itemInfo.name = _item.t_name.text;
        itemScript.itemInfo.type = (ITEM_TYPE)Enum.Parse(typeof(ITEM_TYPE), _item.t_type.text);
        itemScript.SIZE.X = _item.sizeX;
        itemScript.SIZE.Y = _item.sizeY;

        // 아이템 코드 랜덤 생성
        string _code;
        while (true)
        {
            _code = UnityEngine.Random.Range(0, 1000).ToString();
            // 이미 있는 코드인지 확인, 코드 겹치지 않도록
            if(!CreatedItemsDictionary.ContainsKey(_code))
            {
                break;
            }
        }
        itemScript.itemInfo.code = _code;

        string key = itemScript.itemInfo.code;
        Transform value = item;
        // 딕셔너리에 아이템 추가
        CreatedItemsDictionary.Add(key, value);

        item.gameObject.SetActive(true);
        EventHandler.instance.SetSelectedItem(item);
    }

    // 아이템 제거하기
    public void RemoveItem(Transform item)
    {
        string key = item.GetComponent<UI_Item>().GetItemCode();
        CreatedItemsDictionary.Remove(key);
    }
}
