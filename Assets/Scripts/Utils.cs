using UnityEngine;

public class Utils
{
    public static bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) > 0;
    }
    
    public static bool IsInLayerMask(Collider collider, LayerMask layerMask)
    {
        return (layerMask.value & (1 << collider.gameObject.layer)) > 0;
    }
    
    public static bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) > 0;
    }
}