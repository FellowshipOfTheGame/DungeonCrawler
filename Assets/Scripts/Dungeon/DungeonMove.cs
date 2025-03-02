using System.Collections;
using UnityEngine;

public class DungeonMove : MonoBehaviour
{
    public Dungeon dungeon;
    private Vector2Int position;
    
    private enum Direction {North = 0,East = 1,South = 2,West = 3}
    
    private readonly Vector2Int[] direction2D = {Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left};
    private readonly Vector3[] direction3D = {Vector3.forward, Vector3.right, Vector3.back, Vector3.left};
    
    public float timeToRotate = 0.5f;
    public float timeToMove = 0.5f;
    
    private Direction facingDirection = Direction.North;
    private Coroutine moveCoroutine,rotateCoroutine;

    public void SetPosition(int x, int y)
    {
        position = new Vector2Int(x, y);
        transform.position = new Vector3(x, 0, y);
    }

    private void Update()
    {
        if (moveCoroutine != null || rotateCoroutine != null) return;
            
        if (Input.GetKeyDown(KeyCode.D))
            rotateCoroutine = StartCoroutine(RotateCoroutine(true));
        else if(Input.GetKeyDown(KeyCode.A))
            rotateCoroutine = StartCoroutine(RotateCoroutine(false));

        if (Input.GetKeyDown(KeyCode.W))
            Move(Direction.North);
        else if (Input.GetKeyDown(KeyCode.S))
            Move(Direction.South);
        else if (Input.GetKeyDown(KeyCode.E))
            Move(Direction.East);
        else if(Input.GetKeyDown(KeyCode.Q))
            Move(Direction.West);
    }

    void Move(Direction direction)
    {
        int facingNum = (int)facingDirection;
        Direction moveDirection = (Direction)(((int)direction + facingNum) % 4);

        if(MoveWillCollide(moveDirection) || !IsMoveValid(moveDirection))
            return;
        
        moveCoroutine = StartCoroutine(MoveCoroutine(moveDirection));
    }

    private bool IsMoveValid(Direction moveDirection)
    { 
        (uint maxX, uint maxY) = dungeon.GetDimensions();
        Vector2Int newPosition = position + direction2D[(int)moveDirection];

        if (newPosition.x < 0 || newPosition.x >= maxX || newPosition.y < 0 || newPosition.y >= maxY)
            return false;
        return true;
    }
    
    private bool MoveWillCollide(Direction moveDirection)
    {
        DungeonTile currTile = dungeon.GetCell(position.x, position.y);
        
        switch (moveDirection)
        {
            case Direction.North:
                return currTile.sides.HasFlag(DungeonTile.Side.North);
            case Direction.East:
                return currTile.sides.HasFlag(DungeonTile.Side.East);
            case Direction.South:
                return currTile.sides.HasFlag(DungeonTile.Side.South);
            case Direction.West:
                return currTile.sides.HasFlag(DungeonTile.Side.West);
        }
        
        return false;
    }
    private IEnumerator MoveCoroutine(Direction moveDirection)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + direction3D[(int)moveDirection];
        
        float progress = 0;
        while(progress < timeToMove)
        {
            progress += Time.deltaTime;
            transform.position = startPos*(1-progress/timeToRotate) + endPos*(progress/timeToRotate) ; 
            yield return null;
        }
        
        transform.position = endPos;
        position += direction2D[(int)moveDirection];
        moveCoroutine = null;
    }

    private IEnumerator RotateCoroutine(bool right)
    {
        // '4+' is to avoid negative values
        facingDirection = (Direction)((4 + (int)facingDirection + (right ? 1 : -1))%4);
        
        Quaternion startRotation = transform.rotation;
        int endStep = 90 * (right ? 1 : -1);
        Quaternion endRotation = Quaternion.Euler(0, startRotation.eulerAngles.y + endStep, 0);
        
        float progress = 0;
        while(progress < timeToRotate)
        {
            progress += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, progress/timeToRotate); 
            yield return null;
        }
        
        transform.rotation = endRotation;
        rotateCoroutine = null;
    }

}
