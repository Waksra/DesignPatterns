using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask rayHitLayer;
    [SerializeField] private SelectionDisc selectionDisc;
    [SerializeField] private TargetVisualizer targetVisualizer;
    [Space] 
    [SerializeField] private LayerMask actorMask;
    [SerializeField] private LayerMask groundMask;

    private new Camera camera;
    private Controls controls;

    private float rayDistance = 1000f;

    private Vector2 mousePosition;

    private Actor selectedActor;

    public Actor SelectedActor
    {
        get => selectedActor;

        private set
        {
            selectedActor = value;
            
            if (selectedActor != null)
            {
                targetVisualizer.SetOwner(selectedActor);
                selectionDisc.SetTarget(selectedActor);
                if (selectedActor.TargetActor != null)
                    targetVisualizer.SetTarget(selectedActor.TargetActor);
            }
            else
            {
                selectionDisc.Hide();
                targetVisualizer.Hide();
            }
        }
    }

    private void Awake()
    {
        camera = Camera.main;
        
        controls = new Controls();
        
        selectionDisc = GetComponentInChildren<SelectionDisc>();
        targetVisualizer = GetComponentInChildren<TargetVisualizer>();
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

    private void Update()
    {
        if (selectedActor != null && selectedActor.TargetActor == null)
        {
            Ray ray = camera.ScreenPointToRay(mousePosition);

            RaycastHit hitInfo;

            if (!Physics.Raycast(ray, out hitInfo, rayDistance, rayHitLayer))
                return;

            if (Utils.IsInLayerMask(hitInfo.collider, groundMask))
            {
                targetVisualizer.SetEnd(hitInfo.point);
            }
            else if(Utils.IsInLayerMask(hitInfo.collider, actorMask))
            {
                if(hitInfo.collider.TryGetComponent(out Actor hitActor) && !hitActor.IsFriendly)
                    targetVisualizer.SetTarget(hitActor);
            }
        }
    }

    private void OnPointerUpdated(InputAction.CallbackContext callbackContext)
    {
        mousePosition = callbackContext.ReadValue<Vector2>();
    }

    private void OnSelectPerformed(InputAction.CallbackContext callbackContext)
    {
        Actor actor = GetActorUnderPointer(mousePosition);

        if (actor == null)
        {
            SelectedActor = null;
            return;
        }

        if(actor.IsFriendly)
            SelectedActor = actor;
        else if(selectedActor != null)
        {
            selectedActor.TargetActor = actor;
            targetVisualizer.SetTarget(actor);
        }
    }

    private void OnEndTurnPerformed(InputAction.CallbackContext callbackContext)
    {
        GameManager.EndTurn();
        SelectedActor = null;
    }

    private Actor GetActorUnderPointer(Vector2 pointerScreenPosition)
    {
        Ray ray = camera.ScreenPointToRay(pointerScreenPosition);

        RaycastHit hitInfo;

        Physics.Raycast(ray, out hitInfo, rayDistance, rayHitLayer);

        if (hitInfo.collider != null && hitInfo.collider.TryGetComponent(out Actor hitActor))
            return hitActor;
        
        return null;
    }
}