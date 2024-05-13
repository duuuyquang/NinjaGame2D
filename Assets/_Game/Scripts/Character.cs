using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public const float HP_REGEN_AMOUNT = 10f;
    public const float HP_REGEN_RATE_BY_SEC = 1f;
    public const float HP_REGEN_DELAY = 3f;

    private const int HP_MAX = 100;

    [SerializeField] private Animator anim;
    [SerializeField] protected HealthBar healthBar;

    [SerializeField] private CombatText combatTextPrefab;

    private float hp;
    private string curAnimName;
    protected bool isRegenable;

    public bool IsDead => hp <= 0;

    public void Start() {
        OnInit();
    }

    public virtual void OnInit() {
        hp = HP_MAX;
        healthBar.OnInit(hp, transform);
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
            isRegenable = false;
        }
    }

    public void onRegen(float regenAmount)
    {
        if(!IsDead && hp < HP_MAX)
        {
            hp = Mathf.Min(hp + regenAmount, HP_MAX);
            healthBar.SetNewHp(hp);
            Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(regenAmount);
        }

        if(hp >= HP_MAX)
        {
            CancelInvoke(nameof(onRegen));
        }
    }
}
