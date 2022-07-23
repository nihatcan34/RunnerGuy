using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorObstacle : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.down * _direction * _speed * Time.deltaTime);
    }
}
