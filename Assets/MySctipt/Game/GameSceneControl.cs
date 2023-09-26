using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using  System.Threading.Tasks;
public class GameSceneControl : MonoBehaviour
{
    struct CheckPoint
    {
        public Vector3 Position;
        public bool NextFlag;
        public void Init(Vector3 vec)
        {
            Position = vec;
            NextFlag = false;
        }
    }
    GameStageProgress progress;
    [SerializeField] List<GameObject> objPoint;
    [SerializeField] GameObject cameraController;
    CameraControl cameraControl;

    private CheckPoint[] checkPoints;
    private int nowWave;
    [SerializeField] private GameObject player;
    private Vector3 nowAimPosition;
    [SerializeField]
    private float walkSpeed;
    
    [SerializeField] GameObject qTimeUi;
    ThinkingTime thinkingTime;

    [SerializeField] Bet bet;
    [SerializeField] PlayCardDeal dealL;
    [SerializeField] PlayCardDeal dealR;
    [SerializeField] ChipGenerator chipGenerator;

    [SerializeField] ClearEffect effect;
    [SerializeField] GameObject posE1;
    [SerializeField] GameObject posE2;

    bool GameClear = false;
    float magnitude=0;
    int clearPulus = 0;

    [SerializeField] Result result;

    bool firstFlag = false;
    private void Start()
    {
        //制限時間
        thinkingTime = qTimeUi.GetComponent<ThinkingTime>();
        cameraControl = cameraController.GetComponent<CameraControl>();
        progress = GameStageProgress.Walking;
        nowWave = 0;
        checkPoints = new CheckPoint[objPoint.Count];
        for(int i=0; i<objPoint.Count;i++)
        {
            checkPoints[i].Position = objPoint[i].transform.position;
        }
        player.transform.position = checkPoints[0].Position;
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
                player.transform.position = Vector3.Lerp(player.transform.position, nowAimPosition, Time.deltaTime * walkSpeed);//移動

                //もし移動が完了したら
                if(Vector3.Distance(player.transform.position,nowAimPosition)<=0.5f)
                {
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
                    //カメラを移動用に変更する
                    cameraControl.SetFar();
                    
                }
                break;
            case GameStageProgress.Result:
                
                //ゲームクリアフラグが立っていたら
                if(GameClear)
                {
                    //エフェクトを起動
                    effect.StartClearEffect();
                    //ゲームクリアのフラグを折る
                    GameClear = false;
                    //チップをセット
                    chipGenerator.SetChip((bet.GetBetNum()*(int)magnitude)+clearPulus);
                    //演出用の時間を確保
                    await Task.Delay(3000);
                    //次の位置へ
                    NextPosition();
                }
                break;
            case GameStageProgress.Ending:
                //ステージリザルトへ
                result.SetFlag(chipGenerator.GetChipNum());
                break;
        }
        //カメラに進行度を設定
        cameraControl.SetProgress(progress);


    }

    public void NextPosition()
    {
        progress = GameStageProgress.Walking;
        checkPoints[nowWave].NextFlag = true;
        if (nowWave<checkPoints.Count())
        {
            nowWave++;
        }
        else
        {
            progress = GameStageProgress.Ending;
            Debug.Log("Ending");
        }
        
    }
    public GameStageProgress GetProgress()
    {
        return progress;
    }
    void CheckResult()
    {
        if(bet.GetBetState() == BetState.L)
        {
            if (dealL.GetResult())
            {
                InitClearData();
            }
            else
            {
                Debug.Log("GAMEOVER!");
            }
        }
        else if(bet.GetBetState() == BetState.R)
        {
            if (dealR.GetResult())
            {
                InitClearData();
            }
            else
            {

                Debug.Log("GAMEOVER!");
            }
        }
        
        
    }
    void InitClearData()
    {
        GameClear = true;
        clearPulus = dealL.GetPulus();
        magnitude = dealL.GetMagnitude();
        bet.ResetBetState();
        Debug.Log("GAMECLEAR!:"+nowWave+":"+ checkPoints.Count());
    }
}
