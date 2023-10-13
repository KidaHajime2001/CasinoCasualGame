using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Result : MonoBehaviour
{
    [SerializeField] List<GameObject> star;

    bool[] rotSet = new bool[3];
    private float _countTime;
    private bool _startRotate;

    private Quaternion _startRotation;
    private Quaternion _endRotation;
    [SerializeField] TextMeshProUGUI uiText;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        _countTime = 0f;
        for (int i = 0; i < 3; i++)
        {
            rotSet[i] = false;
            
        }
        foreach (var stars in star)
        {
            stars.SetActive(false);
            stars.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if (rotSet[i])
            {
                RotationStar(star[i]);
            }
        }
    }
    public void SetFlag(int _score)
    {
        gameObject.SetActive(true);
        uiText.text = _score.ToString();
        if(_score>=100)
        {
            rotSet[0] = true;
            star[0].SetActive(true);
        }
        if (_score >= 1000)
        {
            rotSet[1] = true;

            star[1].SetActive(true);
        }
        if (_score >= 5000)
        {
            rotSet[2] = true;

            star[2].SetActive(true);
        }
        
    }
    void RotationStar(GameObject _star)
    {

        _countTime = Mathf.Clamp(_countTime + Time.deltaTime, 0f, 5.0f);
        float rate = _countTime / 5.0f;

        _star.transform.localScale = Vector3.Lerp(_star.transform.localScale,new Vector3(2.0f, 2.0f, 2.0f),rate);
        _star.transform.rotation = Quaternion.Lerp(_star.transform.rotation, Quaternion.Euler(360,0,0), rate);

        if (rate >= 1f)
        {
            _startRotate = false;
        }
    }
}
