using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialFinger : MonoBehaviour
{
    [SerializeField] Vector3 startPos;
    [SerializeField] Vector3 aimPos;
    [SerializeField] bool isMoving = false;
    [SerializeField] float speed;
    [SerializeField] bool isBack = false;

    List<Vector3> locations;
    int step = 0;
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

        if(this.isBack)
        {
            this.startPos = this.locations[this.step + 1];
            this.aimPos = this.locations[this.step];
        }
        else
        {
            this.startPos = this.locations[this.step - 1];
            this.aimPos = this.locations[this.step];
        }

        // 現在の場所から移動
        this.transform.position = Vector3.Lerp(startPos, aimPos, this.deltaTimeCounter * this.speed);

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
        // 配列の末尾なら移動不可
        if(this.step == this.locations.Count - 1)
        {
            return;
        }
        // 次のステップに進める
        ++this.step;
        // 次に進むようにして
        this.isBack = false;
        // 移動状態にする。
        this.isMoving = true;
        // デルタタイムをリセット
        this.deltaTimeCounter = 0.0f;
    }

    /// <summary>
    /// 前の場所へ移動する指示を出す。
    /// </summary>
    public void MoveToPreviousStep()
    {
        // 配列の先頭なら移動不可
        if (this.step == 0)
        {
            return;
        }
        // 前のステップに戻る
        --this.step;
        // 前に戻るようにして
        this.isBack = true;
        // 移動状態にする
        this.isMoving = true;
        // デルタタイムをリセット
        this.deltaTimeCounter = 0.0f;
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