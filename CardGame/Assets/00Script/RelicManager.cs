using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;
public class RelicManager : MonoBehaviour
{
    public static RelicManager Inst { get; set; }
    void AWake() => Inst = this;

    public GameObject rewardPrefab;
    public Image rewardImage;
    public Text rewardText;
    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject relicPrefab;
    public int relicCount;
    public List<Relic> relics;
    List<Item> items;
    public bool isWin;

    void Start()
    {

        SetupRelicBuffer();
        int index = PlayerPrefs.GetInt("index");
        if(index > 0)
        {
            for (int i = 0; i < index; i++)
            {
                Spawn();
               
            }
            MyStat();
        }
    }
    void Update()
    {
        if (GameManager.Inst.isReward)
            AddRelicToReward();
    }
    public void AddRelicToReward()
    {
        GameManager.Inst.isReward = false;
        rewardPrefab.SetActive(true);
        rewardImage.sprite = items[0].itemImg;
        rewardText.text = items[0].information;
        

    }
   public void AcceptRewardBtn()
    {

        rewardPrefab.SetActive(false);
        int index = PlayerPrefs.GetInt("index");
        index++;
        PlayerPrefs.SetInt("index", index);
        GameManager.Inst.StartGame();
    }
    void RelicAligment()
    {
        float targetY = 4.2f;
        var targetRelics = relics;
        for(int i = 0; i < targetRelics.Count; i++)
        {
           
            float targetX = -8.3f + (targetRelics.Count * 1f);
            var targetRelic = targetRelics[i];
            targetRelic.originPos = new Vector3(targetX, targetY);
            //targetRelic.transform.position = targetRelic.originPos;
            relics[i].MoveTransform(targetRelic.originPos, true, 1.5f);

        }
    }


    public void Spawn()
    {
        var spawnPos = Utils.MousePos;
        GameObject spawnObj =  Instantiate(relicPrefab, spawnPos,Utils.QI);
        Relic relic = spawnObj.GetComponent<Relic>();
        relic.SetUp(PoPItem());
        relics.Add(relic);
        RelicAligment();


    }
    public Item PoPItem()
    {
        //    if (items.Count == 0)
        //    {
        //        SetupRelicBuffer();
        //  유물은 중복 x // 스테이지보다 많게 설정하기! 
        //    }
        Item popItem = items[0];
        items.RemoveAt(0);
        return popItem;
    }
    void SetupRelicBuffer()
    {
        items = new List<Item>(10);
        for(int i = 0; i< itemSO.playerItems.Length; i++)
        {
            Item plyItem = itemSO.playerItems[i];
            items.Add(plyItem);
        }
       
    }
    public void MyStat()
    {

        
        for (int i = 0; i < relics.Count; i++)
        {
            int stat = relics[i].stat;
            switch (relics[i].type)
            {
                case "Hp":
                    GameManager.Inst.Health();
                    break;
                case "MaxHp":
                    GameManager.Inst.maxHealth = GameManager.Inst.maxHealth + stat;
                    break;
                case "Defence":
                    GameManager.Inst.defensivePower++;
                    break;
                case "Attack":
                    GameManager.Inst.power = GameManager.Inst.power + stat;
                    break;
                case "Avoidance":
                    GameManager.Inst.playerSC.isAvoidence = true;
                    GameManager.Inst.avoidance = GameManager.Inst.avoidance + stat;
                    break;
                case "Critical":
                    GameManager.Inst.critical = GameManager.Inst.critical + stat;
                    break;
                case "CardDrawCount":
                    
                    break;
                case "ChooseCount":
                    EntityManager.Inst.MAX_ENTITY_COUNT++;
                    break;

                case "longDistanceAvoidance":
                    //어택 함수 만들면서 스크립트에 bool값 추가 하기
                    GameManager.Inst.playerSC.isLongDistanceAvoidance = true;
                    break;
            }
        }
    }

}
