using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float healthTimer = 5f;
    [SerializeField] private float bulletDamage = 1f;

    private float _timer;
    [SerializeField] private LayerMask _whoCanDamage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _timer += Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > healthTimer)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMaskHelper.ObjIsInLayerMask(collision.gameObject, _whoCanDamage))
        {
            collision.GetComponent<IDamageable>().Damage(bulletDamage, transform.right * -1);

            Destroy(gameObject);

        }
    }

}
