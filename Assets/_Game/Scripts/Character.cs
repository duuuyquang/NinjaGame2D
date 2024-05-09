using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected HealthBar healthBar;

    [SerializeField] private CombatText combatTextPrefab;

    private float hp;
    private string curAnimName;

    public bool IsDead => hp <= 0;

    public void Start() {
        OnInit();
    }

    public virtual void OnInit() {
        hp = 100;
        healthBar.OnInit(100, transform);
    }

    protected virtual void OnDespawn() {

    }

    protected virtual void OnDeath() {
        ChangeAnim("die");

        Invoke(nameof(OnDespawn), 2f);
    }

    protected void ChangeAnim(string animName) {
        if(curAnimName != animName) {
            anim.ResetTrigger(animName);
            curAnimName = animName;
            anim.SetTrigger(curAnimName);
        }
    }

    public void OnHit(float damage) {
        if(!IsDead) {
            hp -= damage;
            if(IsDead) {
                hp = 0;
                OnDeath();
            }

            healthBar.SetNewHp(hp);
            Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        }
    }
}
