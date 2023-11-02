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
    [SerializeField]int pressedButtonCount = 0;
    List<Vector3> selectButtonLocations;
    List<Vector3> betButtonLocations;
    int fingerStepCount = 0;

    [SerializeField] GameObject player;
    [SerializeField] float walkSpeed;
    [SerializeField] List<GameObject> eventLocations;
    [SerializeField] CameraControl camControl;
    [SerializeField] List<Button> selectButtons;
    [SerializeField] List<Button> betButtons;
    [SerializeField] GameObject finger;
    [SerializeField] float fingerSpeed;

    [SerializeField] TutorialFinger tutoFinger;

    int currentLocation = 0;


    // Start is called before the first frame update
    void Start()
    {
        this.selectButtonLocations = new List<Vector3>();
        this.betButtonLocations = new List<Vector3>();

        // プレイヤーの位置を開始位置に設定
        player.transform.position = eventLocations[0].transform.position;
        // 左右の選択ボタンを無効化
        for (int i = 0; i < this.selectButtons.Count; ++i)
        {
            this.selectButtons[i].interactable = false;
        }
        // ベット用のボタンを無効化
        for (int i = 0; i < this.betButtons.Count; ++i)
        {
            this.betButtons[i].interactable = false;
        }
        // 指を非表示
        finger.SetActive(false);

        // ボタンの座標を配列に記録
        for (int i = 0; i < this.selectButtons.Count; ++i)
        {
            this.selectButtonLocations.Add(selectButtons[i].transform.position);
        }
        for (int i = 0; i < this.betButtons.Count; ++i) 
        {
            this.betButtonLocations.Add(betButtons[i].transform.position);
        }

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
                    // 進捗を次に移す
                    if (isTutorial)
                    {
                        this.progress = TutorialProgress.Selecting;
                        this.normalProgress = GameStageProgress.Thinking;

                        this.finger.SetActive(true);
                        this.deltaTimeCounter = 0.0f;

                        // 指の移動情報を渡す
                        this.tutoFinger.SetLocations(this.selectButtonLocations);
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
                // 移動中でなければ
                if(!this.tutoFinger.IsMoving())
                {
                    switch (this.fingerStepCount)
                    {
                        case 0:
                            // 段階を1増やして移動させる
                            ++this.fingerStepCount;
                            this.tutoFinger.MoveToNextStep();
                            break;

                        case 1:
                            // 段階を1増やして移動させる
                            ++this.fingerStepCount;
                            this.tutoFinger.MoveToPreviousStep();
                            break;

                        case 2:
                            // 左のボタンを押せるようにして、ボタンが押されたらベットに進む
                            this.selectButtons[0].interactable = true;
                            if (this.pressedButtonCount == 1)
                            {
                                await Task.Delay(1000);
                                // 左ボタンを押せなくする
                                this.selectButtons[0].interactable = false;
                                // 指の移動情報を渡す
                                this.tutoFinger.SetLocations(this.betButtonLocations);
                                this.progress = TutorialProgress.Betting;
                                // 使用した変数の初期化
                                this.deltaTimeCounter = 0.0f;
                                this.fingerStepCount = 0;
                                this.pressedButtonCount = 0;
                            }
                            break;
                    }
                }
                break;

            case TutorialProgress.Betting:

                // 移動中でなければ
                if (!this.tutoFinger.IsMoving())
                {
                    switch (this.fingerStepCount)
                    {
                        case 0:
                            // bet1を押せるようにして、ボタンが押されたら次にすすむ
                            this.betButtons[0].interactable = true;
                            if(this.pressedButtonCount == 1)
                            {
                                ++this.fingerStepCount;
                                // bet1を押せなくする
                                this.betButtons[0].interactable = false;
                                // 指の移動情報を渡す
                                this.tutoFinger.MoveToNextStep();
                            }
                            break;
                        case 1:
                            // bet10を押せるようにして、ボタンが押されたら次にすすむ
                            this.betButtons[1].interactable = true;
                            if (this.pressedButtonCount == 2)
                            {
                                ++this.fingerStepCount;
                                // bet1を押せなくする
                                this.betButtons[1].interactable = false;
                                // 指の移動情報を渡す
                                this.tutoFinger.MoveToNextStep();
                            }
                            break;
                        case 2:
                            // bet100を押せるようにして、ボタンが押されたら次にすすむ
                            this.betButtons[2].interactable = true;
                            if (this.pressedButtonCount == 3)
                            {
                                ++this.fingerStepCount;
                                // bet1を押せなくする
                                this.betButtons[2].interactable = false;
                                // 指の移動情報を渡す
                                this.tutoFinger.MoveToNextStep();
                            }
                            break;
                    }
                }

                // 次にベットするボタンを有効にする
                for (int i = 0; i < this.betButtons.Count; ++i)
                {
                    if(this.betButtons[i].interactable == true)
                    {
                        // 有効なボタンが末尾の場合は終了
                        if (i == this.betButtons.Count - 1)
                        {
                            break;
                        }
                        else
                        {
                            this.betButtons[i].interactable = false;
                            this.betButtons[i+1].interactable = true;
                        }
                    }
                }
                
                
                // 1, 10, 100 それぞれボタンを押させ、111枚賭けさせる


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

    public void PressedButton()
    {
        ++this.pressedButtonCount;
    }
}
