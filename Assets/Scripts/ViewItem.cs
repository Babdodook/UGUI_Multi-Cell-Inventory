using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ViewItem : MonoBehaviour, IPointerClickHandler
{
    public Image sprite;
    public Text t_name;
    public Text t_type;
    public Text t_size;
    public int sizeX;
    public int sizeY;

    public void SetInfo(string _sprite, string _name, string _type, int _sizeX, int _sizeY)
    {
        sprite.sprite = Resources.Load<Sprite>(_sprite);
        t_name.text = _name;
        t_type.text = _type;
        sizeX = _sizeX;
        sizeY = _sizeY;
        t_size.text = _sizeX + "X" + _sizeY;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(EventHandler.instance.SelectedItem == null)
            EventHandler.instance.CreateItem(this);
    }
}
