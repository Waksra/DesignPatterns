using Shapes;
using UnityEngine;

public class SelectionDisc : MonoBehaviour
{
    [SerializeField] private float offsetFromFloor = 0.1f;
    [SerializeField] private float rotationSpeed = 10.0f;
    [SerializeField] private float pulseRange = 0.15f;
    [SerializeField] private float pulseSpeed = 0.05f;

    private float discRadius;
    private bool isPulsingOut = true;
    
    private new Transform transform;
    private Disc disc;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        
        disc = GetComponentInChildren<Disc>();
        discRadius = disc.Radius;
        
        Hide();
    }

    private void Update()
    {
        if(!disc.enabled)
            return;

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        Pulse();
    }

    public void Show()
    {
        disc.enabled = true;
    }
    
    public void Hide()
    {
        disc.enabled = false;
    }

    public void SetPosition(Vector3 position)
    {
        position.y = offsetFromFloor; 
        transform.position = position;

        disc.enabled = true;
    }

    public void SetTarget(Actor target)
    {
        disc.enabled = true;
        disc.Radius = target.SelectionDiscRadius;
        discRadius = disc.Radius;

        Vector3 newPosition = target.GetFeetPosition();
        newPosition.y += offsetFromFloor;
        transform.position = newPosition;
    }

    private void Pulse()
    {
        if (isPulsingOut)
        {
            disc.Radius += pulseSpeed * Time.deltaTime;
            if (disc.Radius >= discRadius + pulseRange / 2)
                isPulsingOut = false;
        }
        else
        {
            disc.Radius -= pulseSpeed * Time.deltaTime;
            if (disc.Radius <= discRadius - pulseSpeed / 2)
                isPulsingOut = true;
        }
    }
}
