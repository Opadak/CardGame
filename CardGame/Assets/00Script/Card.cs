using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class Card : MonoBehaviour
{
    [SerializeField] SpriteRenderer Item;
    [SerializeField] SpriteRenderer type;
    [SerializeField] SpriteRenderer effect;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text damageTMP;
    [SerializeField] TMP_Text distanceTMP;
    [SerializeField] ECardState eCardState;


    public BasicCardItem basicCardItem;
    public PRS originPRS;
    public bool isMouseOver;

    bool isMyCardDrag;
    bool onMyCardArea;
    bool isCardChoice;
    Card card;
    enum ECardState { nothing, CanMouseOver, CanMouseDrag}
    public void SetUp(BasicCardItem basicCardItem)
    {
        this.basicCardItem = basicCardItem;
        nameTMP.text = this.basicCardItem.name;
        damageTMP.text = this.basicCardItem.damage.ToString();
        distanceTMP.text = this.basicCardItem.distance.ToString();
        effect.sprite = this.basicCardItem.AbilitySprite;
        type.sprite = this.basicCardItem.AbilityTypeSprite;
    }
  

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }

    void OnMouseOver()
    {


        
        if (eCardState == ECardState.nothing)
            return;
     
        EnlargeCard(true, this);

    }
    void OnMouseExit()
    {
     
        EnlargeCard(false, this);
    }


    void OnMouseDown()
    {
        isMyCardDrag = true;
        if (eCardState != ECardState.CanMouseDrag)
            return;



    }
    void OnMouseUp()
    {

        isMyCardDrag = false;
        if (eCardState != ECardState.CanMouseDrag)
            return;
        if (onMyCardArea)
            EntityManager.Inst.ReMoveMyEmptyEntity();
     

    }
    
    void Update()
    {
        if (isMyCardDrag)
            CardDrag();

        DetectCardArea();
        SetECardState();
    }

    void SetECardState()
    {
        if (TurnManager.Inst.isLoading)
            eCardState = ECardState.nothing;
        else if (!TurnManager.Inst.myTurn)
            eCardState = ECardState.CanMouseOver;
        else if (TurnManager.Inst.myTurn)
            eCardState = ECardState.CanMouseDrag;
    }


    void CardDrag()
    {


        if (eCardState != ECardState.CanMouseDrag)
            return;
        if (!onMyCardArea)
        {
            this.MoveTransform(new PRS(Utils.MousePos, Utils.QI, this.originPRS.scale), false);
            EntityManager.Inst.InsertMyEmptyEntity(Utils.MousePos.x);
        }
        //if (isCardChoice)
        //{
        //    Vector3 CardChoiceArea = new Vector3(-7.36f, -3.26f, 0);
        //    this.MoveTransform(new PRS(CardChoiceArea, Utils.QI, this.originPRS.scale), false);

        //}
    }

    void DetectCardArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        int cardChoicelayer = LayerMask.NameToLayer("MyChoiceCardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
        //isCardChoice = Array.Exists(hits, x => x.collider.gameObject.layer == cardChoicelayer);

    }


    void EnlargeCard(bool isEnlarge, Card card)
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -2.3f, -10f);
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 0.35f), false);

        }
        else
            card.MoveTransform(card.originPRS, false);

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }


}
