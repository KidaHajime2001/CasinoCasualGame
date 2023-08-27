using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCamera; //カメラ


    [SerializeField]
    private GameObject neutralCamera; //第一のカメラ
    private Vector3 neutralCameraPos;  //第一カメラのポジション
    private Quaternion neutralCameraAngle;//第一カメラのアングル

    [SerializeField]
    private GameObject questionCamera; //第二のカメラ
    private Vector3 questionCameraPos;  //第二カメラのポジション
    private Quaternion questionCameraAngle;//第二カメラのアングル


    private Vector3 currentPosition, targetPosition;
    private Quaternion currentAngle,targetAngle;

    float smoothSpeed = 3.0f;

    bool nowFlag = false;//nertral=false;

    // Start is called before the first frame update
    void Start()
    {

        //カメラを設定
        neutralCameraPos = neutralCamera.transform.position;
        neutralCameraAngle = neutralCamera.transform.rotation;
        questionCameraPos = questionCamera.transform.position;
        questionCameraAngle = questionCamera.transform.rotation;

        currentPosition = neutralCameraPos;
        targetPosition = questionCameraPos;
        currentAngle = neutralCameraAngle;
        targetAngle = questionCameraAngle;



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            ChangeTargetPosition();
        }


        if (Vector3.Distance(currentPosition, targetPosition) <= 0.00001)
        {
            currentPosition = targetPosition;
            currentAngle = targetAngle;
        }
        else
        {
            currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * smoothSpeed);
            currentAngle =Quaternion.Lerp(currentAngle,targetAngle, Time.deltaTime * smoothSpeed);
            mainCamera.transform.position = currentPosition;
            mainCamera.transform.rotation = currentAngle;

        }
    }

    void ChangeTargetPosition()
    {
        //neutralならば
        if (!nowFlag)
        {
            targetPosition = questionCameraPos;
            targetAngle = questionCameraAngle;
            nowFlag = true;
        }
        else
        {
            targetPosition = neutralCameraPos;
            targetAngle = neutralCameraAngle;
            nowFlag = false;
        }
    }
}
