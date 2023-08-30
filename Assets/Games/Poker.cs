using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poker : MonoBehaviour
{
    [SerializeField] uint magnification;
    [SerializeField] bool isWin;
    struct Pattern
    {
        uint magnification;
        bool isWin;

        GameObject first;
        GameObject second;
        GameObject third;
        GameObject fourth;
        GameObject fifth;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
