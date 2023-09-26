using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ThinkingTime : MonoBehaviour
{
    float t=0;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI text;
    private const float SLIDER_MAX_VALUE=30.0f;

    bool startFlag;
    private void Start()
    {
        slider.maxValue = SLIDER_MAX_VALUE;
        startFlag = false;
    }
    private void Update()
    {
        if(startFlag)
        {
            slider.value += Time.deltaTime;
            text.text = ((int)slider.value).ToString();
        }
        
    }

    public bool Count()
    {
        if(slider.value>=SLIDER_MAX_VALUE)
        {
            startFlag = false;
            slider.value = 0;
            return true;
        }
        return false;
    }
    public void StartCount()
    {
        startFlag = true;

    }
}
