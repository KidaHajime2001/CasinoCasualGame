using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Trail : MonoBehaviour
{
    [SerializeField] Vector3 speed;
    [SerializeField] float lifetime;

    [SerializeField] float maxHeight;
    [SerializeField] float aimHeight;
    [SerializeField] float timeToPeak;
    [SerializeField] float timeToAim;

    float deltaTimeCounter = 0.0f;
    float firstPosY;
    bool isGeneratedOnStart = false;

    // Update is called once per frame
    void Update()
    {
        // ç≈èâÇ…í ÇÈ
        if(deltaTimeCounter <= 0.0f)
        {
            firstPosY = this.transform.position.y;

            if (this.transform.position.y >= this.aimHeight)
            {
                isGeneratedOnStart = true;
            }
        }

        this.deltaTimeCounter += Time.deltaTime;
        if(this.deltaTimeCounter <= this.lifetime)
        {
            Move();
            if (!isGeneratedOnStart)
            {
                MoveUpAndDown();
            }
        }
        else
        {
            Init();
        }
    }

    void Move()
    {
        this.transform.position += speed * Time.deltaTime;
    }

    void MoveUpAndDown()
    {
        Vector3 temp = this.transform.position;
        // è„è∏èàóù
        if (this.deltaTimeCounter <= this.timeToPeak)
        {
            temp.y = Mathf.Lerp(this.firstPosY, this.maxHeight, (this.deltaTimeCounter / this.timeToPeak));
        }
        // â∫ç~èàóù
        else if(this.deltaTimeCounter <= this.timeToPeak + this.timeToAim)
        {
            temp.y = Mathf.Lerp(this.maxHeight, this.aimHeight, ((this.deltaTimeCounter - this.timeToPeak) / this.timeToAim));
        }
        else
        {
            temp.y = this.aimHeight;
        }
        this.transform.position = temp;
    }

    void Init()
    {
        this.deltaTimeCounter = 0.0f;
        this.gameObject.SetActive(false);
        this.isGeneratedOnStart = false;
    }
}
