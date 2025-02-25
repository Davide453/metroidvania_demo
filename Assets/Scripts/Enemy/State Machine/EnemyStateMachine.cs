using UnityEngine;

public class EnemyStateMachine
{

    public EnemyState CurrentEnemyState { get; set; }
    public void Initialize(EnemyState startingState)
    {
        CurrentEnemyState = startingState;
        CurrentEnemyState.EnterState();
    }
    public void ChangeState(EnemyState state)
    {
        CurrentEnemyState.ExitState();
        CurrentEnemyState = state;
        CurrentEnemyState.EnterState();
    }
}
