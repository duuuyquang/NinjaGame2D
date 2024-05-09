using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{ 
    float timer;
    float randomTime;

    public void OnEnter(Enemy enemy)
    {
        if(enemy.Target != null) {
            
            enemy.ChangeDirection(enemy.Target.transform.position.x > enemy.transform.position.x);

            enemy.StopMoving();
            enemy.Attack();
        }

        timer = 0;
    }
    
    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;

        if(timer < randomTime) {
            enemy.Moving();
        } else {
            enemy.ChangeState(new IdleState());
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}
