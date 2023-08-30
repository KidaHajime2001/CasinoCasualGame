using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 渡された値から、役を選択し、表示するだけ
// 最初は4パターン程度で

public class Poker : MonoBehaviour
{
    [SerializeField] uint magnification;
    [SerializeField] bool isWin;
    struct Role
    {
        uint magnification;
        bool isWin;

        GameObject first;
        GameObject second;
        GameObject third;
        GameObject fourth;
        GameObject fifth;
    }
    // ツーペア2
    [SerializeField] Role two;
    [SerializeField] Role missingTwo;
    // スリーカード3
    [SerializeField] Role three;
    [SerializeField] Role missingThree;



    // ストレート4

    // フラッシュ5

    // フルハウス7

    // フォーカード20

    // ストレートフラッシュ50

    // ロイヤルストレートフラッシュ100

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
