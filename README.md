# 디아블로, 패스오브엑자일 스타일의 멀티셀 인벤토리
  
디아블로와 패스오브엑자일에서 볼 수 있는 멀티셀 방식의 인벤토리입니다.  
아이템마다 칸 수가 정해져 있고, 해당하는 칸에 맞추어 아이템이 옮겨지는 방식입니다.  

# 개발환경
* Unity 2019.2.21  
* Visual Studio 2019  

# 기능
* 아이템 생성
* 아이템 정렬
* 아이템 옮기기  
* 아이템 장착
* 아이템 삭제

# 기능 설명
## 1. 아이템 생성

이미지1 | 이미지2 | 이미지3
:-------------------------:|:-------------------------:|:-------------------------:
![아이템생성1](https://user-images.githubusercontent.com/48229283/104796899-94d10200-57fd-11eb-9316-a0ad2fc4a7c8.png) | ![아이템생성2](https://user-images.githubusercontent.com/48229283/104796901-96022f00-57fd-11eb-8b9e-d17ab019a425.png) | ![아이템생성3](https://user-images.githubusercontent.com/48229283/104796902-97335c00-57fd-11eb-8587-024a1d2acc24.png)

아이템 생성 버튼으로 아이템의 카테고리를 선택하여 생성할 수 있습니다.  

## 2. 아이템 정렬

이미지1 | 이미지2
:-------------------------:|:-------------------------:
![아이템정렬1](https://user-images.githubusercontent.com/48229283/104797025-6c95d300-57fe-11eb-8c69-7eb12b7d5c37.png) | ![아이템정렬2](https://user-images.githubusercontent.com/48229283/104797026-6d2e6980-57fe-11eb-9b0d-f21a682fb55e.png)

정렬 버튼으로 정렬할 수 있습니다.

## 3. 아이템 옮기기  

### 이동 가능

이미지1 | 이미지2
:-------------------------:|:-------------------------:
![아이템옮기기1](https://user-images.githubusercontent.com/48229283/104797155-5b999180-57ff-11eb-91e5-66a45c1dfca5.png) | ![아이템옮기기2](https://user-images.githubusercontent.com/48229283/104797157-5c322800-57ff-11eb-9e21-e73092e7d0a5.png)

겹치는 칸이있고 아이템을 놓을 수 있다면 초록색으로 표시됩니다.  
아이템을 놓으면 겹쳐진 아이템은 마우스 커서로 옮겨집니다.  

### 이동 불가능

이미지1 | 이미지2
:-------------------------:|:-------------------------:
![아이템옮기기3](https://user-images.githubusercontent.com/48229283/104797250-fb571f80-57ff-11eb-9141-296d6a6163be.png) | ![아이템옮기기4](https://user-images.githubusercontent.com/48229283/104797252-fbefb600-57ff-11eb-9259-406462e5fc19.png)

두개 이상의 아이템이 겹쳐질 경우 놓을 수 없으므로 칸이 붉은색으로 표시됩니다.  

## 4. 아이템 장착

![아이템장착](https://user-images.githubusercontent.com/48229283/104797311-728cb380-5800-11eb-8e7c-3c106dcf1685.png)

아이템의 타입에 맞는 장비칸에 장착할 수 있습니다.
타입이 다르면 장착할 수 없습니다.

## 5. 아이템 삭제

이미지1 | 이미지2
:-------------------------:|:-------------------------:
![아이템삭제](https://user-images.githubusercontent.com/48229283/104797420-30b03d00-5801-11eb-8093-43b1facef6ce.png) | ![아이템삭제2](https://user-images.githubusercontent.com/48229283/104797419-3017a680-5801-11eb-87f3-33cf9fc4cfaa.png)

오른쪽 하단 쓰레기통으로 아이템을 드랍하면 아이템을 버릴 수 있습니다.

# Code

## 인벤토리 슬롯 매니저 - InvGridManager.cs

인벤토리 매니저 클래스입니다.  
슬롯에 배치된 아이템을 딕셔너리로 관리합니다.  
슬롯의 사용유무, 아이템 배치, 정렬 등의 기능을 수행합니다.  

```cs
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

// 인벤토리 슬롯 관리 매니저
public class InvGridManager : MonoBehaviour
{
    // 생성할 슬롯 프로토타입
    public Transform SlotPrototype;
    public Transform SlotGrid;
    public Transform Items;

    // 슬롯 그리드 사이즈
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
```

```cs
// 슬롯에 아이템 배치하기
    public void SetItemOnSlot()
    {
        // 배치 가능
        if (canPlaceItem)
        {
            GetItemOnSlot(GetSwapCode());

            // 컨테이너 생성, 아이템과 해당 아이템이 올려질 슬롯 좌표저장용
            ItemContainer value = new ItemContainer();

            for (int i = 0; i < SlotOnItem.Count; i++)
            {
                slotScript = SlotOnItem[i].GetComponent<UI_Slot>();
                // 슬롯 사용하는 상태로 바꾸기
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
```

```cs
 // 정렬하기(타입순서대로)
    public void Sort()
    {
        // 슬롯 아이템 코드 클리어
        for (int y = 0; y < GRIDSIZE_Y; y++)
        {
            for (int x = 0; x < GRIDSIZE_X; x++)
            {
                slotScript = SlotArray[y, x].GetComponent<UI_Slot>();
                slotScript.itemCode = null;
                slotScript.m_slotState = SLOT_STATE.notUse;
                slotScript.prevColor = Color.white;
                slotScript.SetColor(slotScript.prevColor);
            }
        }

        Stack<Transform> Items = new Stack<Transform>();
        UI_Item itemScript;

        // 아이템 타입 순서대로 스택에 저장
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

        if (Items.Count == 0)
            return;

        ItemSize size = new ItemSize();
        size.X = 0;
        size.Y = 0;
        int Max = Items.Count;

        SlotOnItem.Clear();
        for (int i = 0; i < Max; i++)
        {
            // 스택에서 아이템 정보 가져오기
            itemScript = Items.Pop().GetComponent<UI_Item>();
            
            bool escapeFlag = false;
            // 그리드 사이즈 체크
            for (int y = 0; y < GRIDSIZE_Y; y++)
            {
                for (int x = 0; x < GRIDSIZE_X; x++)
                {
                    // 비어 있는 슬롯만 검사
                    if (SlotArray[y, x].GetComponent<UI_Slot>().itemCode == null)
                    {
                        // 그리드 사이즈 벗어나는지 확인, 벗어나면 다음 슬롯 검사
                        if (y + itemScript.SIZE.Y - 1 > GRIDSIZE_Y || x + itemScript.SIZE.X - 1 > GRIDSIZE_X)
                            continue;

                        // 사이즈 벗어나지 않으면 배치하는 작업
                        for (int startY = y; startY < y + itemScript.SIZE.Y; startY++)
                        {
                            for (int startX = x; startX < x + itemScript.SIZE.X; startX++)
                            {
                                SlotOnItem.Add(SlotArray[startY, startX]);

                                slotScript = SlotArray[startY, startX].GetComponent<UI_Slot>();
                                slotScript.itemCode = itemScript.itemInfo.code;
                                slotScript.prevColor = Color.white;
                                slotScript.SetColor(Color.blue);
                            }
                        }
                        // 딕셔너리에 저장되있던 아이템의 슬롯 정보를 재배치된 슬롯 정보로 수정
                        ItemDictionary[itemScript.itemInfo.code].slots = new List<Transform>(SlotOnItem);

                        itemScript.gameObject.transform.position = GetPivotPosition();
                        SlotOnItem.Clear();

                        escapeFlag = true;
                        break;
                    }
                }
                if (escapeFlag)
                    break;
            }
        }

    }
```

## 아이템 생성 매니저 - ItemGenerator.cs

아이템 생성 매니저 클래스입니다.  
아이템의 데이터를 불러오고, 데이터를 기반으로 아이템을 생성합니다.  

```cs
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
```

## 장비 슬롯 매니저 - EquipmentManager.cs

장착된 장비를 딕셔너리로 관리하고, 장비의 장착, 교체 기능을 수행합니다.  

```cs
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
```

## 마우스 이벤트 처리 클래스 - EventHandler.cs

마우스 입력에 따른 이벤트를 처리하는 매니저 클래스입니다.  
이벤트가 발생했을 해당 기능을 수행하는 클래스와 연결하는 기능을 수행하고 있습니다.

```cs
// 슬롯 클릭했을때
    public void ClickedSlot(UI_Slot parentSlot)
    {
        // 현재 드래그 중인 아이템이 없는 경우는??
        // 클릭한 슬롯에 아이템이 있는지 검사
        if(SelectedItem == null)
        {
            if (parentSlot.itemCode == null)
                return;

            sc_InvGridManager.GetItemOnSlot(parentSlot.itemCode);
        }
        // 드래그 중인 아이템 있음
        // 슬롯에 배치해야함
        else if(SelectedItem != null)
        {
            sc_InvGridManager.SetItemOnSlot();
        }
    }

    // 아이템 코드 가져오기
    public string GetItemCode()
    {
        if (SelectedItem == null)
            return null;

        return SelectedItem.GetComponent<UI_Item>().GetItemCode();
    }

    // 아이템 타입 가져오기
    public ITEM_TYPE GetItemType()
    {
        if (SelectedItem == null)
            return ITEM_TYPE.Max;

        return SelectedItem.GetComponent<UI_Item>().GetItemType();
    }

    // 장비창 클릭 했을때
    public void ClickedEquipmentSlot(Transform eSlot)
    {
        sc_EquipManager.CheckSlot(eSlot);
    }

    // 아이템 생성버튼 클릭했을때
    public void ClickedCreateItemBtn()
    {
        // 생성 패널 켜기, 끄기
        CreateItemPanel.gameObject.SetActive(!CreateItemPanel.gameObject.activeSelf);
    }

    // 아이템 카테고리 클릭하면 스크롤뷰에 해당 타입의 아이템 목록 보여주기
    public void ClickedViewBtn(ITEM_TYPE type)
    {
        sc_ItemGenerator.CreateItemOnScrollView(type);
    }

    // 아이템 생성하기
    public void CreateItem(ViewItem _item)
    {
        sc_ItemGenerator.CreateItem(_item);
    }

    // 아이템 삭제하기
    public void RemoveItem()
    {
        if(SelectedItem != null)
        {
            sc_ItemGenerator.RemoveItem(SelectedItem);
            Destroy(SelectedItem.gameObject);
            SelectedItem = null;
        }
    }

    // 정렬하기
    public void ClickedSortButton()
    {
        if(SelectedItem == null)
            sc_InvGridManager.Sort();
    }
```

## 슬롯 인터페이스 클래스 - UI_Slot.cs

UI에 표시될 이미지 정보를 가지고 있습니다.  
슬롯의 사용유무 정보를 가지고 있습니다.  
슬롯의 색상 변경 기능을 수행합니다.  


```cs
// 슬롯 상태
public enum SLOT_STATE
{
    notUse, // 사용중 아님
    inUse,  // 사용중
}

public class UI_Slot : MonoBehaviour
{
    public int x, y;
    public SLOT_STATE m_slotState;
    public Image frontImg;
    public Color prevColor;
    public string itemCode;
    public bool isEdge;

    private void Awake()
    {
        itemCode = null;
        prevColor = frontImg.color;
    }

    // 색상 바꾸기
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
```

## 아이템 인터페이스 클래스 - UI_Item.cs

아이템의 데이터를 담을 아이템정보 객체와 아이템 사이즈를 가지고 있습니다.

```cs
// 아이템 사이즈
public class ItemSize
{
    public int X;
    public int Y;
}

// 아이템
public class UI_Item : MonoBehaviour, IPointerClickHandler
{
    // 아이템 데이터
    public Item itemInfo;
    public ItemSize SIZE;
    public bool m_isSelected = false;

    // 아이템 클릭했을때
    public void OnPointerClick(PointerEventData eventData)
    {
        EventHandler.instance.SetSelectedItem(this.transform);
    }

    private void Update()
    {
        // 마우스 포인터 따라다니도록
        if(m_isSelected)
            transform.position = Input.mousePosition;
    }

    public string GetItemCode()
    {
        return itemInfo.code;
    }

    public ITEM_TYPE GetItemType()
    {
        return itemInfo.type;
    }
}
```

## 아이템 정보 클래스 - Item.cs

아이템의 이름과 코드, 타입 정보를 가지고 있는 클래스입니다.

```cs
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
```
