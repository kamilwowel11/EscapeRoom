using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    public Camera playerCamera;

    public Player player;
    
    [SerializeField]
    private GameObject playerMainBody;
    [SerializeField]
    private float mouseSens = 10f;

    [HideInInspector]
    public Vector3 cameraForward;
    [HideInInspector]
    public Vector3 cameraRight;

    private Vector2 mouseLook;
    private float xRotation;

    private void Awake()
    {
        cameraForward = playerMainBody.transform.forward;
        cameraRight = playerMainBody.transform.right;
    }

    public void Look(InputAction.CallbackContext context)
    {
        if (!player.playerInput.CanPlayerInput)
            return;

        mouseLook = context.ReadValue<Vector2>();

        float mouseX = mouseLook.x * mouseSens * Time.deltaTime;
        float mouseY = mouseLook.y * mouseSens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerMainBody.transform.Rotate(Vector3.up * mouseX);

        cameraForward = playerMainBody.transform.forward;
        cameraRight = playerMainBody.transform.right;
    }

    public TransformData GetTransformCamera()
    {
        var transformData = new TransformData(playerCamera.transform.position, playerCamera.transform.position - playerCamera.transform.parent.position, playerCamera.transform.rotation);
        return transformData;
    }

    [Serializable]
    public struct TransformData
    {
        public Vector3 Position;
        public Vector3 LocalPosition;
        public Quaternion Rotation;

        public TransformData(Vector3 position, Vector3 localPosition, Quaternion rotation)
        {
            Position = position;
            LocalPosition = localPosition;
            Rotation = rotation;
        }
    }
}
