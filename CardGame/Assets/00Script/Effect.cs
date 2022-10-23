using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Effect : MonoBehaviour
{
    public bool isStorm;
    public bool haveHitMotion;
    public int attack;
    public int distance;
    public float speed;
    public string name;
    
    float effectActiveTime;
    Animator anime;
    Vector2 target;
    public void SetUp(Entity entity)
    {
        attack = entity.attack;
        distance = entity.distance;
        name = entity.name;
        speed = entity.speed;

    }

    void Start()
    {
     
            MoveEffect();
        if (haveHitMotion)
            anime = GetComponent<Animator>();
      
    }
    void Update()
    {
          if(transform.position.x == target.x)
        {
            StartCoroutine(MoveEffectCo());
        }
    }

    void MoveEffect()
    {
        target = new Vector2(transform.position.x +distance, transform.position.y + 0);
        
        transform.DOMove(target, speed);
        if (isStorm|| transform.position.x >= target.x)
            ExecuteEffectStorm();
    }
    void ExecuteEffectStorm()
    {
       GameObject stormGO = transform.GetChild(0).gameObject;
       stormGO.SetActive(true);


    }
    IEnumerator MoveEffectCo()
    {
        if (isStorm)
        {
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
    void SetEffectTime()
    {
        effectActiveTime += Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(DestroyEffectCo());
        }
       
    }

    IEnumerator DestroyEffectCo()
    {
       
        if (haveHitMotion)
        {
            anime.SetTrigger("doHit");
            yield return new WaitForSeconds(0.4f);
        }
        transform.DOKill();
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
