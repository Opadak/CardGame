using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyBullet : MonoBehaviour
{
    public int damage;
    public float distance;
    void Start()
    {
        Vector2 target = new Vector2(transform.position.x + ((-1) * distance), transform.position.y + 0);
        transform.DOMove(target, 3f);
    }

     
}
