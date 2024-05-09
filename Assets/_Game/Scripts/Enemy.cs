using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private GameObject attackArea;

    private IState curState;

    private bool isRight = true;

    private Character target;
    public Character Target => target;

    private void Update() {
        if(curState != null && !IsDead) {
            curState.OnExecute(this);
        }
    }

    public override void OnInit() {
        base.OnInit();

        ChangeState(new IdleState());
        DeactiveAttackArea();
    }

    protected override void OnDespawn() {
        base.OnDespawn();
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
    }

    protected override void OnDeath() {
        base.OnDeath();
    }

    private void ActiveAttackArea() {


        attackArea.SetActive(true);
    }
    private void DeactiveAttackArea() {
        attackArea.SetActive(false);
    }

    public void ChangeState(IState newState) {
        if(curState != null) {
            curState.OnExit(this);
        }

        curState = newState;

        if(curState != null) {
            curState.OnEnter(this);
        }
    }

    public void SetTarget(Character character) {
        this.target = character;

        if(IsTargetInRange()) {
            ChangeState(new AttackState());
        } else if( Target != null){
            ChangeState(new PatrolState());
        } else {
            ChangeState(new IdleState());
        }
    }

    public void Moving() {
        ChangeAnim("run");
        rb.velocity = transform.right * moveSpeed;
    }

    public void StopMoving() {
        ChangeAnim("idle");
        rb.velocity = Vector2.zero;
    }

    public void Attack() {
        ChangeAnim("attack");
        //Invoke(nameof(ResetAttack), 0.5f);
        ActiveAttackArea();
        Invoke(nameof(DeactiveAttackArea), 0.5f);
    }

    // private void ResetAttack() {
    //     ChangeAnim("idle");
    // }

    public bool IsTargetInRange() {
        if(target == null) {
            return false;
        }
        return Vector2.Distance(target.transform.position, transform.position) <= attackRange;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "EnemyWall") {
            ChangeDirection(!isRight);
        }
    }

    public void ChangeDirection(bool isRight) {
        this.isRight = isRight;
        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }

}
