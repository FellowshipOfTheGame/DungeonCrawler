using UnityEngine;

public abstract class Item:ScriptableObject{
    public Sprite itemIcon;
    public string itemName;
    
    public uint itemRank;
    public int itemPrice;
}