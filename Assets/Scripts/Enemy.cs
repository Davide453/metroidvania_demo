using UnityEngine;

public class Enemy : BaseEnemy
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
        }
        else
        {

            if (patroDestination == 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[patroDestination].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, patrolPoints[patroDestination].position) < 0.2f)
                {
                    patroDestination = 1;
                }
            }
            if (patroDestination == 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[patroDestination].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, patrolPoints[patroDestination].position) < 0.2f)
                {
                    patroDestination = 0;
                }
            }

        }
     
    }
    private void OnDrawGizmos()
    {
      /*  Gizmos.color = Color.blue;
        Gizmos.draw(groundCheckPos.position, groundCheckSize); Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
        Gizmos.color = Color.red;*/
      
    }
}
