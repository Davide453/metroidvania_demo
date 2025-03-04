using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float knockbackTimer = 0.2f;
    public float hitDIrectionForce = 10f;
    public float constForce = 5f;
    public float inputForce = 7.5f;

    public bool isBeingKnockback { get; private set; } = false;
    private Rigidbody2D rb;
    private Coroutine KnockbackCoroutine;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public IEnumerator KnockBackAction(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
    {
        isBeingKnockback = true;
        Vector2 _hitForce;
        Vector2 _constantForce;
        Vector2 _knockbackForce;
        Vector2 _combinedForce;

        _hitForce = hitDirection * hitDIrectionForce;
        _constantForce = constantForceDirection * constForce;

        float _elapsedTime = 0f;
        while (_elapsedTime < knockbackTimer)
        {
            _elapsedTime += Time.fixedDeltaTime;
            _knockbackForce = _hitForce + _constantForce;


            if (inputDirection != 0)
            {
                _combinedForce = _knockbackForce + new Vector2(inputDirection + inputForce, 0f);
            }
            else
            {
                _combinedForce = _knockbackForce;
            }
            rb.linearVelocity = _combinedForce;

            yield return new WaitForFixedUpdate();
        }
        isBeingKnockback = false;
    }
    public void CallKnockbackRoutine(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)

    {
        KnockbackCoroutine = StartCoroutine(KnockBackAction(hitDirection, constantForceDirection, inputDirection));
    }
}
