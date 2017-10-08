///Steffano de Moura fanolinux@gmail.com 
///https://www.youtube.com/fanolinux
///7 Oct 2017
///Camera TPS

using UnityEngine;
using System;

public class CameraTPS : MonoBehaviour
{
    #region Public

    [Serializable]
    public class CameraSettings
    {
        public Transform Target; //Player or any other target
        [Space]
        [Range(0.0f, 10f)]
        public float TargetHeight = 1f; //the target height
        [Range(0.0f, 10f)]
        public float Distance = 4f; //the desired distance
        [Range(0.0f, 10f)]
        public float Smoothing = 3f; //lerping smooth
        public float HorizontalSpeed = 1.2f; //horizontal input speed
        public float VerticalSpeed = 1.2f; //vertical input speed
        [Header("Limits")]
        public float VerticalMin = -56f;
        public float VerticalMax = 56f;
        [Header("Collision Layer")]
        public LayerMask Layers; //check for collision on those layers
    }

    public CameraSettings Settings;

    #endregion

    #region Private

    private Transform tCamera;
    private float inputV = 0f;
    private float inputH = 0f;
    private float x = 0.0f;
    private float y = 0.0f;
    private float desiredDistance;
    private Quaternion desiredRotation;
    private Vector3 offset = new Vector3(0f, 0f, 1f);
    private Vector3 desiredPosition;
    private Vector3 targetPostion;

    #endregion

    #region Unity

    void Awake()
    {
        tCamera = transform;
    }

    void Start()
    {
        desiredDistance = Settings.Distance;
    }
        
    void Update()
    {
        if (Settings.Target == null)
            return;

        updateCamera();
    }

    #endregion

    #region Camera Logic

    private void updateCamera()
    {
        //Input data
        inputV = Input.GetAxis("Mouse Y");
        inputH = Input.GetAxis("Mouse X");

        //Proccess input data
        y -= inputV * Settings.VerticalSpeed;
        x -= inputH * Settings.HorizontalSpeed;
        y = clampAngle(y, Settings.VerticalMin, Settings.VerticalMax);
        x = clampAngle(x, -360f, 360f);

        //find the desired distance
        desiredDistance = Settings.Distance;
        //find desired rotation
        desiredRotation = Quaternion.Euler(y, -x, tCamera.rotation.z);
        //calculate target height
        targetPostion = Settings.Target.position;
        targetPostion.y += Settings.TargetHeight;

        //calculate desired position
        offset.z = desiredDistance;
        desiredPosition = targetPostion - (desiredRotation * offset);

        //Apply desired position and rotation
        tCamera.position = desiredPosition;
        tCamera.rotation = desiredRotation;
    }

    private static float clampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    #endregion
}