using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    
    bool isCharging;
    [SerializeField] GameObject bulletPrefab;
    void Awake()
    {
        isCharging = false;
    }

    public override void Attack()
    {
        if (!isCharging)
        {
            isCharging = true;
            anime.SetBool("isCharging",true);
            return;
        }
        anime.SetBool("isCharging", false);
        anime.SetTrigger("doAttack");
        Instantiate(bulletPrefab, transform.position,Utils.QI);
        isCharging = false;
    }
   
}
