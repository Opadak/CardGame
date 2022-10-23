using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Enemy : MonoBehaviour
{
    [SerializeField] string enemyName;
    [SerializeField] float distance;
    [SerializeField] Sprite typeAbility;
    [SerializeField] SpriteRenderer spriteRenderer;
    Transform playerPos;
    public Animator anime;
    public int health;
    public int curHealth;
    public int damage;
    public float stopX;
    public float distanceX;
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);
    public bool attackable;
    //public bool isNearByPlayer;
    public bool isMove;
    
    void Start()
    {
        health = curHealth;
        playerPos = GameManager.Inst.player.transform;
    }
    void Update()
    {
       
    }
    public void Move()
    {

        StartCoroutine(MoveCo());
    }


    IEnumerator MoveCo()
    {
        Vector2 target = new Vector2(transform.position.x + ((-1) * distance), transform.position.y + 0);
        if (target.x <= stopX)
            target.x = stopX;
        yield return delay07;
        transform.DOMove(target, 0.5f);
    }
    public bool CheckDistance()
    {
        var distancePos = transform.position.x - playerPos.position.x ;
        if (distancePos <= distanceX)
            return true;
        else
            return false;
        //Debug.DrawRay(transform.position, new Vector2(rayX, 0), new Color(0, 1, 0));
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(rayX, 0));
       
        //if (hit.collider != null)
        //{
        //    if (hit.collider.gameObject.tag == "Player")
        //    {
        //        isNearByPlayer = true;

        //    }
        //}
        //else
        //{
        //    isNearByPlayer = false;
        //}
    }
  
    public virtual void Attack()
    {
        anime.SetTrigger("doAttack");
    }
    IEnumerator AttackMotionCO()
    {
        yield return new WaitForSeconds(1f);
    }
 
    IEnumerator DamageCO()
    {
        anime.SetTrigger("doHit");
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = Color.white;
    }
 
    IEnumerator DeadCO()
    {
        anime.SetTrigger("doDie");
        spriteRenderer.color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Effect")
        {
           
         
            Effect effect = collision.gameObject.GetComponent<Effect>();
            health = health - effect.attack;
            StartCoroutine(DisappearEffectCO(collision.gameObject));
            if(health <= 0)
            {
                StartCoroutine(DeadCO());
            }
            else
            {
                StartCoroutine(DamageCO());
            }
           
        }
    }

   IEnumerator DisappearEffectCO(GameObject effectGOB)
    {
        yield return new WaitForSeconds(1f);
        Destroy(effectGOB);
    }
}
