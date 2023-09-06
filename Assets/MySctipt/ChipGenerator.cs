using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ChipGenerator : MonoBehaviour
{
    enum ChipRate //�`�b�v�̃��[�g
    {
        low,
        medium,
        high,
        
    }

    [SerializeField] private List<GameObject> spawnPoint = new List<GameObject>();


    [SerializeField] private List<GameObject> chipObj = new List<GameObject>();   //�`�b�v�̃v���n�u
    private PoolManager[] poolManager;//3�̃`�b�v�p�I�u�W�F�N�g�v�[��

    private const int POOL_SIZE = 100; //�`�b�v�̌��x��

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

        //�`�b�v�̃��[�g�̐��I�u�W�F�N�g�v�[�����쐬
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
