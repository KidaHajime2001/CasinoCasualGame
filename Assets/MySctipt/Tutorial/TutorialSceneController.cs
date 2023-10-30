using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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

    [SerializeField] GameObject player;
    [SerializeField] float walkSpeed;
    [SerializeField] List<GameObject> eventLocations;
    [SerializeField] CameraControl camControl;

    int currentLocation = 0;


    // Start is called before the first frame update
    void Start()
    {
        // ��������
        player.transform.position = eventLocations[0].transform.position;
        // �����R�C������111��

        this.ToNextProgress();
    }

    // Update is called once per frame
    async void Update()
    {
        switch(this.progress)
        {
            case TutorialProgress.Walking:
                if(!this.hasWalked)
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
                if(Vector3.Distance(this.player.transform.position, this.aimPos) <= 0.5f)
                {
                    await Task.Delay(1000);
                    // �i�������Ɉڂ�
                    if(isTutorial)
                    {
                        this.progress = TutorialProgress.Selecting;
                        this.normalProgress = GameStageProgress.Thinking;
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
                // ����I��������
                // 1, 10, 100 ���ꂼ��{�^�����������A111���q��������
                // �J�E���g�_�E����������
                // ���ʂ�������(�v���X�ɂȂ�)

                break;

            case TutorialProgress.Betting:

            break;

            case TutorialProgress.CountingDown:

            break;

            case TutorialProgress.Thinking:

            break;

            case TutorialProgress.ShowingResult:

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
}
