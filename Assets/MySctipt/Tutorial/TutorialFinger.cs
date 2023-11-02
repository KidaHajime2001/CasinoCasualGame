using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialFinger : MonoBehaviour
{
    [SerializeField] Vector3 startPos;
    [SerializeField] Vector3 aimPos;
    [SerializeField] bool isMoving = false;
    [SerializeField] float speed;
    [SerializeField] bool isBack = false;

    List<Vector3> locations;
    int step = 0;
    float deltaTimeCounter = 0.0f;

    void Start()
    {
        this.locations = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.isMoving)
        {
            this.Move();
        }
    }

    void Move()
    {
        this.deltaTimeCounter += Time.deltaTime;

        if(this.isBack)
        {
            this.startPos = this.locations[this.step + 1];
            this.aimPos = this.locations[this.step];
        }
        else
        {
            this.startPos = this.locations[this.step - 1];
            this.aimPos = this.locations[this.step];
        }

        // ���݂̏ꏊ����ړ�
        this.transform.position = Vector3.Lerp(startPos, aimPos, this.deltaTimeCounter * this.speed);

        // �ړ�������������A�ړ�������Ԃɂ���B
        if(Vector3.Distance(this.transform.position, this.locations[this.step]) <= 0.5f)
        {
            this.isMoving = false;
        }
    }

    /// <summary>
    /// ���̏ꏊ�ֈړ�����w�����o���B
    /// </summary>
    public void MoveToNextStep()
    {
        // �z��̖����Ȃ�ړ��s��
        if(this.step == this.locations.Count - 1)
        {
            return;
        }
        // ���̃X�e�b�v�ɐi�߂�
        ++this.step;
        // ���ɐi�ނ悤�ɂ���
        this.isBack = false;
        // �ړ���Ԃɂ���B
        this.isMoving = true;
        // �f���^�^�C�������Z�b�g
        this.deltaTimeCounter = 0.0f;
    }

    /// <summary>
    /// �O�̏ꏊ�ֈړ�����w�����o���B
    /// </summary>
    public void MoveToPreviousStep()
    {
        // �z��̐擪�Ȃ�ړ��s��
        if (this.step == 0)
        {
            return;
        }
        // �O�̃X�e�b�v�ɖ߂�
        --this.step;
        // �O�ɖ߂�悤�ɂ���
        this.isBack = true;
        // �ړ���Ԃɂ���
        this.isMoving = true;
        // �f���^�^�C�������Z�b�g
        this.deltaTimeCounter = 0.0f;
    }

    public void SetLocations(List<Vector3> _locations)
    {
        // �����̃f�[�^��locations���X�V
        this.locations.Clear();
        this.locations = new List<Vector3>(_locations);

        // locations[0]�̈ʒu�Ɉړ�
        this.transform.position = locations[0];
    }

    public bool IsMoving()
    {
        return this.isMoving;
    }
}