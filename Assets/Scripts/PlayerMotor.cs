using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{

    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 jumpForce = Vector3.zero;

    private float cameraRotationX = 0f;
    private float cameraRotationY = 0f;
    private float currentCameraRotationX = 0f;
    private float currentCameraRotationY = 0f;
    private bool resetRotation = false;

    private Rigidbody rb;

    private float maxZoomTreshold;
    private float minZoomTreshold;
    private float zoomStepSize;

    private Coroutine zoomCoroutine;
    private float smoothingFactor;



    #region Properties
    public Camera Cam
    {
        set
        {
            cam = value;
        }
    }

    public float MinZoomTreshold
    {
        set
        {
            minZoomTreshold = value;
        }
    }

    public float MaxZoomTreshold
    {
        set
        {
            maxZoomTreshold = value;
        }
    }

    public float ZoomStepSize
    {
        get
        {
            return zoomStepSize;
        }

        set
        {
            zoomStepSize = value;
        }
    }

    public float SmoothingFactor
    {
        get
        {
            return smoothingFactor;
        }

        set
        {
            smoothingFactor = value;
        }
    }
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        PerfomMovement();
        PerformRotation();
    }

    void LateUpdate()
    {

        cam.transform.LookAt(cam.transform.parent);
    }

    public void Move(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    public void Rotate(Vector3 rotation)
    {
        this.rotation = rotation;
    }
    public void RotateCameraAroundCharacter(float cameraRotationX, float cameraRotationY)
    {
        this.cameraRotationX = cameraRotationX;
        this.cameraRotationY = cameraRotationY;
    }

    public void ApplyJump(Vector3 jumpForce)
    {
        this.jumpForce = jumpForce;
    }

    void PerfomMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
        }

        if (jumpForce != Vector3.zero)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }

    void PerformRotation()
    {
        if(rotation != Vector3.zero)
        {
            rb.MoveRotation(transform.rotation * Quaternion.Euler(rotation));
            rotation = Vector3.zero;
        }
        if (cam != null && ((cameraRotationX != 0 || cameraRotationY != 0)) || resetRotation)
        {
            if (resetRotation)
            {
                currentCameraRotationX = 0;
                currentCameraRotationY = 0;
                resetRotation = false;
            }
            else
            {
                currentCameraRotationX += cameraRotationX;
                currentCameraRotationY += cameraRotationY;
            }
            
            cameraRotationX = 0;
            cameraRotationY = 0;
            cam.transform.parent.transform.localEulerAngles = new Vector3(currentCameraRotationX, currentCameraRotationY, 0f);
        }
        
    }

    public void ResetCameraRotation()
    {
        resetRotation = true;
    }

    public void Zoom(int zoomDirection)
    {
        if(zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
        }

        zoomCoroutine = StartCoroutine(ZoomRoutine(zoomDirection));
    }

    //handles zoom functionality 
    public IEnumerator ZoomRoutine(int zoomDirection)
    {
        float distance;
        Vector3 targetPos;

        //zoomDirection = 1 is zoom in
        if (zoomDirection == 1)
        {
            //calculate position of cam after zoom and the distance to the player
            targetPos = Vector3.MoveTowards(cam.transform.position, cam.transform.parent.position, zoomStepSize);
            distance = Vector3.Distance(targetPos, cam.transform.parent.position);

            //apply zoom if distance between targetPos and player is above treshold
            if (distance > minZoomTreshold)
            {
                float progress = 0;
                while(progress < 1)
                {
                    progress += Time.deltaTime * smoothingFactor;
                    cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, progress);
                    yield return null;
                }
            }
        }
        //zoomDirection = -1 is zoom out
        else if(zoomDirection == -1)
        {
            //calculate position of cam after zoom and the distance to the player
            targetPos = Vector3.MoveTowards(cam.transform.position, cam.transform.parent.position, -zoomStepSize);
            distance = Vector3.Distance(targetPos, cam.transform.parent.position);

            //apply zoom if distance between targetPos and player is below treshold
            if (distance < maxZoomTreshold)
            {
                float progress = 0;
                while (progress < 1)
                {
                    progress += Time.deltaTime * smoothingFactor;
                    cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, progress);
                    yield return null;
                }
            }
        }

        yield return null;

        StopCoroutine(zoomCoroutine);
    }
}
