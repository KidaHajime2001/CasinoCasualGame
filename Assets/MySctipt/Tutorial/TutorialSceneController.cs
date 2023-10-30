using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

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
    Vector3 fingerAimPos;
    bool fingerRoundTripFlag = false;
    bool pressedLeftButton = false;

    [SerializeField] GameObject player;
    [SerializeField] float walkSpeed;
    [SerializeField] List<GameObject> eventLocations;
    [SerializeField] CameraControl camControl;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] GameObject bet1;
    [SerializeField] GameObject bet10;
    [SerializeField] GameObject bet100;
    [SerializeField] GameObject finger;
    [SerializeField] float fingerSpeed;
    [SerializeField] float fingerMagnification;
    [SerializeField] float fingerScalingSpeed;

    int currentLocation = 0;


    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーの位置を開始位置に設定
        player.transform.position = eventLocations[0].transform.position;
        // 左右の選択ボタンを無効化
        leftButton.interactable = false;
        rightButton.interactable = false;
        // 指を非表示
        finger.SetActive(false);
        finger.transform.position = leftButton.transform.position;
        
        // 初期コイン枚数111枚

        this.ToNextProgress();
    }

    // Update is called once per frame
    async void Update()
    {
        switch (this.progress)
        {
            case TutorialProgress.Walking:
                if (!this.hasWalked)
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
                if (Vector3.Distance(this.player.transform.position, this.aimPos) <= 0.5f)
                {
                    await Task.Delay(1000);
                    // 進捗を次に移す
                    if (isTutorial)
                    {
                        this.progress = TutorialProgress.Selecting;
                        this.normalProgress = GameStageProgress.Thinking;

                        this.finger.SetActive(true);
                        this.deltaTimeCounter = 0.0f;

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
                // 指UIの往復処理
                if (!this.fingerRoundTripFlag)
                {
                    // 左のボタンの位置にいたら
                    if (Vector3.Distance(this.finger.transform.position, this.leftButton.transform.position) <= 0.5f)
                    {
                        this.fingerAimPos = this.rightButton.transform.position;
                    }
                    // 右のボタンの位置にいたら
                    else if (Vector3.Distance(this.finger.transform.position, this.rightButton.transform.position) <= 0.5f)
                    {
                        this.fingerAimPos = this.leftButton.transform.position;
                    }
                    // 移動処理
                    // 目標が右のボタンなら
                    if (this.fingerAimPos == this.rightButton.transform.position)
                    {
                        this.deltaTimeCounter += Time.deltaTime;
                    }
                    else if (this.fingerAimPos == this.leftButton.transform.position)
                    {
                        this.deltaTimeCounter -= Time.deltaTime;
                    }
                    this.finger.transform.position = Vector3.Lerp(this.leftButton.transform.position, this.rightButton.transform.position, this.deltaTimeCounter * this.fingerSpeed);

                    // 往復が終わったら
                    if(Vector3.Distance(this.finger.transform.position, this.leftButton.transform.position) <= 0.5f
                        && Vector3.Distance(this.fingerAimPos, this.leftButton.transform.position) <= 0.5f)
                    {
                        this.fingerRoundTripFlag = true;
                        this.deltaTimeCounter = 0.0f;
                    }
                }
                else
                {
                    // 左のボタンを押せるようにする
                    this.leftButton.interactable = true;
                    // 左のボタンを押すように拡大縮小させる
                    //this.deltaTimeCounter += Time.deltaTime;

                    // 左のボタンが押されたらベットに進む
                    if(this.pressedLeftButton)
                    {
                        await Task.Delay(1000);
                        this.progress = TutorialProgress.Betting;
                        this.deltaTimeCounter = 0.0f;
                    }
                }

                break;

            case TutorialProgress.Betting:
                // 1, 10, 100 それぞれボタンを押させ、111枚賭けさせる
                // 1の位置に指を移動
                this.finger.transform.position = this.bet1.transform.position;

                break;

            case TutorialProgress.CountingDown:
                // カウントダウンを見せる

                break;

            case TutorialProgress.Thinking:

                break;

            case TutorialProgress.ShowingResult:
                // 結果を見せる(プラスになる)

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

    public void PressedLeftButton()
    {
        // 進捗がチュートリアルの選択で
        // 往復した後、有効になってるときに押した場合
        if(this.fingerRoundTripFlag && !this.pressedLeftButton)
        {
            this.pressedLeftButton = true;
        }
    }
}
