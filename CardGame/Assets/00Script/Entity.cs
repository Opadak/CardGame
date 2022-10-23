using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class Entity : MonoBehaviour
{
     public BasicCardItem item;
    [SerializeField] SpriteRenderer type;
    [SerializeField] SpriteRenderer effect;
    [SerializeField] TMP_Text damageTMP;
    [SerializeField] TMP_Text distanceTMP;

    public int attack;
    public int distance;
    public string name;
    public bool isMine;
    public bool isBossOrEmpty;
    public float speed;
    public Vector3 originPos;
    public GameObject effectPrefab;
    public void Setup(BasicCardItem item)
    {
        attack = item.damage;
        distance = item.distance;
        this.item = item;
        name = item.name;
        type.sprite = this.item.AbilityTypeSprite;
        effect.sprite = this.item.AbilitySprite;
        damageTMP.text = attack.ToString();
        distanceTMP.text = distance.ToString();
        effectPrefab = this.item.effectPrefab;
        speed = this.item.speed;

    }

 public void MoveTransform(Vector3 pos, bool useDotween, float dotweenTiem = 0)
    {
        
        if (useDotween)
            transform.DOMove(pos, dotweenTiem);
        else
            transform.position = pos;
    }
}
