using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ディーラーから倍率と役成立フラグが渡される。
// 役成立      ：倍率に合う手札を作成。
// 役不成立    ：倍率に合う手札から、一枚をランダムに変更or倍率に近い役の惜しい、不成立の手札を作成。

// 一度仮置きで作成。(渡された倍率を表示するのみ。)
public class Poker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 手札を生成する。
    /// </summary>
    /// <param name="_mag">作成したい役の倍率</param>
    /// <param name="isValid">役が成立したものを生成するか？</param>
    public void GenerateHands(int _mag, bool isValid)
    {

    }
}