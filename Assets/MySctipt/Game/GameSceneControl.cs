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
    bool GameClear = false;
    float magnitude=0;
    int clearPulus = 0;

    [SerializeField] Result result;


    private void Start()
    {
        Debug.Log(effect);
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
                nowAimPosition = checkPoints[nowWave].Position;
                player.transform.position = Vector3.Lerp(player.transform.position, nowAimPosition, Time.deltaTime * walkSpeed);
                if(Vector3.Distance(player.transform.position,nowAimPosition)<=0.5f)
                {
                    progress = GameStageProgress.Thinking;
                    cameraControl.SetMiddle();
                    thinkingTime.StartCount();
                    
                }
                break;
            case GameStageProgress.Thinking:
                if(thinkingTime.Count())
                {
                    progress=GameStageProgress.Result;
                    CheckResult();
                    cameraControl.SetFar();
                    
                }
                break;
            case GameStageProgress.Result:
                bet.GetBetState();
                
                if(GameClear)
                {
                    effect.StartClearEffect();
                    GameClear = false;
                    chipGenerator.SetChip((bet.GetBetNum()*(int)magnitude)+clearPulus);
                    await Task.Delay(3000);
                    NextPosition();
                }
                break;
            case GameStageProgress.Ending:
                result.SetFlag(chipGenerator.GetChipNum());
                break;
        }

        cameraControl.SetProgress(progress);
        Debug.Log(progress);


    }

    public void NextPosition()
    {
        progress = GameStageProgress.Walking;
        checkPoints[nowWave].NextFlag = true;
        if (nowWave+1<checkPoints.Count())
        {
            nowWave++;
        }
        else
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
        if(bet.GetBetState() == BetState.L)
        {
            if (dealL.GetResult())
            {
                GameClear = true;
                clearPulus=dealL.GetPulus();
                magnitude = dealL.GetMagnitude();
                Debug.Log("GAMECLEAR!");
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
                GameClear = true;
                clearPulus = dealL.GetPulus();
                magnitude = dealL.GetMagnitude();
                Debug.Log("GAMECLEAR!");
            }
            else
            {

                Debug.Log("GAMEOVER!");
            }
        }
        
        
    }
}
