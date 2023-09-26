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
        //��������
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
                nowAimPosition = checkPoints[nowWave].Position;//���Ɉړ�����ʒu��I��
                player.transform.position = Vector3.Lerp(player.transform.position, nowAimPosition, Time.deltaTime * walkSpeed);//�ړ�

                //�����ړ�������������
                if(Vector3.Distance(player.transform.position,nowAimPosition)<=0.5f)
                {
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
                    //�J�������ړ��p�ɕύX����
                    cameraControl.SetFar();
                    
                }
                break;
            case GameStageProgress.Result:
                
                //�Q�[���N���A�t���O�������Ă�����
                if(GameClear)
                {
                    //�G�t�F�N�g���N��
                    effect.StartClearEffect();
                    //�Q�[���N���A�̃t���O��܂�
                    GameClear = false;
                    //�`�b�v���Z�b�g
                    chipGenerator.SetChip((bet.GetBetNum()*(int)magnitude)+clearPulus);
                    //���o�p�̎��Ԃ��m��
                    await Task.Delay(3000);
                    //���̈ʒu��
                    NextPosition();
                }
                break;
            case GameStageProgress.Ending:
                //�X�e�[�W���U���g��
                result.SetFlag(chipGenerator.GetChipNum());
                break;
        }
        //�J�����ɐi�s�x��ݒ�
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
