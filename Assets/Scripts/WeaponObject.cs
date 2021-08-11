using UnityEngine;

[CreateAssetMenu(fileName = "WeaponObject", menuName = "Scriptable Objects", order = 0)]
public class WeaponObject : ScriptableObject
{
    public int damage;
    public float range;
}