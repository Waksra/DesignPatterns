using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class Actor : MonoBehaviour
{
      [SerializeField] private bool isFriendly;
      [SerializeField] private float moveSpeed;
      [SerializeField] private WeaponObject weapon;
      public  bool IsFriendly => isFriendly;

      private new Transform transform;

      public Actor TargetActor { get; set; }

      private void Awake()
      {
          transform = GetComponent<Transform>();
      }

      private void Start()
      {
          GameManager.AddActor(this);
      }

      private void OnDestroy()
      {
          GameManager.RemoveActor(this);
      }

      public ActorCommand GetCommand()
      {
          if(TargetActor != null)
          {
              ActorCommand command = new ActorCommand(() => Attack(TargetActor));
              return command;
          }
          
          return null;
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
          
          while (Vector3.Distance(transform.position,attackPosition) >= 0.0001f)
          {
              float frameMovement = moveSpeed * Time.deltaTime;
              transform.position = Vector3.MoveTowards(transform.position, attackPosition, frameMovement);
              yield return null;
          }
          transform.position = attackPosition;
          
          Debug.Log("POW!");

          yield return new WaitForSeconds(0.5f);
          
          while (Vector3.Distance(transform.position,originalPosition) >= 0.0001f)
          {
              float frameMovement = moveSpeed * Time.deltaTime;
              transform.position = Vector3.MoveTowards(transform.position, originalPosition, frameMovement);
              yield return null;
          }
          transform.position = originalPosition;
      }
}