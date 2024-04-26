using BikeMaster;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform cameraFollowTarget;
    [SerializeField] private FloatingJoystick floatingJoystick;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private CameraCullingMask cameraCullingMask;
    [SerializeField] DragInput input;

    [Header("Min & Max camera view")]
    [SerializeField] private float XMin = -70f;
    [SerializeField] private float XMax = 70f;

    [Header("Camera position")]
    [SerializeField] private float cameraDistance = 12f;
    [SerializeField] private float cameraVerticalSensitivity = 200f;

    [SerializeField]
    private float cameraHorizontalSensitivity = 200f;

    [SerializeField]
    private float rotationInertial;

    [SerializeField]
    private float rotateDuration = 2f;

    [SerializeField]
    private float largeAngleRotateDuration = 3f;

    [SerializeField]
    private float nonMovingDurationRate = 5f;

    [SerializeField]
    private float rotationDelay = 0.25f;

    [SerializeField]
    private float largeAngleRotationDelay = 0.25f;

    [SerializeField]
    private Vector3 defaultCameraRotation = new Vector3(20f, 0f, 0f);

    [Header("Other config")]
    [Range(0f, 1f)]
    [SerializeField]
    private float rotateSpeedDecreaseRateWhileJumping = 0.25f;

    [Range(0f, 1f)]
    [SerializeField]
    private float rotateSpeedDecreaseRateWhileNotMoving = 0.1f;

    public bool oldOption;

    private Quaternion currentRotation;

    private float rotationTimer;

    private bool isPressed;

    private void Awake()
    {
        this.RegisterListener(EventID.OnPlayerRespawnCompleted, delegate
        {
            RefreshCamera();
        });
        this.RegisterListener(EventID.OnPlayerDead, delegate
        {
            transform.DOKill();
        });
    }

    private void Start()
    {
        RefreshCamera();
    }

    private void Update()
    {
        LookThroughGround();
    }

    private void LateUpdate()
    {
        if (GameManager.IsState(GameState.Gameplay))
        {
            if (floatingJoystick.Vertical != 0f || floatingJoystick.Horizontal != 0f)
            {
                FreeRotateCamera();
            }
            RelatedTargetRotateCamera();
            CameraFollow();
        }
    }

    private void FreeRotateCamera()
    {
        transform.DOKill();
        Vector3 eulerAngles = currentRotation.eulerAngles;
        if (eulerAngles.x > 180f)
        {
            eulerAngles.x -= 360f;
        }
        eulerAngles.x -= input.Vertical * cameraVerticalSensitivity * Time.deltaTime;
        eulerAngles.y += input.Horizontal * cameraHorizontalSensitivity * Time.deltaTime;
        //eulerAngles.x -= floatingJoystick.Vertical * cameraVerticalSensitivity * Time.deltaTime;
        //eulerAngles.y += floatingJoystick.Horizontal * cameraHorizontalSensitivity * Time.deltaTime;
        eulerAngles.x = Mathf.Clamp(eulerAngles.x, XMin, XMax);
        transform.rotation = Quaternion.Euler(eulerAngles);
        currentRotation = transform.rotation;
    }

    private void RelatedTargetRotateCamera()
    {
        if (!player.IsGrounded)
        {
            transform.DOKill();
        }
        else
        {
            if (player.IsDead)
            {
                return;
            }
            if (player.HorizontalInput != 0f || player.VerticalInput != 0f)
            {
                float y = player.transform.eulerAngles.y;
                Quaternion quaternion = Quaternion.Euler(new Vector3(transform.eulerAngles.x, y, transform.eulerAngles.z));
                if (player.IsMoving)
                {
                    rotationTimer = 0f;
                    float num = Quaternion.Angle(transform.rotation, quaternion);

                    float duration = largeAngleRotateDuration * Mathf.Abs(num) / 180f;
                    if (Mathf.Abs(num) < 90f)
                    {
                        transform.DORotateQuaternion(quaternion, rotateDuration).SetDelay(rotationDelay).SetEase(Ease.Linear);
                    }
                    else
                    {
                        transform.DORotateQuaternion(quaternion, duration).SetDelay(largeAngleRotationDelay).SetEase(Ease.Linear);
                    }
                }
                else if (oldOption)
                {
                    rotationTimer += Time.deltaTime;
                    if (rotationTimer >= rotationInertial)
                    {
                        transform.DOKill();
                    }
                }
                else
                {
                    rotationTimer = 0f;
                    if (Mathf.Abs(Quaternion.Angle(transform.rotation, quaternion)) < 90f)
                    {
                        transform.DORotateQuaternion(quaternion, rotateDuration * nonMovingDurationRate).SetEase(Ease.Linear);
                    }
                    else
                    {
                        transform.DORotateQuaternion(quaternion, largeAngleRotateDuration * nonMovingDurationRate).SetEase(Ease.Linear);
                    }
                }
            }
            else
            {
                rotationTimer += Time.deltaTime;
                if (rotationTimer >= rotationInertial)
                {
                    transform.DOKill();
                }
            }
            currentRotation = transform.rotation;
        }
    }

    #region CameraFollow
    [Header("Camera Follow")]
    [SerializeField] float offSetWhenCameraBlocked = 0.5f;
    private Vector3 desiredPosition;
    private Vector3 blockedPosition;
    private bool isBlockedByGrouund;
    private void CameraFollow()
    {
        Vector3 vector = new Vector3(0f, 0f, 0f - cameraDistance);

        desiredPosition = cameraFollowTarget.position + transform.rotation * vector;

        if (isBlockedByGrouund)
        {
            transform.position = blockedPosition;
        }
        else
        {
            transform.position = desiredPosition;
        }     
    }

    private void LookThroughGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(desiredPosition, cameraFollowTarget.position - desiredPosition, out hit, cameraDistance, groundLayer)/* && transform.rotation.eulerAngles.x >= 180f*/)
        {
            isBlockedByGrouund = true;
            blockedPosition = hit.point + (cameraFollowTarget.position - desiredPosition) * offSetWhenCameraBlocked;
            //cameraCullingMask.HideLayerMask("Ground");
        }
        else
        {
            isBlockedByGrouund = false;
            //cameraCullingMask.ShowLayerMask("Ground");
        }
    }
    #endregion

    private void RefreshCamera()
    {
        transform.rotation = Quaternion.Euler(defaultCameraRotation);
        currentRotation = transform.rotation;
    }
}
