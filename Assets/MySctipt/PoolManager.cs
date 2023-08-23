using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class PoolManager : MonoBehaviour
{
    ObjectPool<GameObject> pool;�@//�I�u�W�F�N�g�v�[��

    public GameObject Prefab { get; private set; }�@//�O������Q�b�^�[�̂ݎQ�Ɖ\,�I�u�W�F�N�g�v�[���p�̃v���n�u���󂯎��

    void Awake()
    {
        //��P�������I�u�W�F�N�g�����̊֐����󂯎��
        //��Q�������v�[������I�u�W�F�N�g����т����֐����󂯎��
        //��R�������v�[���ɃI�u�W�F�N�g��Ԃ��֐����󂯎��
        //��S�������I�u�W�F�N�g����������֐����󂯎��
        pool = new ObjectPool<GameObject>(OnCreatePooledObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
    }

    //�I�u�W�F�N�g����
    GameObject OnCreatePooledObject()
    {
        return Instantiate(Prefab);
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
