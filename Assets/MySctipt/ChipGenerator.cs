using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ChipGenerator : MonoBehaviour
{
    enum ChipRate //チップのレート
    {
        low,
        medium,
        high,
        
    }

    [SerializeField] private List<GameObject> spawnPoint = new List<GameObject>();


    [SerializeField] private List<GameObject> chipObj = new List<GameObject>();   //チップのプレハブ
    private PoolManager[] poolManager;//3個のチップ用オブジェクトプール

    private const int POOL_SIZE = 100; //チップの限度数

    //debug
    WaitForSeconds waitForSeconds = new WaitForSeconds(0.3f);

    // Start is called before the first frame update
    void Start()
    {
        foreach (var rate in Enum.GetValues(typeof(ChipRate)))
        {
            this.gameObject.AddComponent<PoolManager>();
        }
        poolManager = GetComponents<PoolManager>();

        //チップのレートの数オブジェクトプールを作成
        foreach (var rate in Enum.GetValues(typeof(ChipRate)))
        {
            poolManager[(int)rate].EntryPrefab(chipObj[(int)rate]);
            poolManager[(int)rate].CreateObjectPool(POOL_SIZE);
        }
        //debug
        StartCoroutine(DebugCoroutine());
    }

    IEnumerator DebugCoroutine()
    {
        while (true)
        {
            
            foreach (var rate in Enum.GetValues(typeof(ChipRate)))
            {

                poolManager[(int)rate].GetObj();
            }
                yield return waitForSeconds;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
