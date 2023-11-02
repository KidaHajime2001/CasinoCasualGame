using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class TutorialSceneController : MonoBehaviour
{
    enum TutorialProgress
    {
        Walking,
        
        // Game1�ő�����@��`����
        Selecting,
        Betting,
        CountingDown,
        
        // Game2�͎��R�ɑ��삳����
        Thinking,

        // ����
        ShowingResult,
        Ending
    }
    [SerializeField] TutorialProgress progress;
    GameStageProgress normalProgress;   // �ʏ�̃Q�[���Ŏg���i��(�J�����̌����̂��߂Ɏg�p)
    //public enum BetState
    //{
    //    Right,
    //    Left,
    //    Non
    //}
    //public struct DealWave
    //{
    //    public PlayCardDeal dealL;
    //    public PlayCardDeal dealR;
    //}
    //Dictionary<BetState, PlayCardDeal> BP;
    //[SerializeField] List<DealWave> dealWave;

    // �L�����N�^�[�̈ړ�
    float deltaTimeCounter = 0.0f;
    bool hasWalked = false;
    Vector3 nowPos;
    Vector3 aimPos;

    [SerializeField] GameObject player;
    [SerializeField] float walkSpeed;

    // �`���[�g���A���̐i�s
    bool isTutorial = true;
    int pressedButtonCount = 0;
    List<Vector3> selectButtonLocations;
    List<Vector3> betButtonLocations;
    int fingerStepCount = 0;

    [SerializeField] List<GameObject> eventLocations;
    [SerializeField] CameraControl camControl;
    [SerializeField] List<Button> selectButtons;
    [SerializeField] List<Button> betButtons;
    [SerializeField] TutorialFinger finger;
    [SerializeField] CountDownTimer countDownTimer;
    [SerializeField] Bet bet;

    int currentLocation = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.selectButtonLocations = new List<Vector3>();
        this.betButtonLocations = new List<Vector3>();

        // �v���C���[�̈ʒu���J�n�ʒu�ɐݒ�
        player.transform.position = eventLocations[0].transform.position;
        // ���E�̑I���{�^���𖳌���
        for (int i = 0; i < this.selectButtons.Count; ++i)
        {
            this.selectButtons[i].interactable = false;
        }
        // �x�b�g�p�̃{�^���𖳌���
        for (int i = 0; i < this.betButtons.Count; ++i)
        {
            this.betButtons[i].interactable = false;
        }
        // �w���\��
        finger.gameObject.SetActive(false);

        // �{�^���̍��W��z��ɋL�^
        for (int i = 0; i < this.selectButtons.Count; ++i)
        {
            this.selectButtonLocations.Add(selectButtons[i].transform.position);
        }
        for (int i = 0; i < this.betButtons.Count; ++i) 
        {
            this.betButtonLocations.Add(betButtons[i].transform.position);
        }

        //BP = new Dictionary<BetState, PlayCardDeal>();

        // �����R�C�������̐ݒ�(111��)

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
                // �ړ��ڕW��ݒ�
                this.aimPos = this.eventLocations[this.currentLocation].transform.position;
                this.deltaTimeCounter += Time.deltaTime;
                // �ړ�����
                this.player.transform.position = Vector3.Lerp(this.nowPos, this.aimPos, this.deltaTimeCounter * this.walkSpeed);
                // �ړ������������ꍇ
                if (Vector3.Distance(this.player.transform.position, this.aimPos) <= 0.5f)
                {
                    // �i�������Ɉڂ�
                    if (isTutorial)
                    {
                        this.progress = TutorialProgress.Selecting;
                        this.normalProgress = GameStageProgress.Thinking;

                        this.finger.gameObject.SetActive(true);
                        this.deltaTimeCounter = 0.0f;

                        // �w�̈ړ�����n��
                        this.finger.SetLocations(this.selectButtonLocations);
                    }
                    else
                    {
                        this.progress = TutorialProgress.Thinking;
                        this.normalProgress = GameStageProgress.Thinking;
                    }
                    // �J�������ۑ�悤�ɕύX
                    this.camControl.SetMiddle();
                }
                break;

            case TutorialProgress.Selecting:
                // ��Game1

                // ���E�̑I���{�^���ƍ��E�̃Q�[���ȊO���Â�����B
                // ���E�ǂ����I�����Ă��邩�킩��₷������B

                // �ړ����łȂ����
                if(!this.finger.IsMoving())
                {
                    switch (this.fingerStepCount)
                    {
                        case 0:
                            // �i�K��1���₵�Ĉړ�������
                            ++this.fingerStepCount;
                            this.finger.MoveToNextStep();
                            break;

                        case 1:
                            // �i�K��1���₵�Ĉړ�������
                            ++this.fingerStepCount;
                            this.finger.MoveToPreviousStep();
                            break;

                        case 2:
                            // ���̃{�^����������悤�ɂ��āA�{�^���������ꂽ��x�b�g�ɐi��
                            this.selectButtons[0].interactable = true;
                            if (this.pressedButtonCount == 1)
                            {
                                // ���{�^���������Ȃ�����
                                this.selectButtons[0].interactable = false;
                                // �w�̈ړ�����n��
                                this.finger.SetLocations(this.betButtonLocations);
                                this.progress = TutorialProgress.Betting;
                                // �g�p�����ϐ��̏�����
                                this.fingerStepCount = 0;
                                this.pressedButtonCount = 0;
                            }
                            break;
                    }
                }
                break;

            case TutorialProgress.Betting:

                // �x�b�g�{�^���Ɠq���Ă�z�A�c��̃R�C�����̕����ȊO���Â�����B

                // �ړ����łȂ����
                if (!this.finger.IsMoving())
                {
                    switch (this.fingerStepCount)
                    {
                        // 0~3��2��i���2��߂�
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
                            // bet1��������悤�ɂ��āA�{�^���������ꂽ�玟�ɂ�����
                            this.betButtons[0].interactable = true;
                            if (this.pressedButtonCount == 1)
                            {
                                ++this.fingerStepCount;
                                // bet1�������Ȃ�����
                                this.betButtons[0].interactable = false;
                                // �w�̈ړ�����n��
                                this.finger.MoveToNextStep();
                            }
                            break;

                        case 5:
                            // bet10��������悤�ɂ��āA�{�^���������ꂽ�玟�ɂ�����
                            this.betButtons[1].interactable = true;
                            if (this.pressedButtonCount == 2)
                            {
                                ++this.fingerStepCount;
                                // bet1�������Ȃ�����
                                this.betButtons[1].interactable = false;
                                // �w�̈ړ�����n��
                                this.finger.MoveToNextStep();
                            }
                            break;

                        case 6:
                            // bet100��������悤�ɂ��āA�{�^���������ꂽ�玟�ɂ�����
                            this.betButtons[2].interactable = true;
                            if (this.pressedButtonCount == 3)
                            {
                                ++this.fingerStepCount;
                                // bet1�������Ȃ�����
                                this.betButtons[2].interactable = false;
                                // �w�̈ړ�����n��
                                this.finger.MoveToNextStep();

                            }
                            break;

                        case 7:
                            // �w���\��
                            this.finger.gameObject.SetActive(false);
                            // �g�p�����ϐ��̏�����
                            this.fingerStepCount = 0;
                            this.pressedButtonCount = 0;

                            // �J�E���g�_�E���̐ݒ�
                            this.progress = TutorialProgress.CountingDown;
                            this.countDownTimer.SetTimer(5.0f);
                            break;
                    }
                }

                break;

            case TutorialProgress.CountingDown:
                // �J�E���g�_�E����������

                // �J�E���g�_�E�����I��������
                if (!this.countDownTimer.IsCountingDown())
                {
                    this.countDownTimer.InitDisplay();
                    //�i�s�x�����ʔ��\�Ɉڂ�
                    this.progress = TutorialProgress.ShowingResult;
                    normalProgress = GameStageProgress.Result;
                    //���s���`�F�b�N����
                    // CheckResult();
                }

                break;

            case TutorialProgress.Thinking:

                break;

            case TutorialProgress.ShowingResult:
                ////�Q�[���N���A�t���O�������Ă�����
                //if (GameClear && BP[BetState.R].GetReverseComplete() && BP[BetState.L].GetReverseComplete())
                //{
                //    countup += Time.deltaTime;
                //    if (countup >= timeLimit)
                //    {
                //        countup = 0;
                //        Debug.Log("WAVE:" + nowWave);
                //        UpdateDIC();
                //        //�J�������ړ��p�ɕύX����
                //        cameraControl.SetFar();

                //        //�G�t�F�N�g���N��
                //        effect[eCount].StartClearEffect();
                //        eCount++;
                //        //�Q�[���N���A�̃t���O��܂�
                //        GameClear = false;
                //        //�`�b�v���Z�b�g
                //        chipGenerator.SetChip((bet.GetBetNum() * (int)magnitude) + clearPulus);
                //        bet.ResetRimmit();
                //        //���o�p�̎��Ԃ��m��
                //        await Task.Delay(3000);
                //        //���̈ʒu��
                //        NextPosition();

                //    }

                //}

                break;

            case TutorialProgress.Ending:

                break;
        }

        // �J�����ɐi����ݒ�
        camControl.SetProgress(normalProgress);


        // ��Game2�t�F�[�Y
        // ���R�ɑI��
        // ���R�Ƀx�b�g
        // �J�E���g�_�E��
        // ���ʂ�������(�v���X���}�C�i�X���L��)

        // �����U���g
    }

    /// <summary>
    /// �ڕW�����̖ڕW�֕ύX���邽�߂̊֐�
    /// </summary>
    void ToNextProgress()
    {
        // �i�����ړ��ɕύX
        this.progress = TutorialProgress.Walking;
        this.normalProgress = GameStageProgress.Walking;
        // ���̖ڕW�Ɍ����Ēl�̏������Ɛݒ�
        this.deltaTimeCounter = 0.0f;
        this.nowPos = this.player.transform.position;
        ++this.currentLocation;
        // �����S�[���֓��B���Ă����ꍇ�A�i�����Q�[���I���ɂ���B
        if (this.eventLocations.Count-1 == this.currentLocation)
        {
            this.progress = TutorialProgress.Ending;
        }
    }

    public void PressedButton()
    {
        ++this.pressedButtonCount;
    }

    //void CheckResult()
    //{
    //    if (bet.GetBetState() == BetState.Left)
    //    {
    //        InitClearData(bet.GetBetState());

    //    }
    //    else if (bet.GetBetState() == BetState.Right)
    //    {
    //        InitClearData(bet.GetBetState());
    //    }

    //    //BP[BetState.R].ReverseCard();
    //    //BP[BetState.L].ReverseCard();
    //    //Debug.Log("Bet:" + bet.GetBetState() + "R/L:" + BP[BetState.R].GetResult() + "/" + BP[BetState.L].GetResult() + "/nowwave:" + nowWave);
    //}

    //void InitClearData(BetState _state)
    //{
    //    clearPulus = BP[_state].GetPulus();
    //    magnitude = BP[_state].GetMagnitude();

    //    bet.ResetBetState();

    //}
}
