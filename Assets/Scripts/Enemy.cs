using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float moveSpeed;
    public int patroDestination;
    private RaycastHit2D hit;
    private bool foundPlayer;
    [SerializeField] private float playerCheckRadius;





    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(transform.position, patrolPoints[patroDestination].position, playerCheckRadius, playerLayer);

        foundPlayer = hit.collider != null;
        if (foundPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, hit.transform.position, moveSpeed * Time.deltaTime);
            //Debug.Log("found player");
        }
        else
        {

            if (patroDestination == 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[patroDestination].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, patrolPoints[patroDestination].position) < 0.2f)
                {
                    //Debug.Log("close");
                    patroDestination = 1;
                }
            }
            if (patroDestination == 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[patroDestination].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, patrolPoints[patroDestination].position) < 0.2f)
                {
                    //Debug.Log("close");
                    patroDestination = 0;
                }
            }
        }
    }
}
