using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Random = UnityEngine.Random;
public class TurnManager : MonoBehaviour
{
    public static TurnManager Inst { get; private set; }
    void Awake() => Inst = this;

    [Header("Develop")]
    [SerializeField] [Tooltip("시작 턴 모드를 정합니다.")] EturnMode eturnMode;
    [SerializeField] [Tooltip("카드 배분이 빨라집니다.")] bool fastMode;
    [SerializeField] [Tooltip("시작 카드 개수를 정합니다.")] int startCardCount;

    [Header("Properties")]
    public bool isLoading; // 게임이 끝나면 isLoading을 true => 카드와 엔티티 클릭 방지 
    public bool myTurn;
    public int myTurnCount;
    public int enemyTurnCount;
    enum EturnMode { Random, My, Enemy}
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);
    WaitForSeconds delay15 = new WaitForSeconds(1.5f);
    WaitForSeconds delay30 = new WaitForSeconds(3f);
    public static Action<bool> OnAddCard;
    public static event Action<bool> OnTurnStarted;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;
   
    public bool isEmptyEnemy;
    public bool isFindEnemy;
    void Update()
    {
        FindEnemy();
    }
    void GameSetUp()
    {
        if (fastMode)
        {
            delay05 = new WaitForSeconds(0.05f);
          
        }
        switch (eturnMode)
        {
            case EturnMode.Random:
                myTurn = Random.Range(0, 2) == 0;
                break;
            case EturnMode.My:
                myTurn = true;
                break;
            case EturnMode.Enemy:
                myTurn = false;
                break;
        }
    }

    public IEnumerator StartGameCo()
    {
        GameSetUp();
        isLoading = true;
        spawnList = new List<Spawn>();
        ReadSpawnFile(GameManager.Inst.stage);
            for (int i = 0; i < startCardCount; i++)
            {
                yield return delay05;
                OnAddCard?.Invoke(false);
                //yield return delay05;
                //OnAddCard?.Invoke(true);
            }
        yield return delay05;
        OnAddCard?.Invoke(false);
        StartCoroutine(StartTurnCo());
    }

   public IEnumerator StartTurnCo()
    {
        if(myTurnCount == 0 && enemyTurnCount == 0)
        {
            GameManager.Inst.GameNotification("Stage" + GameManager.Inst.stage);
            yield return delay15;
        }
        isLoading = true;
        if (myTurn)
        {
            EntityManager.Inst.myEntities.RemoveRange(0, EntityManager.Inst.myEntities.Count);
            var entityTrush = GameObject.FindGameObjectsWithTag("Entity");
            for(int i = 0; i < entityTrush.Length; i++)
            {
                Destroy(entityTrush[i]);
            }
            GameManager.Inst.Notification("나의 턴");
            yield return delay07;
            OnAddCard?.Invoke(myTurn);
            
            myTurnCount++;
        }
        else
        {
            yield return StartCoroutine(EntityManager.Inst.MoveEntitiesCo());
            //EntityManager.Inst.BehaviorPlayer();
            yield return delay15;
            if (enemyTurnCount < spawnList.Count)
            {
                EnemyManager.Inst.EnemySpawn(spawnList[enemyTurnCount].isSpawn, spawnList[enemyTurnCount].enemyIndex);
            }
            yield return delay15;
            enemyTurnCount++;
            isFindEnemy = true;
           

        }


        yield return delay15;
        isLoading = false;
        
        OnTurnStarted?.Invoke(myTurn);
        if (isEmptyEnemy && spawnList.Count <= enemyTurnCount)
        {
           GameManager.Inst.Win();
           GameManager.Inst.isFinish = true;
            
        }
        if (!myTurn&&!GameManager.Inst.isFinish)
        {
            myTurn = !myTurn;
            StartCoroutine(StartTurnCo());
        }
         
    }


    void FindEnemy()
    {
        if (!isFindEnemy)
            return;
        var Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(Enemies.Length);
        if (Enemies.Length == 0)
        {
            isEmptyEnemy = true;
            return;
        }
        for(int i = 0; i < Enemies.Length; i++)
        {
            Enemy enemySc = Enemies[i].GetComponent<Enemy>();
            Debug.Log(enemySc.enabled);
           
            if(enemySc.CheckDistance())
                enemySc.Attack();
            else
                enemySc.Move();


        }
     
        isFindEnemy = false;
      
    }
    
    public void EndTurn()
    {
       
        if (EntityManager.Inst.myEntities.Count == 0)
            return;
        if (GameManager.Inst.isFinish)
            return;
        myTurn = !myTurn;
        StartCoroutine(StartTurnCo());
    }

    void ReadSpawnFile(int Stageindex)
    {
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;
        Stageindex = PlayerPrefs.GetInt("stage");
        TextAsset textFile = Resources.Load("stage" + Stageindex) as TextAsset;
        Debug.Log(textFile);
        StringReader stringReader = new StringReader(textFile.text);

        while(stringReader != null)
        {
            string line = stringReader.ReadLine();
            if(line == null)
            {
                break;
            }
            Spawn spawnData = new Spawn();
            spawnData.isSpawn = line.Split(',')[0];
            spawnData.enemyIndex = int.Parse(line.Split(',')[1]);
            spawnList.Add(spawnData);

        }
        stringReader.Close();
        
    }
  
}
