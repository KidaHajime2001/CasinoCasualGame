using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ChipGenerator : MonoBehaviour
{
    [SerializeField]
    bool useObjectPool = true;  //オブジェクトプールを使用するかどうか
    [SerializeField]
    PoolManager poolManager;    //オブジェクトプール用に作成したクラス
    [SerializeField]
    GameObject prefab;          //オブジェクトプールに使用するプレハブ
    [SerializeField]
    int spawnCount = 50;         //生成する数

    [SerializeField]
    GameObject objPos;         //生成する数
    Vector3 pos;

    [SerializeField]
    float spawnInterval = 0.1f;
    [SerializeField]
    Vector3 minSpawnPosition = Vector3.zero;
    [SerializeField]
    Vector3 maxSpawnPosition = Vector3.zero;
    [SerializeField]
    float destroyWaitTime = 3;

    WaitForSeconds spawnIntervalWait;

    void Start()
    {
        pos=objPos.transform.position;
        spawnIntervalWait = new WaitForSeconds(spawnInterval);

        StartCoroutine(nameof(SpawnTimer));
    }

    IEnumerator SpawnTimer()
    {
        int i;

        while (true)
        {
            for (i = 0; i < spawnCount; i++)
            {
                poolManager.GetGameObject(prefab, pos, Quaternion.identity);
                
            }

            yield return spawnIntervalWait;
        }
    }

}
