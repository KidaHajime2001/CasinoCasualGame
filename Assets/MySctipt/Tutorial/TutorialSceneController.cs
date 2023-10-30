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
        // �v���C���[�̈ʒu���J�n�ʒu�ɐݒ�
        player.transform.position = eventLocations[0].transform.position;
        // ���E�̑I���{�^���𖳌���
        leftButton.interactable = false;
        rightButton.interactable = false;
        // �w���\��
        finger.SetActive(false);
        finger.transform.position = leftButton.transform.position;
        
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
                    await Task.Delay(1000);
                    // �i�������Ɉڂ�
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
                    // �J�������ۑ�悤�ɕύX
                    this.camControl.SetMiddle();
                }
                break;

            case TutorialProgress.Selecting:
                // ��Game1
                // �wUI�̉�������
                if (!this.fingerRoundTripFlag)
                {
                    // ���̃{�^���̈ʒu�ɂ�����
                    if (Vector3.Distance(this.finger.transform.position, this.leftButton.transform.position) <= 0.5f)
                    {
                        this.fingerAimPos = this.rightButton.transform.position;
                    }
                    // �E�̃{�^���̈ʒu�ɂ�����
                    else if (Vector3.Distance(this.finger.transform.position, this.rightButton.transform.position) <= 0.5f)
                    {
                        this.fingerAimPos = this.leftButton.transform.position;
                    }
                    // �ړ�����
                    // �ڕW���E�̃{�^���Ȃ�
                    if (this.fingerAimPos == this.rightButton.transform.position)
                    {
                        this.deltaTimeCounter += Time.deltaTime;
                    }
                    else if (this.fingerAimPos == this.leftButton.transform.position)
                    {
                        this.deltaTimeCounter -= Time.deltaTime;
                    }
                    this.finger.transform.position = Vector3.Lerp(this.leftButton.transform.position, this.rightButton.transform.position, this.deltaTimeCounter * this.fingerSpeed);

                    // �������I�������
                    if(Vector3.Distance(this.finger.transform.position, this.leftButton.transform.position) <= 0.5f
                        && Vector3.Distance(this.fingerAimPos, this.leftButton.transform.position) <= 0.5f)
                    {
                        this.fingerRoundTripFlag = true;
                        this.deltaTimeCounter = 0.0f;
                    }
                }
                else
                {
                    // ���̃{�^����������悤�ɂ���
                    this.leftButton.interactable = true;
                    // ���̃{�^���������悤�Ɋg��k��������
                    //this.deltaTimeCounter += Time.deltaTime;

                    // ���̃{�^���������ꂽ��x�b�g�ɐi��
                    if(this.pressedLeftButton)
                    {
                        await Task.Delay(1000);
                        this.progress = TutorialProgress.Betting;
                        this.deltaTimeCounter = 0.0f;
                    }
                }

                break;

            case TutorialProgress.Betting:
                // 1, 10, 100 ���ꂼ��{�^�����������A111���q��������
                // 1�̈ʒu�Ɏw���ړ�
                this.finger.transform.position = this.bet1.transform.position;

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

    public void PressedLeftButton()
    {
        // �i�����`���[�g���A���̑I����
        // ����������A�L���ɂȂ��Ă�Ƃ��ɉ������ꍇ
        if(this.fingerRoundTripFlag && !this.pressedLeftButton)
        {
            this.pressedLeftButton = true;
        }
    }
}
