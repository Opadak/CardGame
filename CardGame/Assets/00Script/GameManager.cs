using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Inst { get; private set; }
    void Awake() => Inst = this;
    public TMP_Text HpText;
    public TMP_Text StageText;
    public int health ;
    public int maxHealth ;
    public int power;
    public int defensivePower ;
    public int avoidance ; //회피
    public int critical ;
    public int stage;
    public GameObject player;
    public Player playerSC;
    [SerializeField] NoticationPanel noticationPanel;
    [SerializeField] NoticationPanel startGamePanel;
    [SerializeField] GameObject RewardPanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject RewardBox;
    [SerializeField] Transform RewardBoxPos;
    public bool isFinish;
   
    public bool isReward;
    void Start()
    {

        playerSC = player.GetComponent<Player>();
      
  
        InitialValue();
        StartCoroutine(TurnManager.Inst.StartGameCo());
    }

 

    void Update()
    {
      
        
        if(health <= 0)
        {
            GameOver();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerPrefs.SetInt("stage", 0);
            PlayerPrefs.SetInt("index", 0);
        }

    }

    

    public void StartGame()
    {

        SceneManager.LoadScene(0);
    }

    public void Notification(string message)
    {
        noticationPanel.Show(message);
    }
    public void GameNotification(string message)
    {
        startGamePanel.Show(message);
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
    public void Health()
    {
        if(health >= maxHealth)
        {
            health = maxHealth;
            return;
        }
        health++;
        HpText.text = health.ToString();
    }
   
   public void Win()
    {
       
        stage++;
        PlayerPrefs.SetInt("stage", stage);
        //Instantiate(RewardBox, RewardBoxPos);
        isReward = true;
    }
   

    void InitialValue()
    {
        int stage = PlayerPrefs.GetInt("stage");

        if (stage < 10)
            StageText.text = "0" + stage;
        else
            StageText.text = stage.ToString();

        health = 20;
        maxHealth = 20;
        power = 0;
        defensivePower = 0;
        avoidance = 10;
        critical = 10;
    }

  
    //Hp,MaxHp,Defence,Attack,Avoidance,Critical,CardDrawCount,ChooseCount,longDistanceAvoidance
}


