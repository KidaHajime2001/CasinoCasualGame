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
        
        // Game1�ő�����@��`����
        Selecting,
        Betting,
        CountingDown,
        
        // Game2�͎��R�ɑ��삳����
        Thinking,

        // ����
        ShowingResult,
        Ending,
    }
    TutorialProgress progress;
    GameStageProgress normalProgress;   // �ʏ�̃Q�[���Ŏg���i��(�J�����̌����̂��߂Ɏg�p)

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
        finger.SetActive(false);

        // �{�^���̍��W��z��ɋL�^
        for (int i = 0; i < this.selectButtons.Count; ++i)
        {
            this.selectButtonLocations.Add(selectButtons[i].transform.position);
        }
        for (int i = 0; i < this.betButtons.Count; ++i) 
        {
            this.betButtonLocations.Add(betButtons[i].transform.position);
        }

        // �����R�C������111��

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

                        this.finger.SetActive(true);
                        this.deltaTimeCounter = 0.0f;

                        // �w�̈ړ�����n��
                        this.tutoFinger.SetLocations(this.selectButtonLocations);
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
                // �ړ����łȂ����
                if(!this.tutoFinger.IsMoving())
                {
                    switch (this.fingerStepCount)
                    {
                        case 0:
                            // �i�K��1���₵�Ĉړ�������
                            ++this.fingerStepCount;
                            this.tutoFinger.MoveToNextStep();
                            break;

                        case 1:
                            // �i�K��1���₵�Ĉړ�������
                            ++this.fingerStepCount;
                            this.tutoFinger.MoveToPreviousStep();
                            break;

                        case 2:
                            // ���̃{�^����������悤�ɂ��āA�{�^���������ꂽ��x�b�g�ɐi��
                            this.selectButtons[0].interactable = true;
                            if (this.pressedButtonCount == 1)
                            {
                                await Task.Delay(1000);
                                // ���{�^���������Ȃ�����
                                this.selectButtons[0].interactable = false;
                                // �w�̈ړ�����n��
                                this.tutoFinger.SetLocations(this.betButtonLocations);
                                this.progress = TutorialProgress.Betting;
                                // �g�p�����ϐ��̏�����
                                this.deltaTimeCounter = 0.0f;
                                this.fingerStepCount = 0;
                                this.pressedButtonCount = 0;
                            }
                            break;
                    }
                }
                break;

            case TutorialProgress.Betting:

                // �ړ����łȂ����
                if (!this.tutoFinger.IsMoving())
                {
                    switch (this.fingerStepCount)
                    {
                        case 0:
                            // bet1��������悤�ɂ��āA�{�^���������ꂽ�玟�ɂ�����
                            this.betButtons[0].interactable = true;
                            if(this.pressedButtonCount == 1)
                            {
                                ++this.fingerStepCount;
                                // bet1�������Ȃ�����
                                this.betButtons[0].interactable = false;
                                // �w�̈ړ�����n��
                                this.tutoFinger.MoveToNextStep();
                            }
                            break;
                        case 1:
                            // bet10��������悤�ɂ��āA�{�^���������ꂽ�玟�ɂ�����
                            this.betButtons[1].interactable = true;
                            if (this.pressedButtonCount == 2)
                            {
                                ++this.fingerStepCount;
                                // bet1�������Ȃ�����
                                this.betButtons[1].interactable = false;
                                // �w�̈ړ�����n��
                                this.tutoFinger.MoveToNextStep();
                            }
                            break;
                        case 2:
                            // bet100��������悤�ɂ��āA�{�^���������ꂽ�玟�ɂ�����
                            this.betButtons[2].interactable = true;
                            if (this.pressedButtonCount == 3)
                            {
                                ++this.fingerStepCount;
                                // bet1�������Ȃ�����
                                this.betButtons[2].interactable = false;
                                // �w�̈ړ�����n��
                                this.tutoFinger.MoveToNextStep();
                            }
                            break;
                    }
                }

                // ���Ƀx�b�g����{�^����L���ɂ���
                for (int i = 0; i < this.betButtons.Count; ++i)
                {
                    if(this.betButtons[i].interactable == true)
                    {
                        // �L���ȃ{�^���������̏ꍇ�͏I��
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
                
                
                // 1, 10, 100 ���ꂼ��{�^�����������A111���q��������


                break;

            case TutorialProgress.CountingDown:
                // �J�E���g�_�E����������

                break;

            case TutorialProgress.Thinking:

                break;

            case TutorialProgress.ShowingResult:
                // ���ʂ�������(�v���X�ɂȂ�)

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
}
