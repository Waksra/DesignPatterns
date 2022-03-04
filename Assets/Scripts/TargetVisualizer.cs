using Shapes;
using UnityEngine;

public class TargetVisualizer : MonoBehaviour
{
    [SerializeField] private float offsetFromFloor = 0.1f;
    
    private SelectionDisc selectionDisc;
    private Line line;

    private void Awake()
    {
        selectionDisc = GetComponentInChildren<SelectionDisc>();
        line = GetComponentInChildren<Line>();
        line.transform.position = Vector3.up * offsetFromFloor; 
        
        line.enabled = false;
    }

    public void SetOwner(Actor newOwner)
    {
        if(newOwner.TargetActor == null)
            Hide();
        
        SetStart(newOwner.GetFeetPosition());
    }

    public void SetEnd(Vector3 position)
    {
        Vector3 newPosition = new Vector3(position.x, position.z);
        line.End = newPosition;
        line.enabled = true;
        
        selectionDisc.SetPosition(position);
    }

    public void SetStart(Vector3 position)
    {
        Vector3 newPosition = new Vector3(position.x, position.z);
        line.Start = newPosition;
    }

    public void SetTarget(Actor target)
    {
        Vector3 newPosition = target.GetFeetPosition();
        newPosition.z += offsetFromFloor;
        line.End = newPosition;
        line.enabled = true;
        
        selectionDisc.SetTarget(target);
    }

    public void Hide()
    {
        line.enabled = false;
        selectionDisc.Hide();
    }
}