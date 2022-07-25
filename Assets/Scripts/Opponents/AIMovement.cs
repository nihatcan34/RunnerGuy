using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : Movement
{
    private float _direct;
    private float _posX;
    
    public void Run()
    {
        if (_run == true)
        {
            _posX = transform.position.x - _direct * _forwardSpeed * Time.deltaTime;
            _posX = Mathf.Clamp(_posX, -7f, 7f);
            _rb.MovePosition(new Vector3(_posX, transform.position.y, transform.position.z + _forwardSpeed * Time.fixedDeltaTime));
        }

    }

    public void FixedUpdate()
    {
        Run();
    }

    IEnumerator FinishLine()
    {
        yield return new WaitForSeconds(1f);

        _run = false;
        _forwardSpeed = 0f;
        animator.SetTrigger("win");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.layer.Equals(LayerMaskExtensionMethods.LayerMask2Int(LayerManager.Instance.obstacle)))
        {
            var direction = transform.InverseTransformPoint(other.transform.position);
            float moveLeft = Random.Range(-2f, -0.5f);
            float moveRight = Random.Range(0.5f,2f);
            _direct = Mathf.Clamp(direction.x, moveLeft, moveRight);
            Debug.Log(_direct);
        }
    }
    private void OnCollisionStay(Collision col)
    {
        if (col.transform.gameObject.layer.Equals(LayerMaskExtensionMethods.LayerMask2Int(LayerManager.Instance.rotateplatform)))
        {
            RotatingPlatform rotatingPlatform = col.gameObject.GetComponent<RotatingPlatform>();
            if (rotatingPlatform != null)
            {
                RotatingPlatform.ObstacleDirection obstacleDirection = rotatingPlatform.getObstacleDirection();

                if (obstacleDirection == RotatingPlatform.ObstacleDirection.Left2Right)
                {
                    _direct = Mathf.Lerp(_direct, -_maxOffsetX, 3f * Time.fixedDeltaTime);
                }
                else if (obstacleDirection == RotatingPlatform.ObstacleDirection.Right2Left)
                {
                    _direct = Mathf.Lerp(_direct, _maxOffsetX, 3f * Time.fixedDeltaTime);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.layer.Equals(LayerMaskExtensionMethods.LayerMask2Int(LayerManager.Instance.obstacle)))
        {
            var direction = transform.InverseTransformPoint(other.transform.position);
            _direct = 0f;
        }
    }

    public override void OnCollisionExit(Collision col)
    {
        _direct = 0f;
        base.OnCollisionExit(col);
    }
}
