using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float maxLife = 5f;
    private float _timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _timer += Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > maxLife)
        {
            Destroy(gameObject);
        }
    }
}
