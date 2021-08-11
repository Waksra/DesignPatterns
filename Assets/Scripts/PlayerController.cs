using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask rayHitLayer;

    private new Camera camera;
    private Controls controls;

    private float rayDistance = 1000f;

    private Vector2 mousePosition;

    private Actor selectedActor;

    private void Awake()
    {
        camera = Camera.main;
        
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.Gameplay.PointerPosition.performed += OnPointerUpdated;
        controls.Gameplay.Select.performed += OnSelectPerformed;
        controls.Gameplay.EndTurn.performed += OnEndTurnPerformed;
    }

    private void OnDisable()
    {
        controls.Disable();
        
        controls.Gameplay.PointerPosition.performed -= OnPointerUpdated;
        controls.Gameplay.Select.performed -= OnSelectPerformed;
        controls.Gameplay.EndTurn.performed -= OnEndTurnPerformed;
    }

    private void OnPointerUpdated(InputAction.CallbackContext callbackContext)
    {
        mousePosition = callbackContext.ReadValue<Vector2>();
    }

    private void OnSelectPerformed(InputAction.CallbackContext callbackContext)
    {
        Actor actor = GetActorUnderPointer(mousePosition);
        
        if(actor == null)
            return;

        if(actor.IsFriendly)
            selectedActor = actor;
        else if(selectedActor != null)
        {
            selectedActor.TargetActor = actor;
        }
    }

    private void OnEndTurnPerformed(InputAction.CallbackContext callbackContext)
    {
        GameManager.EndTurn();
    }

    private Actor GetActorUnderPointer(Vector2 pointerScreenPosition)
    {
        Ray ray = camera.ScreenPointToRay(pointerScreenPosition);

        RaycastHit hitInfo;

        Physics.Raycast(ray, out hitInfo, rayDistance, rayHitLayer);

        if (hitInfo.collider.TryGetComponent(out Actor hitActor))
            return hitActor;
        
        return null;
    }
}