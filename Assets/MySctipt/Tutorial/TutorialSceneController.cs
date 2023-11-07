using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
        Ending
    }
    [SerializeField] TutorialProgress progress;
    GameStageProgress normalProgress;   // 通常のゲームで使う進捗(カメラの向きのために使用)
    public struct DealWave
    {
        public PlayCardDeal dealL;
        public PlayCardDeal dealR;
    }
    Dictionary<BetState, PlayCardDeal> BP;
    [SerializeField] List<DealWave> dealWave;

    // キャラクターの移動
    float deltaTimeCounter = 0.0f;
    bool hasWalked = false;
    Vector3 nowPos;
    Vector3 aimPos;

    [SerializeField] GameObject player;
    [SerializeField] float walkSpeed;

    // チュートリアルの進行
    bool isTutorial = true;
    int pressedButtonCount = 0;
    List<Vector3> selectButtonLocations;
    List<Vector3> betButtonLocations;
    int fingerStepCount = 0;

    int clearPulus = 0;
    float magnitude = 0;
    bool isGameClear = false;
    int qWave = 0;
    int eCount = 0;

    [SerializeField] List<GameObject> eventLocations;
    [SerializeField] CameraControl camControl;
    [SerializeField] List<Button> selectButtons;
    [SerializeField] List<Button> betButtons;
    [SerializeField] TutorialFinger finger;
    [SerializeField] CountDownTimer countDownTimer;
    [SerializeField] Bet bet;
    [SerializeField] List<ClearEffect> effect;
    [SerializeField] ChipGenerator chipGenerator;
    [SerializeField] Result result;

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
        finger.gameObject.SetActive(false);

        // ボタンの座標を配列に記録
        for (int i = 0; i < this.selectButtons.Count; ++i)
        {
            this.selectButtonLocations.Add(selectButtons[i].transform.position);
        }
        for (int i = 0; i < this.betButtons.Count; ++i) 
        {
            this.betButtonLocations.Add(betButtons[i].transform.position);
        }

        BP = new Dictionary<BetState, PlayCardDeal>();

        // 初期コイン枚数の設定(111枚)

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
                    // チュートリアル用
                    if (isTutorial)
                    {
                        this.isTutorial = false;

                        this.progress = TutorialProgress.Selecting;
                        this.normalProgress = GameStageProgress.Thinking;

                        this.finger.gameObject.SetActive(true);
                        this.deltaTimeCounter = 0.0f;

                        // 指の移動情報を渡す
                        this.finger.SetLocations(this.selectButtonLocations);
                    }
                    // 通常用
                    else
                    {
                        // 左右の選択ボタンを無効化
                        for (int i = 0; i < this.selectButtons.Count; ++i)
                        {
                            this.selectButtons[i].interactable = true;
                        }
                        // ベット用のボタンを無効化
                        for (int i = 0; i < this.betButtons.Count; ++i)
                        {
                            this.betButtons[i].interactable = true;
                        }

                        // 時間制限を追加
                        this.countDownTimer.SetTimer(10.0f);

                        this.progress = TutorialProgress.Thinking;
                        this.normalProgress = GameStageProgress.Thinking;
                    }
                    // カメラを課題ように変更
                    this.camControl.SetMiddle();
                }
                break;

            case TutorialProgress.Selecting:
                // ■Game1

                // 左右の選択ボタンと左右のゲーム以外を暗くする。
                // 左右どちらを選択しているかわかりやすくする。

                // 移動中でなければ
                if(!this.finger.IsMoving())
                {
                    switch (this.fingerStepCount)
                    {
                        case 0:
                            // 段階を1増やして移動させる
                            ++this.fingerStepCount;
                            this.finger.MoveToNextStep();
                            break;

                        case 1:
                            // 段階を1増やして移動させる
                            ++this.fingerStepCount;
                            this.finger.MoveToPreviousStep();
                            break;

                        case 2:
                            // 左のボタンを押せるようにして、ボタンが押されたらベットに進む
                            this.selectButtons[0].interactable = true;
                            if (this.pressedButtonCount == 1)
                            {
                                // 左ボタンを押せなくする
                                this.selectButtons[0].interactable = false;
                                // 指の移動情報を渡す
                                this.finger.SetLocations(this.betButtonLocations);
                                this.progress = TutorialProgress.Betting;
                                // 使用した変数の初期化
                                this.fingerStepCount = 0;
                                this.pressedButtonCount = 0;
                            }
                            break;
                    }
                }
                break;

            case TutorialProgress.Betting:

                // ベットボタンと賭けてる額、残りのコイン数の部分以外を暗くする。

                // 移動中でなければ
                if (!this.finger.IsMoving())
                {
                    switch (this.fingerStepCount)
                    {
                        // 0~3で2回進んで2回戻る
                        case 0:
                            ++this.fingerStepCount;
                            this.finger.MoveToNextStep();
                            break;

                        case 1:
                            ++this.fingerStepCount;
                            this.finger.MoveToNextStep();
                            break;

                        case 2:
                            ++this.fingerStepCount;
                            this.finger.MoveToPreviousStep();
                            break;

                        case 3:
                            ++this.fingerStepCount;
                            this.finger.MoveToPreviousStep();
                            break;

                        case 4:
                            // bet1を押せるようにして、ボタンが押されたら次にすすむ
                            this.betButtons[0].interactable = true;
                            if (this.pressedButtonCount == 1)
                            {
                                ++this.fingerStepCount;
                                // bet1を押せなくする
                                this.betButtons[0].interactable = false;
                                // 指の移動情報を渡す
                                this.finger.MoveToNextStep();
                            }
                            break;

                        case 5:
                            // bet10を押せるようにして、ボタンが押されたら次にすすむ
                            this.betButtons[1].interactable = true;
                            if (this.pressedButtonCount == 2)
                            {
                                ++this.fingerStepCount;
                                // bet1を押せなくする
                                this.betButtons[1].interactable = false;
                                // 指の移動情報を渡す
                                this.finger.MoveToNextStep();
                            }
                            break;

                        case 6:
                            // bet100を押せるようにして、ボタンが押されたら次にすすむ
                            this.betButtons[2].interactable = true;
                            if (this.pressedButtonCount == 3)
                            {
                                ++this.fingerStepCount;
                                // bet1を押せなくする
                                this.betButtons[2].interactable = false;
                                // 指の移動情報を渡す
                                this.finger.MoveToNextStep();

                            }
                            break;

                        case 7:
                            // 指を非表示
                            this.finger.gameObject.SetActive(false);
                            // 使用した変数の初期化
                            this.fingerStepCount = 0;
                            this.pressedButtonCount = 0;

                            // カウントダウンの設定
                            this.progress = TutorialProgress.CountingDown;
                            this.countDownTimer.SetTimer(5.0f);
                            break;
                    }
                }

                break;

            case TutorialProgress.CountingDown:
                // カウントダウンを見せる

                // カウントダウンが終了したら
                if (!this.countDownTimer.IsCountingDown())
                {
                    this.countDownTimer.InitDisplay();
                    //進行度を結果発表に移す
                    this.progress = TutorialProgress.ShowingResult;
                    normalProgress = GameStageProgress.Result;

                    // リザルトの表示が上手くいかないため、スキップするために書いてある
                    this.ToNextProgress();
                    
                    // 勝敗をチェックする
                    CheckResult();
                }

                break;

            case TutorialProgress.Thinking:
                // もし時間制限に達したら
                if(!this.countDownTimer.IsCountingDown())
                {
                    // 進捗を結果発表に移す
                    this.progress = TutorialProgress.ShowingResult;
                    normalProgress = GameStageProgress.Result;

                    // リザルトの表示が上手くいかないため、スキップするために書いてある
                    this.ToNextProgress();

                    // 勝敗をチェックする
                    CheckResult();
                }


                break;

            case TutorialProgress.ShowingResult:
                //ゲームクリアフラグが立っていたら
                if (this.isGameClear && this.BP[BetState.R].GetReverseComplete() && this.BP[BetState.L].GetReverseComplete())
                {
                    this.deltaTimeCounter += Time.deltaTime;
                    if (this.deltaTimeCounter >= 3.0f)
                    {
                        this.deltaTimeCounter = 0.0f;

                        UpdateDIC();
                        //カメラを移動用に変更する
                        camControl.SetFar();

                        //エフェクトを起動
                        effect[eCount].StartClearEffect();
                        eCount++;
                        //ゲームクリアのフラグを折る
                        isGameClear = false;
                        //チップをセット
                        chipGenerator.SetChip((bet.GetBetNum() * (int)magnitude) + clearPulus);
                        bet.ResetRimmit();
                        //演出用の時間を確保
                        await Task.Delay(3000);
                        //次の位置へ
                        this.ToNextProgress();
                    }

                }

                break;

            case TutorialProgress.Ending:
                // 移動目標を設定
                this.aimPos = this.eventLocations[this.currentLocation].transform.position;
                this.deltaTimeCounter += Time.deltaTime;
                // 移動処理
                this.player.transform.position = Vector3.Lerp(this.nowPos, this.aimPos, this.deltaTimeCounter * this.walkSpeed);
                // 移動が完了した場合
                if (Vector3.Distance(this.player.transform.position, this.aimPos) <= 0.5f)
                {
                    this.deltaTimeCounter += Time.deltaTime;
                    if(this.deltaTimeCounter >= 3.0f)
                    {
                        // ステージリザルト
                        result.SetFlag(this.chipGenerator.GetChipNum());
                    }
                }
                    break;
        }

        // カメラに進捗を設定
        camControl.SetProgress(normalProgress);
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

    void CheckResult()
    {
        if (this.bet.GetBetState() != BetState.N)
        {
            this.InitClearData(this.bet.GetBetState());
        }

        this.BP[BetState.R].ReverseCard();
        this.BP[BetState.L].ReverseCard();
        Debug.Log("Bet:" + this.bet.GetBetState() + "R/L:" + this.BP[BetState.R].GetResult() + "/" + this.BP[BetState.L].GetResult() + "/nowwave:" + 0);

        ++qWave;
    }

    void InitClearData(BetState _state)
    {
        this.isGameClear = true;

        this.clearPulus = BP[_state].GetPulus();
        this.magnitude = BP[_state].GetMagnitude();

        this.bet.ResetBetState();

    }

    void UpdateDIC()
    {
        Debug.Log("Qwall2R" + qWave);
        if (qWave >= dealWave.Count)
        {
            return;
        }
        BP[BetState.R] = dealWave[qWave].dealR;
        BP[BetState.L] = dealWave[qWave].dealL;
    }
}
