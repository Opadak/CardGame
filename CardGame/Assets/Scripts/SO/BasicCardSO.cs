using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasicCardItem
{
    public string name;
    public int damage;
    public int distance;
    public float speed;
    public string type;
    public Sprite AbilitySprite;
    public Sprite AbilityTypeSprite;
    public float percent;
    public GameObject effectPrefab;
}

[CreateAssetMenu(fileName = "BasicCardSO", menuName ="Scriptable Object/BasicCardSO")]
public class BasicCardSO : ScriptableObject
{
    public BasicCardItem[] items;
}
