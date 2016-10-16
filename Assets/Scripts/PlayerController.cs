using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float sprintSpeedMultiplier = 2f;
    [SerializeField]
    private float maxStamina = 1;
    [SerializeField]
    private float staminaBurnSpeed = 0.2f;
    [SerializeField]
    private float staminaRegenSpeed = 0.1f;
    [SerializeField]
    private float staminaRegenDelay = 1f;

    private float staminaAmount;
    private float lastTimeStaminaUsed;
    private bool staminaWasDepleted = false;

    [SerializeField]
    private float jumpForce = 50;
    [SerializeField]
    private bool airControl = true;
    [SerializeField]
    private int allowedConsecutiveJumps = 2;
    private int amountConsecutiveJumps;
    private bool isGrounded = true;

    [SerializeField]
    private float groundCheckDistance = 0.1f;
    [SerializeField]
    private GameObject graphics;

    [Header("Mouse Settings")]
    [SerializeField]
    private float mouseSensitivity = 3f;
    [SerializeField]
    private bool invertMouseX = false;
    [SerializeField]
    private bool invertMouseY = false;

    [Header("Zoom Settings")]
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float zoomStepSize = 1;
    [SerializeField]
    private float minZoomTreshold = 1f;
    [SerializeField]
    private float maxZoomTreshold = 15f;
    [SerializeField]
    private float smoothingFactor = 2;
    [SerializeField]
    float doubleClickSpeed = 0.5f;

    PlayerMotor motor;
    bool isFirstClick = true;
    float firstClickTime = 0;

    #region Properties
    public float StaminaAmount
    {
        get
        {
            return staminaAmount;
        }
    }

    public float MaxStamina
    {
        get
        {
            return maxStamina;
        }
    }
    #endregion

    void OnEnable()
    {
        EventHandler.OnSettingsChanged += OnsettingsChanged;
    }

    void OnDisable()
    {
        EventHandler.OnSettingsChanged -= OnsettingsChanged;
    }

    private void OnsettingsChanged(SettingsSave config)
    {
        doubleClickSpeed = config.controls.doubleClickSpeed;
        mouseSensitivity = config.controls.mouseSensitivity;
    }

    void Awake()
    {
        InitialMotorSetup();

        staminaAmount = maxStamina;
    }

    void Update()
    {
        GroundCheck();
        //Calculate Movement velocity
        float xMov = Input.GetAxis("Horizontal");
        float zMov = Input.GetAxis("Vertical");

        Vector3 movHorizontal = transform.right * xMov;
        Vector3 movVertical = transform.forward * zMov;


        //final movement vector
        Vector3 velocity = (movHorizontal + movVertical).normalized;


        //adjust velocity to run or sprint speed
        if (Input.GetKey(KeyCode.LeftShift) && ((staminaAmount >= .5f && staminaWasDepleted) || !staminaWasDepleted) && isGrounded)
        {
            staminaWasDepleted = false;
            lastTimeStaminaUsed = Time.time;

            velocity *= speed * sprintSpeedMultiplier;
            staminaAmount -= staminaBurnSpeed * Time.deltaTime;

            if (staminaAmount <= 0)
            {
                staminaWasDepleted = true;
            }
        }
        else
        {
            if (Time.time >= lastTimeStaminaUsed + staminaRegenDelay)
            {
                staminaAmount += staminaRegenSpeed * Time.deltaTime;
            }

            velocity *= speed;
        }

        staminaAmount = Mathf.Clamp(staminaAmount, 0, maxStamina);

        //apply movement if on ground or with activated air control
        if (airControl || isGrounded)
        {
            motor.Move(velocity);
        }

        //calculate jump force
        Vector3 jumpForce = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || amountConsecutiveJumps < allowedConsecutiveJumps)
            {
                jumpForce = Vector3.up * this.jumpForce;
                amountConsecutiveJumps++;
            }

        }

        //apply jump force

        motor.ApplyJump(jumpForce);

        Vector3 rotation = Vector3.zero;



        //reset camera position on double click
        if (InputManager.GetKeyDown("Camera"))
        {
            if (isFirstClick)
            {
                firstClickTime = Time.time;
                isFirstClick = false;
            }
            else
            {
                float timeTillSecondClick = Time.time - firstClickTime;
                if (timeTillSecondClick < doubleClickSpeed)
                {
                    motor.ResetCameraRotation();
                    isFirstClick = true;
                }
                else
                {
                    firstClickTime = Time.time;
                }
            }
        }

        //calculate camera rotation around character
        if (InputManager.GetKey("Camera"))
        {            
            float xRotChar = Input.GetAxis("Mouse Y");
            float cameraRotationX = xRotChar * mouseSensitivity;
            float yRotChar = Input.GetAxis("Mouse X");
            float cameraRotationY = yRotChar * mouseSensitivity;

            //apply camera rotation based on invertion settings
            if (!invertMouseX)
            {
                if (!invertMouseY)
                {
                    motor.RotateCameraAroundCharacter(-cameraRotationX, cameraRotationY);
                }
                else
                {
                    motor.RotateCameraAroundCharacter(-cameraRotationX, -cameraRotationY);
                }
            }
            else
            {
                if (!invertMouseY)
                {
                    motor.RotateCameraAroundCharacter(cameraRotationX, cameraRotationY);
                }
                else
                {
                    motor.RotateCameraAroundCharacter(cameraRotationX, -cameraRotationY);
                }
            }
        }
        else
        {
            //calculate character rotation
            float yRot = Input.GetAxis("Mouse X");
            rotation = new Vector3(0f, yRot, 0f) * mouseSensitivity;

        }

        //apply rotation based on invertion settings
        if (rotation != Vector3.zero)
        {
            if (!invertMouseY)
            {
                motor.Rotate(rotation);
            }
            else
            {
                motor.Rotate(-rotation);
            }
        }


        //calculate camera zoom
        int zoomDirection = 0;
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            zoomDirection = 1;
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                zoomDirection = -1;
            }
        }

        //apply camera zoom
        if (zoomDirection != 0)
        {
            SetZoomSettings(); //only for testing - can be deleted in build
            motor.Zoom(zoomDirection);
        }


    }

    void InitialMotorSetup()
    {
        motor = GetComponent<PlayerMotor>();

        motor.Cam = cam;

        SetZoomSettings();
    }

    void SetZoomSettings()
    {
        motor.MaxZoomTreshold = maxZoomTreshold;
        motor.MinZoomTreshold = minZoomTreshold;
        motor.ZoomStepSize = zoomStepSize;
        motor.SmoothingFactor = smoothingFactor;
    }
    // check if the character is standing on a ground
    void GroundCheck()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position - transform.GetComponent<Collider>().bounds.extents + new Vector3(0f, 0.05f, 0f), Vector3.down * groundCheckDistance, Color.red);
        if (Physics.Raycast(transform.position - transform.GetComponent<Collider>().bounds.extents + new Vector3(0f, 0.05f, 0f), Vector3.down, out hit, groundCheckDistance))
        {
            isGrounded = true;
            amountConsecutiveJumps = 0;
        }
        else
        {
            isGrounded = false;
        }
    }
}
