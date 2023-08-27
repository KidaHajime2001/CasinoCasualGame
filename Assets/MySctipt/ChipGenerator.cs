using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ChipGenerator : MonoBehaviour
{
    [SerializeField]
    bool useObjectPool = true;  //�I�u�W�F�N�g�v�[�����g�p���邩�ǂ���
    [SerializeField]
    PoolManager poolManager;    //�I�u�W�F�N�g�v�[���p�ɍ쐬�����N���X
    [SerializeField]
    GameObject prefab;          //�I�u�W�F�N�g�v�[���Ɏg�p����v���n�u
    [SerializeField]
    int spawnCount = 50;         //�������鐔

    [SerializeField]
    GameObject objPos;         //�������鐔
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
