using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [SerializeField] private float _speed;
    private float _direction;

    public enum ObstacleDirection
    {
        Left2Right,
        Right2Left
    }

    [SerializeField] ObstacleDirection obstacleDirection;

    public ObstacleDirection getObstacleDirection() { return obstacleDirection; }


    // Start is called before the first frame update
    void Start()
    {
        if(obstacleDirection == ObstacleDirection.Left2Right)
        {
            _direction = -1;
        }
        else if (obstacleDirection == ObstacleDirection.Right2Left)
        {
            _direction = 1;
        }
            
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * _direction * _speed * Time.deltaTime);
    }
}
