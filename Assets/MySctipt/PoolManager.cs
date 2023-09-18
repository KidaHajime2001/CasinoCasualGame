using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Pool;
public class PoolManager : MonoBehaviour
{
    IObjectPool<GameObject> pool;�@//�I�u�W�F�N�g�v�[��

    private GameObject prefab;//�O������Q�b�^�[�̂ݎQ�Ɖ\,�I�u�W�F�N�g�v�[���p�̃v���n�u���󂯎��
    private List<GameObject> nowActiveList;

    public void EntryPrefab(GameObject _poolBase)
    {
       prefab = _poolBase;
    }

    public void CreateObjectPool(int _poolSize)
    {
        if (!prefab)
        {
            throw new ArgumentException($"{nameof(prefab)}�ɓo�^���Ă�������");
        }
        //��P�������I�u�W�F�N�g�����̊֐����󂯎��
        //��Q�������v�[������I�u�W�F�N�g����т����֐����󂯎��
        //��R�������v�[���ɃI�u�W�F�N�g��Ԃ��֐����󂯎��
        //��S�������I�u�W�F�N�g����������֐����󂯎��
        pool = new ObjectPool<GameObject>(OnCreatePooledObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,true,10,50);
        nowActiveList = new List<GameObject>();
    }


    //�I�u�W�F�N�g����
    GameObject OnCreatePooledObject()
    {
        var obj= Instantiate(prefab);


        return obj;
    }

    //�v�[������I�u�W�F�N�g���Ăт���
    void OnGetFromPool(GameObject _obj)
    {

        _obj.SetActive(true);
    }

    //�v�[���ɃI�u�W�F�N�g��ԋp����
    void OnReleaseToPool(GameObject _obj)
    {

        _obj.SetActive(false);
    }

    //�I�u�W�F�N�g���폜����
    void OnDestroyPooledObject(GameObject _obj)
    {
        Destroy(_obj);
    }

    public GameObject GetObj()
    {
        var obj = pool.Get();
        obj.AddComponent<DeleteInvoke>();
        obj.GetComponent<DeleteInvoke>().InitStatus(pool);
        nowActiveList.Add(obj);
        return obj;
    }
    public void ReleaseObj()
    {
        if(nowActiveList.Count>0)
        {
            nowActiveList[nowActiveList.Count - 1].GetComponent<DeleteInvoke>().Release();
            nowActiveList.Remove(nowActiveList[nowActiveList.Count - 1]);



        }
        
    }
    public void SpawnGameObject(Vector3 pos,Quaternion rot)
    {
       var obj = GetObj();
        obj.transform.position = pos;
        obj.transform.rotation = rot;
    }
}
