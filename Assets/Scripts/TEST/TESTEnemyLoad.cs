using UnityEngine;

public class TESTEnemyLoad : MonoBehaviour
{
    public Character enemy;
    void Start()
    {
        var _enemy = gameObject.AddComponent<Enemy>();
        _enemy.Init(enemy); 
    }
}
