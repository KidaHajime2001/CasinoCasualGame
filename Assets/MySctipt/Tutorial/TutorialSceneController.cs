using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TutorialSceneController : MonoBehaviour
{
    enum TutorialProgress
    {
        Walking,
        
        // Game1で操作方法を伝える
        Selecting,
        Betting,
        CountingDown,
        
        // Game2は自由に操作させる
        Thinking,

        // 共通
        ShowingResult,
        Ending,
    }
    TutorialProgress progress;
    GameStageProgress normalProgress;   // 通常のゲームで使う進捗(カメラの向きのために使用)

    bool isTutorial = true;
    bool hasWalked = false;
    float deltaTimeCounter = 0.0f;
    Vector3 nowPos;
    Vector3 aimPos;

    [SerializeField] GameObject player;
    [SerializeField] float walkSpeed;
    [SerializeField] List<GameObject> eventLocations;
    [SerializeField] CameraControl camControl;

    int currentLocation = 0;


    // Start is called before the first frame update
    void Start()
    {
        // ■初期化
        player.transform.position = eventLocations[0].transform.position;
        // 初期コイン枚数111枚

        this.ToNextProgress();
    }

    // Update is called once per frame
    async void Update()
    {
        switch(this.progress)
        {
            case TutorialProgress.Walking:
                if(!this.hasWalked)
                {
                    await Task.Delay(3000);
                    this.hasWalked = true;
                }
                // 移動目標を設定
                this.aimPos = this.eventLocations[this.currentLocation].transform.position;
                this.deltaTimeCounter += Time.deltaTime;
                // 移動処理
                this.player.transform.position = Vector3.Lerp(this.nowPos, this.aimPos, this.deltaTimeCounter * this.walkSpeed);
                // 移動が完了した場合
                if(Vector3.Distance(this.player.transform.position, this.aimPos) <= 0.5f)
                {
                    await Task.Delay(1000);
                    // 進捗を次に移す
                    if(isTutorial)
                    {
                        this.progress = TutorialProgress.Selecting;
                        this.normalProgress = GameStageProgress.Thinking;
                    }
                    else
                    {
                        this.progress = TutorialProgress.Thinking;
                        this.normalProgress = GameStageProgress.Thinking;
                    }
                    // カメラを課題ように変更
                    this.camControl.SetMiddle();
                }
            break;

            case TutorialProgress.Selecting:
                // ■Game1
                // 左を選択させる
                // 1, 10, 100 それぞれボタンを押させ、111枚賭けさせる
                // カウントダウンを見せる
                // 結果を見せる(プラスになる)

                break;

            case TutorialProgress.Betting:

            break;

            case TutorialProgress.CountingDown:

            break;

            case TutorialProgress.Thinking:

            break;

            case TutorialProgress.ShowingResult:

            break;

            case TutorialProgress.Ending:

            break;
        }

        // カメラに進捗を設定
        camControl.SetProgress(normalProgress);


        // ■Game2フェーズ
        // 自由に選択
        // 自由にベット
        // カウントダウン
        // 結果を見せる(プラスもマイナスも有る)

        // ■リザルト
    }

    /// <summary>
    /// 目標を次の目標へ変更するための関数
    /// </summary>
    void ToNextProgress()
    {
        // 進捗を移動に変更
        this.progress = TutorialProgress.Walking;
        this.normalProgress = GameStageProgress.Walking;
        // 次の目標に向けて値の初期化と設定
        this.deltaTimeCounter = 0.0f;
        this.nowPos = this.player.transform.position;
        ++this.currentLocation;
        // もしゴールへ到達していた場合、進捗をゲーム終了にする。
        if (this.eventLocations.Count-1 == this.currentLocation)
        {
            this.progress = TutorialProgress.Ending;
        }
    }
}
