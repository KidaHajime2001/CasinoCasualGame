using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using  System.Threading.Tasks;
public class GameSceneControl : MonoBehaviour
{
    enum CheckPointType
    {
        Other,
        End,
    }

    struct CheckPoint
    {
        public Vector3 Position;
        public CheckPointType cP;
        public void Init(Vector3 vec)
        {
            Position = vec;
            cP = CheckPointType.Other;
        }
    }
    [System.Serializable]
    public struct DealWave
    {
       public  PlayCardDeal dealL;
       public  PlayCardDeal dealR;
    }

    GameStageProgress progress;
    [SerializeField] List<GameObject> objPoint;
    [SerializeField] GameObject cameraController;
    CameraControl cameraControl;

    private CheckPoint[] checkPoints;
    private int nowWave;
    private int qWave;

    [SerializeField] private GameObject player;


    private Vector3 nowAimPosition;
    private Vector3 firstPosition;
    private float f = 0;


    [SerializeField]
    private float walkSpeed;
    
    [SerializeField] GameObject qTimeUi;
    ThinkingTime thinkingTime;

    [SerializeField] Bet bet;
    [SerializeField] List<DealWave> dealWave;
    

    [SerializeField] ChipGenerator chipGenerator;

    [SerializeField] List<ClearEffect> effect;
    private int eCount=0;

    bool GameClear = false;
    float magnitude=0;
    int clearPulus = 0;

    [SerializeField] Result result;
    Dictionary<BetState, PlayCardDeal> BP;
    bool firstFlag = false;
    //カウントアップ
    private float countup = 0.0f;
    private float timeLimit = 3.0f;
    private void Start()
    {
        BP = new Dictionary<BetState, PlayCardDeal>();
       

        //制限時間
        thinkingTime = qTimeUi.GetComponent<ThinkingTime>();
        cameraControl = cameraController.GetComponent<CameraControl>();
        progress = GameStageProgress.Walking;
        nowWave = 0;
        qWave = 0;
        checkPoints = new CheckPoint[objPoint.Count];
        for(int i=0; i<objPoint.Count;i++)
        {
            checkPoints[i].Position = objPoint[i].transform.position;
            if((i+1)== objPoint.Count)
            {
                checkPoints[i].cP=CheckPointType.End;
            }
        }
        player.transform.position = checkPoints[0].Position;


        UpdateDIC();
        NextPosition();
    }
    public void StartGame()
    {
        
        
    }

    private async void Update()
    {
        switch (progress)
        {
            case GameStageProgress.Walking:
                if (!firstFlag)
                {
                    await Task.Delay(3000);
                    firstFlag = true ;
                }
                nowAimPosition = checkPoints[nowWave].Position;//次に移動する位置を選択
               f+= Time.deltaTime;
                player.transform.position = Vector3.Lerp(firstPosition, nowAimPosition,f *walkSpeed);//移動
               
                //もし移動が完了したら
                if(Vector3.Distance(player.transform.position,nowAimPosition)<=0.5f)
                {
                    await Task.Delay(1000);
                    //課題に進行度を移す
                    progress = GameStageProgress.Thinking;
                    
                    //カメラを課題用に変更
                    cameraControl.SetMiddle();

                    //制限時間のカウント開始
                    thinkingTime.StartCount();
                    
                }
                break;

            case GameStageProgress.Thinking:
                //もし制限時間に達したら
                if(thinkingTime.Count())
                {
                    //進行度を結果発表に移す
                    progress=GameStageProgress.Result;
                    //勝敗をチェックする
                    CheckResult();

                    
                }
                break;
            case GameStageProgress.Result:
                
                //ゲームクリアフラグが立っていたら
                if(GameClear&& BP[BetState.R].GetReverseComplete()&& BP[BetState.L].GetReverseComplete())
                {
                    countup += Time.deltaTime;
                    if(countup>=timeLimit)
                    {
                        countup = 0;
                        Debug.Log("WAVE:" + nowWave);
                        UpdateDIC();
                        //カメラを移動用に変更する
                        cameraControl.SetFar();

                        //エフェクトを起動
                        effect[eCount].StartClearEffect();
                        eCount++;
                        //ゲームクリアのフラグを折る
                        GameClear = false;
                        //チップをセット
                        chipGenerator.SetChip((bet.GetBetNum() * (int)magnitude) + clearPulus);
                        bet.ResetRimmit();
                        //演出用の時間を確保
                        await Task.Delay(3000);
                        //次の位置へ
                        NextPosition();

                    }

                }
                break;
            case GameStageProgress.Ending:
                nowAimPosition = checkPoints[nowWave].Position;//次に移動する位置を選択4
                f += Time.deltaTime * walkSpeed;
                player.transform.position = Vector3.Lerp(firstPosition, nowAimPosition,f);//移動
                if (Vector3.Distance(player.transform.position, nowAimPosition) <= 0.5f)
                {
                    countup += Time.deltaTime;
                    if (countup >= timeLimit)
                    {
                        //ステージリザルトへ
                        result.SetFlag(chipGenerator.GetChipNum());
                    }
                    
                }
                break;
        }
        //カメラに進行度を設定
        cameraControl.SetProgress(progress);


    }

    public void NextPosition()
    {
        progress = GameStageProgress.Walking;
        nowWave++;
        f = 0;
        firstPosition = player.transform.position;
        if (checkPoints[nowWave].cP == CheckPointType.End)
        {
            progress = GameStageProgress.Ending;
        }

    }
    public GameStageProgress GetProgress()
    {
        return progress;
    }
    void CheckResult()
    {
        if(bet.GetBetState() != BetState.N)
        {
            InitClearData(bet.GetBetState());
        }

        BP[BetState.R].ReverseCard();
        BP[BetState.L].ReverseCard();
        Debug.Log("Bet:"+bet.GetBetState()+"R/L:"+BP[BetState.R].GetResult()+"/"+ BP[BetState.L].GetResult() + "/nowwave:"+nowWave);

        qWave++;
        
    }
    void InitClearData(BetState _state )
    {
        
        GameClear = true;

        clearPulus = BP[_state].GetPulus();
        magnitude = BP[_state].GetMagnitude();
        
        bet.ResetBetState();

    }
    void UpdateDIC()
    {
        Debug.Log("Qwall2R"+qWave);
        if(qWave>=dealWave.Count)
        {
            return;
        }
        BP[BetState.R] =dealWave[qWave].dealR;
        BP[BetState.L] = dealWave[qWave].dealL;
    }
}
