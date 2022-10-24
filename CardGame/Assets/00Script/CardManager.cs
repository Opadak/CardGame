using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CardManager : MonoBehaviour
{
    public static CardManager Inst { get;  set; }
    void AWake() => Inst = this;

    [SerializeField] BasicCardSO basicCardSO;
    [SerializeField] GameObject cardprefab;
    
    [SerializeField] Transform cardSqawnPoint;
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;

    public List<Card> myCards;
   

    List<BasicCardItem> basicCardItemBuffer;
    Card selectCard;
    
    bool isMine;
    public bool isClick;
    public bool attackable;
    

    public void ClickBtn()
    {
        
        //BasicCardItem choiceCard =  PoPBasicCard();
        AddCard(isMine);
      
    }

    public BasicCardItem PoPBasicCard()
    {
        if(basicCardItemBuffer.Count == 0)
        {
            SetupBasicCardBuffer();
        }

        BasicCardItem basicCardItem = basicCardItemBuffer[0];
        basicCardItemBuffer.RemoveAt(0);
        return basicCardItem;
    }


    void SetupBasicCardBuffer()
    {
        basicCardItemBuffer = new List<BasicCardItem>(100);
        for(int i = 0; i < basicCardSO.items.Length; i++)
        {
            BasicCardItem bcItem = basicCardSO.items[i];
            for (int j = 0; j < bcItem.percent; j++)
                basicCardItemBuffer.Add(bcItem);
        }

        for(int i = 0; i < basicCardItemBuffer.Count; i++)
        {
            int rand = Random.Range(i, basicCardItemBuffer.Count);
            BasicCardItem temp = basicCardItemBuffer[i];
            basicCardItemBuffer[i] = basicCardItemBuffer[rand];
            basicCardItemBuffer[rand] = temp;
        }
    }

   void Start()
    {
        SetupBasicCardBuffer();
        TurnManager.OnAddCard += AddCard;
        TurnManager.OnTurnStarted += OnTurnStarted;
    }
    void OnDestroy()
    {
        TurnManager.OnAddCard -= AddCard;
        TurnManager.OnTurnStarted -= OnTurnStarted;
    }
    void Update()
    {
        GetObj();
        if (isClick)
        {
            TryPutCard(true);
        }

    }
   
    void OnTurnStarted(bool myTurn)
    {
        if (myTurn)
            EntityManager.Inst.myPutCount = 0;
    }

    void AddCard(bool isMine)
    {
        var cardObject = Instantiate(cardprefab, cardSqawnPoint.position, Utils.QI);
        var card = cardObject.GetComponent<Card>();
        card.SetUp(PoPBasicCard());
        myCards.Add(card);


        SetOriginOrder();
        CardAlignment();
    }
  

    public void CancelEntity()
    {
        int index = EntityManager.Inst.myEntities.Count;
        for (int i = 0; i < index; i++)
        {
            Debug.Log(i);
            Entity entity = EntityManager.Inst.myEntities[i];
            GameObject entityObj = entity.gameObject;
            var entitySc = entity.GetComponent<Entity>();
            var cardObject = Instantiate(cardprefab, entityObj.transform.position, Utils.QI);
            var card = cardObject.GetComponent<Card>();
            card.SetUp(entitySc.item);
            myCards.Add(card);
           

            Destroy(entityObj);
            SetOriginOrder();
            //card.MoveTransform(card.originPRS, true, 0.7f);
            CardAlignment();
        }
        EntityManager.Inst.myEntities.RemoveRange(0,EntityManager.Inst.myEntities.Count);
        EntityManager.Inst.myPutCount = 0;
    }
   
    void SetOriginOrder()
    {
        int count = myCards.Count;
        for (int i = 0; i < count; i++)
        {
            var targetCard = myCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
    }

    public void CardAlignment()
    {
        List<PRS> originCardPRSs = new List<PRS>();
        originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one * 0.3f);
        var targetCards = myCards;
        for(int i = 0; i < targetCards.Count; i++)
        {
            var targetCard = targetCards[i];

            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
        }
    }

    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount); //Capacity를 미리 잡아둠 

        switch (objCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.4f, 0.6f }; break;
            case 3: objLerps = new float[] { 0.35f, 0.5f, 0.65f }; break;
            default:
                float interval = 1f / (objCount - 1);
                for(int i = 0; i<objCount; i++)
                {
                    objLerps[i] = interval * i;
                }
                break;
        }

        for(int i = 0; i<objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Utils.QI;
            if(objCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);

            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }

        return results;
    }

    #region MyCard


    void GetObj()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPos, Camera.main.transform.forward);


            if (hitInformation.collider != null)
            {
                GameObject touchedObject = hitInformation.transform.gameObject;

                if (touchedObject.tag == "Card")
                {
                    Card card = touchedObject.GetComponent<Card>();
                    isClick = true;
                    selectCard = card;


                }
            }
           
        }
    }


    public bool TryPutCard(bool isMine)
    {
        if (isMine && EntityManager.Inst.myPutCount >= 4)
            return false;

        Card card = selectCard;
        var spawnPos = Utils.MousePos;
        var targetCards = myCards;
        if (EntityManager.Inst.SpawnEntity(isMine, card.basicCardItem, spawnPos))
        {
            targetCards.Remove(card);
            card.transform.DOKill();
            DestroyImmediate(card.gameObject);
            if (isMine)
            {
                EntityManager.Inst.SelectCard = null;
                EntityManager.Inst.myPutCount++;

            }
            CardAlignment();
            isClick = false;

            return true;
        }
        else
        {
            targetCards.ForEach(x => x.GetComponent<Order>().SetMostFrontOrder(false));
            CardAlignment();
            isClick = false;
            return false;

        }
        return false;
    }
    #endregion

}
