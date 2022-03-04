using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Actor owner;

    private Slider slider;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        owner.SubscribeToOnDamageTaken(DamageTakenResponse);
        
        slider.value = (float)owner.CurrentHealth / owner.MaxHealth;
    }

    private void DamageTakenResponse(float damage)
    {
        slider.value = (float)owner.CurrentHealth / owner.MaxHealth;
    }
}
