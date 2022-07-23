using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject target; // Following object
    [SerializeField]
    private Vector3 distance; // Distance at target
    [SerializeField]
    private float _speed;

    // Update is called once per frame
    void LateUpdate()
    {
        //Follow target
        this.transform.position = Vector3.Lerp(this.transform.position, target.transform.position + distance, _speed * Time.deltaTime);
    }
}
