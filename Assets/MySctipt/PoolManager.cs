using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class PoolManager : MonoBehaviour
{
    ObjectPool<GameObject> pool;　//オブジェクトプール

    public GameObject Prefab { get; private set; }　//外部からゲッターのみ参照可能,オブジェクトプール用のプレハブを受け取る

    void Awake()
    {
        //第１引数がオブジェクト生成の関数を受け取る
        //第２引数がプールからオブジェクトをよびだす関数を受け取る
        //第３引数がプールにオブジェクトを返す関数を受け取る
        //第４引数がオブジェクトを消去する関数を受け取る
        pool = new ObjectPool<GameObject>(OnCreatePooledObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
    }

    //オブジェクト生成
    GameObject OnCreatePooledObject()
    {
        return Instantiate(Prefab);
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

    //
    public GameObject GetGameObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        Prefab = prefab;
        GameObject obj = pool.Get();
        Transform tf = obj.transform;
        tf.position = position;
        tf.rotation = rotation;

        return obj;
    }

    public void ReleaseGameObject(GameObject obj)
    {
        pool.Release(obj);
    }
}
