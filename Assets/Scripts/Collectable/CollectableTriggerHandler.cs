using UnityEngine;

public class CollectableTriggerHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _whoCanCollect;
    private Collectable _collectable;

    private void Awake()
    {
        _collectable = GetComponent<Collectable>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (LayerMaskHelper.ObjIsInLayerMask(collision.gameObject, _whoCanCollect))
        {
            _collectable.Collect(collision.gameObject);



            Destroy(gameObject);

        }
    }
}
