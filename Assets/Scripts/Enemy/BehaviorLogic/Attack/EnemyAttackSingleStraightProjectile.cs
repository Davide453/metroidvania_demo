using UnityEngine;

[CreateAssetMenu(fileName = "Attack-Straight-Single Projectile", menuName = "Enemy Logic/Attack logic/Straight Single Projectile")]
public class EnemyAttackSingleStraightProjectile : EnemyAttackSOBase
{
    [SerializeField] private Rigidbody2D BulletPrefab;
    [SerializeField] private float _timer;
    [SerializeField] private float _timeBetweenShots = 1f;
    [SerializeField] private float _exitTimer;
    [SerializeField] private float _timeTillExit = 2f;
    [SerializeField] private float _distanceToCountExit = 3f;
    [SerializeField] private float _bulletSpeed = 30f;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        enemy.MoveEnemy(Vector2.zero);
        if (_timer > _timeBetweenShots)
        {
            _timer = 0f;
            Vector2 dir = (playerTransform.position - enemy.transform.position).normalized;
            Rigidbody2D bullet = GameObject.Instantiate(BulletPrefab, enemy.transform.position, Quaternion.identity);
            bullet.linearVelocity = dir * _bulletSpeed;
        }
        if (Vector2.Distance(playerTransform.position, enemy.transform.position) > _distanceToCountExit)
        {
            _exitTimer += Time.deltaTime;
            if (_exitTimer > _timeTillExit)
            {
                enemy.StateMachine.ChangeState(enemy.ChaseState);
            }
        }
        else
        {
            _exitTimer = 0f;
        }
        _timer += Time.deltaTime;

    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
    }

    public override void Initialize(GameObject gameObject, Enemy_ enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
