using UnityEngine;

public class Enemy_ : MonoBehaviour, IDamageable, IEnemyMovable, ITriggerCheckable
{
    #region Stats
    public virtual float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }
    public bool isFacingRight { get; set; }
    #endregion

    public Rigidbody2D rb { get; set; }
    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }




    #region ScriptableObjects Logic
    [SerializeField] private EnemyIdleSOBase EnemyIdleBase;
    [SerializeField] private EnemyChaseSOBase EnemyChaseBase;
    [SerializeField] private EnemyAttackSOBase EnemyAttackBase;

    public EnemyIdleSOBase EnemyIdleInstance { get; set; }
    public EnemyChaseSOBase EnemyChaseInstance { get; set; }
    public EnemyAttackSOBase EnemyAttackInstance { get; set; }

    #endregion
    void Awake()
    {
        EnemyIdleInstance = Instantiate(EnemyIdleBase);
        EnemyChaseInstance = Instantiate(EnemyChaseBase);
        EnemyAttackInstance = Instantiate(EnemyAttackBase);

        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
    }
    void Start()
    {
        CurrentHealth = MaxHealth;
        rb = GetComponent<Rigidbody2D>();

        EnemyIdleInstance.Initialize(gameObject, this);
        EnemyChaseInstance.Initialize(gameObject, this);
        EnemyAttackInstance.Initialize(gameObject, this);

        StateMachine.Initialize(IdleState);

    }
    private void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }
    public void CheckForLeftOrRightFacing(Vector2 velocity)
    {

    }

    public void Damage(float damageAmount, Vector2 pos)
    {
        CurrentHealth -= damageAmount;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void MoveEnemy(Vector2 vector2)
    {
        rb.linearVelocity = vector2;
    }
    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        //todo
    }

    #region Distance Checks
    public void SetAggroStatus(bool isAggroed)
    {
        IsAggroed = isAggroed;
    }

    public void SetStrikingDistanceBool(bool isWithinStrikingDistance)
    {
        IsWithinStrikingDistance = isWithinStrikingDistance;
    }
    #endregion
    public enum AnimationTriggerType
    {
        EnemySamaged,
        PlayFootstepSound
    }
}
