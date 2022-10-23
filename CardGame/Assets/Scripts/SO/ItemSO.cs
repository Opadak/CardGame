using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item
{
    public string name;
    public int index;
    public string information;
    public Sprite itemImg;
    public enum Type {Hp,MaxHp,Defence,Attack,Avoidance,Critical,CardDrawCount,ChooseCount,longDistanceAvoidance };
    public Type itemType;
    public int stat;
   
}
[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Object/ItemSO")]
public class ItemSO : ScriptableObject
{
    public Item[] playerItems;
}
