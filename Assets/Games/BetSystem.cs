using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BetSystem : MonoBehaviour
{
    [SerializeField] List<Button> lrSelectButtons;
    [SerializeField] List<Button> betButtons;
    [SerializeField] Chip playerChip;
    [SerializeField] int bet = 0;
    
    [Tooltip("���E�ǂ���̃Q�[����Bet����̂�?")]
    [SerializeField] bool isSelectedLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // �x�b�g�̋����~�肽����s�J�n

        // 1.���E�ǂ���ɓq����̂���I�΂���

        // 2.�x�b�g������(�{�^����������悤�ɂ���B)

        // 3.�{�^���������ꂽ��A�v���C���[�̃`�b�v��������邩�����Ȃ���A�x�b�g�̗ʂ�ύX

        // 4.���Ԃ��o�߂�����f�B�[���[����I���w��������
    }

    public void SetSelectLeft(bool _isSelectLeft)
    {
        isSelectedLeft = _isSelectLeft;
    }

    public bool IsSelectedLeft()
    {
        return this.isSelectedLeft;
    }


    public int GetBet()
    {
        return this.bet;
    }

    public void BetReset()
    {
        this.bet = 0;
    }

    public void Bet(int _value)
    {
        // �x�b�g���[�g�ɁA�`�b�v������Ă����
        if(this.playerChip.GetComponent<Chip>().Get() >= this.bet + _value)
        {
            this.bet += _value;
        }
    }
}