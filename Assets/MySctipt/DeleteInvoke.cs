using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DeleteInvoke : MonoBehaviour
{
    IObjectPool<GameObject> pool;
    float limitTime = 3.0f;
    float countTime = 0.0f;
        // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Release()
    {
        pool.Release(this.gameObject);
    }
    public void InitStatus(IObjectPool<GameObject> _pool)
    {
        pool = _pool;
        countTime = 0.0f;
    }
    private void Update()
    { 
    //{
    //    if (pool == null) return;

    //    countTime += Time.deltaTime;
    //    if(limitTime<=countTime)
    //    {
    //        Release();
    //    }
    }

}
