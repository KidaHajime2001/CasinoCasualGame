using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Pool;
public class PoolManager : MonoBehaviour
{
    IObjectPool<GameObject> pool;　//オブジェクトプール

    private GameObject prefab;//外部からゲッターのみ参照可能,オブジェクトプール用のプレハブを受け取る
    private List<GameObject> nowActiveList;

    public void EntryPrefab(GameObject _poolBase)
    {
       prefab = _poolBase;
    }

    public void CreateObjectPool(int _poolSize)
    {
        if (!prefab)
        {
            throw new ArgumentException($"{nameof(prefab)}に登録してください");
        }
        //第１引数がオブジェクト生成の関数を受け取る
        //第２引数がプールからオブジェクトをよびだす関数を受け取る
        //第３引数がプールにオブジェクトを返す関数を受け取る
        //第４引数がオブジェクトを消去する関数を受け取る
        pool = new ObjectPool<GameObject>(OnCreatePooledObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,true,10,50);
        nowActiveList = new List<GameObject>();
    }


    //オブジェクト生成
    GameObject OnCreatePooledObject()
    {
        var obj= Instantiate(prefab);


        return obj;
    }

    //プールからオブジェクトを呼びだす
    void OnGetFromPool(GameObject _obj)
    {

        _obj.SetActive(true);
    }

    //プールにオブジェクトを返却する
    void OnReleaseToPool(GameObject _obj)
    {

        _obj.SetActive(false);
    }

    //オブジェクトを削除する
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
