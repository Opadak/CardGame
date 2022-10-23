using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EntityManager : MonoBehaviour
{
    public static EntityManager Inst { get; private set; }
    void Awake() => Inst = this;


    [SerializeField] GameObject entityPrefab;
    [SerializeField] List<Entity> otherEntities;
    [SerializeField] Entity myEmptyEntity;
    [SerializeField] Transform EntityMovePos;
    [SerializeField] Transform EffectAppearPos;
    public List<Entity> myEntities;
    public GameObject[] effects;
    public Card SelectCard;
    

    public int MAX_ENTITY_COUNT = 1; //맥스 카운트 
    public bool IsFullMyEntities => myEntities.Count >= MAX_ENTITY_COUNT && !ExistMyEmptyEntity;
    bool ExistMyEmptyEntity => myEntities.Exists(x => x == myEmptyEntity);
    int MyEmptyEntityIndex => myEntities.FindIndex(x => x == myEmptyEntity);
    public int myPutCount;
    public int bulletisActive;
    Player playerSC;

    void EntityAligment(bool isMine)
    {
        float targety = 0f;
        var targetEntities = myEntities;
        for (int i = 0; i < targetEntities.Count; i++)
        {
            float targetX = (targetEntities.Count - 1) * -2f + i * 4f;

            var targetEntity = targetEntities[i];
            targetEntity.originPos = new Vector3(targetX, targety, 0);
            targetEntity.MoveTransform(targetEntity.originPos, true, 0.5f);
            targetEntity.GetComponent<Order>()?.SetOriginOrder(i);
        }
    }

    public void InsertMyEmptyEntity(float xPos)
    {
        if (IsFullMyEntities)
            return;
        if (!ExistMyEmptyEntity)
            myEntities.Add(myEmptyEntity);

        Vector3 emptyEntityPos = myEmptyEntity.transform.position;
        emptyEntityPos.x = xPos;
        myEmptyEntity.transform.position = emptyEntityPos;

        int _emptyEntityIndex = MyEmptyEntityIndex;
        myEntities.Sort((entity1, entity2) => entity1.transform.position.x.CompareTo(entity2.transform.position.x));
        if (MyEmptyEntityIndex != _emptyEntityIndex)
            EntityAligment(true);
    }
    public void ReMoveMyEmptyEntity()
    {
        if (!ExistMyEmptyEntity)
            return;
        myEntities.RemoveAt(MyEmptyEntityIndex);
        EntityAligment(true);
    }
   

    public bool SpawnEntity(bool isMine, BasicCardItem item, Vector3 spawnPos)
    {
        if (isMine)
        {
            if (IsFullMyEntities || !ExistMyEmptyEntity)
                return false;
        }
        else
        {
            if (IsFullMyEntities)
                return false;
        }

        var entityObject = Instantiate(entityPrefab, spawnPos, Utils.QI);
        var entity = entityObject.GetComponent<Entity>();
        if (isMine)
            myEntities[MyEmptyEntityIndex] = entity;
        else
            //otherEntities.Insert(Random.Range(0, otherEntities.Count), entity);

        entity.isMine = isMine;
        entity.Setup(item);
        EntityAligment(isMine);

       

        return true;
    }

    public void BehaviorPlayer()
    {
       
        StartCoroutine(MoveEntitiesCo());


    }
    
   public IEnumerator MoveEntitiesCo()
    {

        yield return StartCoroutine(MoveEntitiesSubCo());
         
    }

    IEnumerator MoveEntitiesSubCo()
    {
        for(int i = 0; i < myPutCount; i++)
        {
            myEntities[i].MoveTransform(EntityMovePos.position, true, 2f);
            yield return new WaitForSeconds(1f);
        }

        for (int i = 0; i < myPutCount; i++)
        {
            playerSC = GameManager.Inst.player.GetComponent<Player>();
            playerSC.Attack();
            GameObject effect = Instantiate(myEntities[i].effectPrefab, EffectAppearPos.transform.position, Utils.QI);
            Effect effectSc = effect.GetComponent<Effect>();
            effectSc.SetUp(myEntities[i]);
            yield return new WaitForSeconds(1f);
        }

        
    }
}
