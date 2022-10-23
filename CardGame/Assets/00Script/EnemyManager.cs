using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] GameObject[] EnemyPrefab;
    [SerializeField] Transform EnemySpawnPos;
    public GameObject[] enemies;
    public int count;
    public void EnemySpawn(string isSpawn,int index)
    {
        if (isSpawn == "F")
            return;
        Debug.Log(count);
        GameObject enemtGO = Instantiate(EnemyPrefab[index], EnemySpawnPos.transform);
        
    }



 
}
