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
    //�J�E���g�A�b�v
    private float countup = 0.0f;
    private float timeLimit = 3.0f;
    private void Start()
    {
        BP = new Dictionary<BetState, PlayCardDeal>();
       

        //��������
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
                nowAimPosition = checkPoints[nowWave].Position;//���Ɉړ�����ʒu��I��
               f+= Time.deltaTime;
                player.transform.position = Vector3.Lerp(firstPosition, nowAimPosition,f *walkSpeed);//�ړ�
               
                //�����ړ�������������
                if(Vector3.Distance(player.transform.position,nowAimPosition)<=0.5f)
                {
                    await Task.Delay(1000);
                    //�ۑ�ɐi�s�x���ڂ�
                    progress = GameStageProgress.Thinking;
                    
                    //�J�������ۑ�p�ɕύX
                    cameraControl.SetMiddle();

                    //�������Ԃ̃J�E���g�J�n
                    thinkingTime.StartCount();
                    
                }
                break;

            case GameStageProgress.Thinking:
                //�����������ԂɒB������
                if(thinkingTime.Count())
                {
                    //�i�s�x�����ʔ��\�Ɉڂ�
                    progress=GameStageProgress.Result;
                    //���s���`�F�b�N����
                    CheckResult();

                    
                }
                break;
            case GameStageProgress.Result:
                
                //�Q�[���N���A�t���O�������Ă�����
                if(GameClear&& BP[BetState.R].GetReverseComplete()&& BP[BetState.L].GetReverseComplete())
                {
                    countup += Time.deltaTime;
                    if(countup>=timeLimit)
                    {
                        countup = 0;
                        Debug.Log("WAVE:" + nowWave);
                        UpdateDIC();
                        //�J�������ړ��p�ɕύX����
                        cameraControl.SetFar();

                        //�G�t�F�N�g���N��
                        effect[eCount].StartClearEffect();
                        eCount++;
                        //�Q�[���N���A�̃t���O��܂�
                        GameClear = false;
                        //�`�b�v���Z�b�g
                        chipGenerator.SetChip((bet.GetBetNum() * (int)magnitude) + clearPulus);
                        bet.ResetRimmit();
                        //���o�p�̎��Ԃ��m��
                        await Task.Delay(3000);
                        //���̈ʒu��
                        NextPosition();

                    }

                }
                break;
            case GameStageProgress.Ending:
                nowAimPosition = checkPoints[nowWave].Position;//���Ɉړ�����ʒu��I��4
                f += Time.deltaTime * walkSpeed;
                player.transform.position = Vector3.Lerp(firstPosition, nowAimPosition,f);//�ړ�
                if (Vector3.Distance(player.transform.position, nowAimPosition) <= 0.5f)
                {
                    countup += Time.deltaTime;
                    if (countup >= timeLimit)
                    {
                        //�X�e�[�W���U���g��
                        result.SetFlag(chipGenerator.GetChipNum());
                    }
                    
                }
                break;
        }
        //�J�����ɐi�s�x��ݒ�
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
