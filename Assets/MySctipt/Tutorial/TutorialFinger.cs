using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialFinger : MonoBehaviour
{
    [SerializeField] List<Vector3> locations;
    [SerializeField] int step = 0;
    [SerializeField] bool isMoving = false;
    [SerializeField] float speed;

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

        // ���݂̏ꏊ����ړ�
        this.transform.position = Vector3.Lerp(this.locations[this.step - 1], this.locations[this.step], this.deltaTimeCounter * this.speed);

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
        if(!this.CanProceedToNextStep())
        {
            return;
        }
        // �z��̖����Ȃ�ړ��s��
        if(this.step == this.locations.Count - 1)
        {
            return;
        }
        // ���̃X�e�b�v�ɐi�߂�
        ++this.step;
        // �ړ���Ԃɂ���B
        this.isMoving = true;
    }

    public void MoveToPreviousStep()
    {
        if(!this.CanProceedToNextStep())
        {
            return;
        }
        // �z��̐擪�Ȃ�ړ��s��
        if (this.step == 0)
        {
            return;
        }
        // �O�̃X�e�b�v�ɖ߂�
        --this.step;
        // �ړ���Ԃɂ���
        this.isMoving = true;
    }

    /// <summary>
    /// ���̒i�K�ɐi�߂Ă������H�O�̒i�K�ɖ߂��Ă������H���ʂ̃`�F�b�N
    /// </summary>
    bool CanProceedToNextStep()
    {
        // �ړ����Ȃ�X�V�s��
        if(this.isMoving)
        {
            return false;
        }

        return true;
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