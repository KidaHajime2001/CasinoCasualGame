using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InfinityTrailMaker : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] uint poolSize;
    [SerializeField] float instaceInterval;
    [SerializeField] List<GameObject> pool;

    float deltaTimeCounter = 0.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        CreatePool();
    }

    // Update is called once per frame
    void Update()
    {
        this.deltaTimeCounter += Time.deltaTime;
        if(this.deltaTimeCounter >= this.instaceInterval)
        {
            this.deltaTimeCounter -= this.instaceInterval;

            // Pool�ɔ�A�N�e�B�u�ȃI�u�W�F�N�g������΁A���W��ݒ肵�ėL����
            GameObject temp = this.GetInactiveElement();
            if(temp != null)
            {
                temp.transform.position = this.transform.position;
                temp.SetActive(true);
            }
        }
    }

    void CreatePool()
    {
        // ��������ʂ����肷��
        int instanceNum = (int)(this.poolSize - this.pool.Count());
        if(instanceNum <= 0)
        {
            return;
        }

        for (int i = 0; i < instanceNum; ++i)
        {
            GameObject temp = Instantiate(this.prefab, this.transform);
            temp.SetActive(false);
            pool.Add(temp);
        }
    }

    GameObject GetInactiveElement()
    {
        for (int i = 0; i < this.pool.Count; ++i)
        {
            if (this.pool[i].activeSelf == false)
            {
                return this.pool[i];
            }
        }
        return null;
    }
}
