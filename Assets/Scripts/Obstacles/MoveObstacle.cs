using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    private float _moveSpeed;
    private float _objectPositionX;
    [SerializeField]private float _limitPositionNegative = -7f;
    [SerializeField]private float _limitPositionPositive= 7f;

    void Start()
    {
        _objectPositionX = transform.position.x;
        _moveSpeed = Random.Range(0.3f,0.6f);
    }
    public void Move()
    {
        _objectPositionX = Mathf.Lerp(_limitPositionNegative,_limitPositionPositive, Mathf.PingPong(Time.time * _moveSpeed, 1.0f));
        transform.position = new Vector3(_objectPositionX,transform.position.y,transform.position.z);
    }

    private void Update()
    {
        Move();
    }
}
