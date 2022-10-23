using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator anime;
    public GameObject playerHp;
    Animator Txtanimator;
    public bool isAvoidence;
    public bool isLongDistanceAvoidance;
    public GameObject miss;
    public GameObject damageText;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();
        Txtanimator = playerHp.GetComponent<Animator>();
    }
    IEnumerator DamageCO()
    {
        damageText.SetActive(true);
        Txtanimator.SetTrigger("doDamage");
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
        damageText.SetActive(false);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" )
        {
            Damage();
        }
        else if(collision.gameObject.tag == "EnemyBullet")
        {

            LongDamage();
           
            Destroy(collision.gameObject);
        }
    }
    public void Damage()
    {
        int avoidance = 2;
        if (isAvoidence)
            avoidance += 3;
        int R = Random.Range(0, avoidance);
        if (R < avoidance - 1)
        {
            StartCoroutine(MissCo());
            return;
        }
        else
        {
            GameManager.Inst.health = GameManager.Inst.health - 1;
            GameManager.Inst.HpText.text = GameManager.Inst.health.ToString();
            StartCoroutine(DamageCO());
        }
    }
    IEnumerator MissCo()
    {
        miss.SetActive(true);
        yield return new WaitForSeconds(1f);
        miss.SetActive(false);
    }
    public void LongDamage()
    {
        if (!isLongDistanceAvoidance)
        {
            GameManager.Inst.health = GameManager.Inst.health - 1;
            GameManager.Inst.HpText.text = GameManager.Inst.health.ToString();
            StartCoroutine(DamageCO());
            return;
        }
        int result = Random.Range(0, 2);
        if(result == 0)
        {
            GameManager.Inst.health = GameManager.Inst.health - 1;
            GameManager.Inst.HpText.text = GameManager.Inst.health.ToString();
            StartCoroutine(DamageCO());
        }
        else
        {
            StartCoroutine(MissCo());
        }

    }
    public void Attack()
    {
        anime.SetTrigger("doAttack");
    }
  
}
