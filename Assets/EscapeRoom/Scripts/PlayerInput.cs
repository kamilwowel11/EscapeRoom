using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerCameraController;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector]
    public IInteractable InteractionTarget { get; private set; }

    [HideInInspector]
    public bool CanPlayerInput = true;

    [SerializeField]
    private float playerMovementSpeed = 10f;
    [SerializeField]
    private float gravityMultiplier = 3.0f;
    [SerializeField]
    private float jumpPower = 10f;
    [SerializeField]
    private Vector3 playerDirectionVector;
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private PlayerCameraController cameraController;

    [Header("Interaction")]
    [SerializeField]
    private LayerMask interactionMask;
    [SerializeField]
    private float interactionDistance = 10f;
    [SerializeField]
    private float interactionPrecisionRadius = 1f;

    private Player localPlayer;
    private Vector2 playerInput;
    private float velocity;

    private bool IsGrounded() => characterController.isGrounded;
    private RaycastHit[] interactionHits = new RaycastHit[10];

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        localPlayer = GetComponent<Player>();
        localPlayer.playerInput.CanPlayerInput = true;
    }

    void Update()
    {
        if (!CanPlayerInput)
            return;

        UpdateInteractionTarget();

        if (localPlayer.isInDialogue)
            return;


        MoveUpdate();
        ApplyGravity();

        characterController.Move(playerDirectionVector * playerMovementSpeed * Time.deltaTime);
    }


    public void MoveUpdate()
    {
        playerDirectionVector = (playerInput.y * cameraController.cameraForward) + (playerInput.x * cameraController.cameraRight);
        playerDirectionVector = playerDirectionVector.normalized;
    }

    public void Move(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (InteractionTarget == null)
            return;

        InteractionTarget.Interact(localPlayer);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!CanPlayerInput) return;
        if (!context.started) return;
        if (!IsGrounded()) return;
        if (localPlayer.isInDialogue) return;

        velocity += jumpPower;
    }

    private void UpdateInteractionTarget()
    {
        InteractionTarget = null;
        TransformData cameraTransform = cameraController.GetTransformCamera();
        Vector3 cameraDirection = cameraTransform.Rotation * Vector3.forward;

        cameraTransform.Position += cameraDirection * interactionPrecisionRadius;

        interactionHits = Physics.SphereCastAll(cameraTransform.Position, interactionPrecisionRadius, cameraDirection, interactionDistance, interactionMask, QueryTriggerInteraction.Ignore);

        int hitCount = interactionHits.Length;

        if (hitCount <= 0)
            return;

        //GizmosHitDebugNames();

        RaycastHit validHit = default;

        if (Physics.Raycast(cameraTransform.Position, cameraDirection, out RaycastHit raycastHit, interactionDistance, interactionMask, QueryTriggerInteraction.Ignore) == true && raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Interaction"))
        {
            validHit = raycastHit;
        }
        else
        {
            SortRaycasts(interactionHits, interactionHits.Length);

            for (int i = 0; i < hitCount; i++)
            {
                var hit = interactionHits[i];

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
                {
                    return;
                }

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interaction"))
                {
                    validHit = hit;
                    break;
                }
            }
        }

        Collider collider = validHit.collider;

        if (collider == null)
        {
            return;
        }

        IInteractable interaction = collider.GetComponent<IInteractable>();

        if (interaction != null && interaction.canBeInteracted)
        {
            InteractionTarget = interaction;
        }
    }
    private void ApplyGravity()
    {
        if (IsGrounded() && velocity < 0.0f)
        {
            velocity = -1.0f;
        }
        else
        {
            velocity += Config.CONFIG_GRAVITY * gravityMultiplier * Time.deltaTime;
        }

        playerDirectionVector.y = velocity;
    }

    private void GizmosHitDebugNames()
    {
        string debug = "";
        foreach (var item in interactionHits)
        {
            debug += " + " + item.collider.name;
        }
        Debug.Log("Hitting: " + debug);
    }


    private void SortRaycasts(RaycastHit[] hits, int maxHits)
    {
        while (true)
        {
            bool swap = false;

            for (int i = 0; i < maxHits; ++i)
            {
                for (int j = i + 1; j < maxHits; ++j)
                {
                    if (hits[j].distance < hits[i].distance)
                    {
                        RaycastHit hit = hits[i];
                        hits[i] = hits[j];
                        hits[j] = hit;

                        swap = true;
                    }
                }
            }

            if (swap == false)
                return;
        }
    }
}
