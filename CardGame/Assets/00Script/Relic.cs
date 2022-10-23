using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Relic : MonoBehaviour
{
    [SerializeField] SpriteRenderer relicSprite;
    public Item relicItem;
    public string name;
    public int index;
    public string information;
    public Sprite itemImg;
    public string type;
    public int stat;
    public Vector3 originPos;

    public void SetUp(Item RelicItem)
    {
        this.relicItem = RelicItem;
        relicSprite.sprite = RelicItem.itemImg;
        name = this.relicItem.name;
        index = this.relicItem.index;
        information = this.relicItem.information;
        itemImg = this.relicItem.itemImg;
        type = this.relicItem.itemType.ToString();
        stat = this.relicItem.stat;
    }

    public void MoveTransform(Vector3 pos, bool useDotween, float dotweenTiem = 0)
    {
        if (useDotween)
            transform.DOMove(pos, dotweenTiem);
        else
            transform.position = pos;
    }
}
