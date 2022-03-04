using System;
using System.Collections;
using UnityEngine;

public class Actor : MonoBehaviour
{
      [SerializeField] private bool isFriendly;
      [SerializeField] private float moveSpeed;
      [SerializeField] private int maxHealth;
      [SerializeField] private int initiative;
      [SerializeField] private WeaponObject weapon;
      [SerializeField] private float selectionDiscRadius = 0.75f;
      [SerializeField] private Vector3 damageNumberStart;

      private int currentHealth;
      public  bool IsFriendly => isFriendly;

      private new Transform transform;
      private new Collider collider;

      public Actor TargetActor { get; set; }
      public ActorState CurrentState { get; private set; }
      public int MaxHealth => maxHealth;
      public int CurrentHealth => currentHealth;
      public int Initiative => initiative;
      public float SelectionDiscRadius => selectionDiscRadius;
      public Vector3 GetDamageNumberPosition => transform.position + damageNumberStart;

      private event Action<float> OnDamageTaken;

      private void Awake()
      {
          transform = GetComponent<Transform>();
          collider = GetComponent<Collider>();
          currentHealth = maxHealth;

          CurrentState = ActorState.Idle;
      }

      private void Start()
      {
          GameManager.AddActor(this);
      }

      private void OnDestroy()
      {
          GameManager.RemoveActor(this);
      }

      public void SubscribeToOnDamageTaken(Action<float> response)
      {
          OnDamageTaken += response;
      }

      public void UnsubscribeToOnDamageTaken(Action<float> response)
      {
          OnDamageTaken -= response;
      }

      public ActorCommand GetCommand()
      {
          if(TargetActor != null)
          {
              ActorCommand command = new ActorCommand(this, TargetActor, ActorCommand.ActionType.Attack);
              return command;
          }
          
          return null;
      }

      public Vector3 GetFeetPosition()
      {
          Vector3 position = transform.position;
          position.y -= collider.bounds.size.y / 2;

          return position;
      }

      public void TakeDamage(int damage)
      {
          if(damage <= 0)
              return;

          currentHealth -= damage;
          currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

          OnDamageTaken?.Invoke(damage);

          DamageNumber.SpawnNewDamageNumber(damage, this);
          
          Debug.Log(name + " took " + damage + " damage.");
      }

      public void Attack(Actor target)
      {
          Vector3 position = transform.position;
          Vector3 targetPosition = target.transform.position;
          targetPosition.y = position.y;

          Vector3 attackPosition;
          if (Vector3.Distance(position, targetPosition) <= weapon.range)
          {
              attackPosition = position;
          }
          else
          {
              Vector3 directionFromTarget = (position - targetPosition).normalized;
              attackPosition = targetPosition + directionFromTarget * weapon.range;   
          }

          StartCoroutine(MoveAndAttack(attackPosition, target));
      }

      private IEnumerator MoveAndAttack(Vector3 attackPosition, Actor target)
      {
          Vector3 originalPosition = transform.position;
          
          CurrentState = ActorState.MovingForward;
          
          while (Vector3.Distance(transform.position,attackPosition) >= 0.0001f)
          {
              float frameMovement = moveSpeed * Time.deltaTime;
              transform.position = Vector3.MoveTowards(transform.position, attackPosition, frameMovement);
              yield return null;
          }
          transform.position = attackPosition;

          CurrentState = ActorState.Attacking;
          target.TakeDamage(weapon.damage);
          
          yield return new WaitForSeconds(0.5f);

          CurrentState = ActorState.MovingBack;
          
          while (Vector3.Distance(transform.position,originalPosition) >= 0.0001f)
          {
              float frameMovement = moveSpeed * Time.deltaTime;
              transform.position = Vector3.MoveTowards(transform.position, originalPosition, frameMovement);
              yield return null;
          }
          transform.position = originalPosition;

          CurrentState = ActorState.Idle;
      }

      private void OnDrawGizmos()
      {
          if (TargetActor != null)
          {
              Gizmos.color = Color.red;
              Gizmos.DrawLine(transform.position, TargetActor.transform.position);
          }
      }

      private void OnDrawGizmosSelected()
      {
          Vector3 position = GetComponent<Transform>().position;
          
          Gizmos.color = Color.red;
          Gizmos.DrawWireSphere(position + damageNumberStart, 0.2f);
      }

      public enum ActorState
      {
          Idle,
          MovingForward,
          MovingBack,
          Attacking
      }
}