using UnityEngine;

public interface IEnemyMovable
{
    Rigidbody2D rb { get; set; }
    bool isFacingRight { get; set; }
    void MoveEnemy(Vector2 vector2);
    void CheckForLeftOrRightFacing(Vector2 velocity);

}
