using UnityEngine;

public class EnemyChaseSOBase : ScriptableObject
{
    protected Enemy_ enemy;
    protected Transform transform;
    protected GameObject gameObject;

    protected Transform playerTransform;

    public virtual void Initialize(GameObject gameObject, Enemy_ enemy)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
        this.enemy = enemy;
        this.playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public virtual void DoEnterLogic() { }
    public virtual void DoExitLogic() { ResetValues(); }
    public virtual void DoFrameUpdateLogic()
    {
        if (enemy.IsWithinStrikingDistance)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }
    }
    public virtual void DoPhysicsLogic() { }

    //public virtual void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType) { }
    public virtual void ResetValues() { }


}
