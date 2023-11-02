using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialFinger : MonoBehaviour
{
    [SerializeField] List<Vector3> locations;
    [SerializeField] int step = 0;
    [SerializeField] bool isMoving = false;
    [SerializeField] float speed;

    float deltaTimeCounter = 0.0f;

    void Start()
    {
        this.locations = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.isMoving)
        {
            this.Move();
        }
    }

    void Move()
    {
        this.deltaTimeCounter += Time.deltaTime;

        // 現在の場所から移動
        this.transform.position = Vector3.Lerp(this.locations[this.step - 1], this.locations[this.step], this.deltaTimeCounter * this.speed);

        // 移動が完了したら、移動完了状態にする。
        if(Vector3.Distance(this.transform.position, this.locations[this.step]) <= 0.5f)
        {
            this.isMoving = false;
        }
    }

    /// <summary>
    /// 次の場所へ移動する指示を出す。
    /// </summary>
    public void MoveToNextStep()
    {
        if(!this.CanProceedToNextStep())
        {
            return;
        }
        // 配列の末尾なら移動不可
        if(this.step == this.locations.Count - 1)
        {
            return;
        }
        // 次のステップに進める
        ++this.step;
        // 移動状態にする。
        this.isMoving = true;
    }

    public void MoveToPreviousStep()
    {
        if(!this.CanProceedToNextStep())
        {
            return;
        }
        // 配列の先頭なら移動不可
        if (this.step == 0)
        {
            return;
        }
        // 前のステップに戻る
        --this.step;
        // 移動状態にする
        this.isMoving = true;
    }

    /// <summary>
    /// 次の段階に進めていいか？前の段階に戻っていいか？共通のチェック
    /// </summary>
    bool CanProceedToNextStep()
    {
        // 移動中なら更新不可
        if(this.isMoving)
        {
            return false;
        }

        return true;
    }

    public void SetLocations(List<Vector3> _locations)
    {
        // 引数のデータでlocationsを更新
        this.locations.Clear();
        this.locations = new List<Vector3>(_locations);

        // locations[0]の位置に移動
        this.transform.position = locations[0];
    }

    public bool IsMoving()
    {
        return this.isMoving;
    }
}