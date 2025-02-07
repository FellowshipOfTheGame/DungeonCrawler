using System;using UnityEngine;


public enum EnemyPosition{
    Left,Center,Right
}

public class EnemyLayoutLoader : MonoBehaviour
{
    public GameObject LeftPosition, CenterPosition, RightPosition;

    public Vector3 GetPosition(EnemyPosition position)
    {
        switch (position)
        {
            case EnemyPosition.Left:
                return LeftPosition.transform.position;
            case EnemyPosition.Center:
                return CenterPosition.transform.position;
            case EnemyPosition.Right:
                return RightPosition.transform.position; 
        }

        throw new Exception("Position is invalid;");
    }
}
