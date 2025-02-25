using UnityEngine;

public class EnemyAttackState : EnemyState
{


    public EnemyAttackState(Enemy_ enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
     }

    public override void AnimationTriggerEvent(Enemy_.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.EnemyAttackInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.EnemyAttackInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.EnemyAttackInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.EnemyAttackInstance.DoPhysicsLogic();
    }
}
