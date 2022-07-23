using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _forwardSpeed = 5f;
    [SerializeField] float _lerpSpeed = 5f;
    [SerializeField] float _playerXValue = 2f;
    [SerializeField] Vector2 _clampValues = new Vector2(-7, 7);
    Vector3 startPosition;

    private float _newXPos;
    private float _startXPos;
    private float _startFingerPosition;
    private float _moveFingerDistance;
    private float _lastFingerPosition;

    private float offsetX;
    [SerializeField]private float maxOffsetX;

    private float pushForce;
    private Vector3 pushDir;

    public Animator animator;
    private Rigidbody _rb;

    private bool run = true;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        _startXPos = transform.position.x;
    }

    private void OnEnable()
    {
        InputController.CharacterMove += OnCharacterMove;
        InputController.IsFirstInput += OnIsFirstInput;
    }


    private void OnDisable()
    {
        InputController.CharacterMove -= OnCharacterMove;
        InputController.IsFirstInput -= OnIsFirstInput;
    }

    private void OnIsFirstInput()
    {
        animator.SetFloat("speed", 5f);
    }

    private void OnCharacterMove(float targetPosX, float rightPosX, float leftPosX)
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x += targetPosX;

        _newXPos = Mathf.Clamp(currentPosition.x + offsetX,leftPosX,rightPosX);

        if (run)
        {
            Vector3 targetPos = transform.position;
            targetPos.z += _forwardSpeed;
            Vector3 newPos = Vector3.MoveTowards(transform.position,targetPos, _lerpSpeed * Time.fixedDeltaTime);
            newPos.x = _newXPos;
            _rb.MovePosition(newPos);
        }

    }


    // Update is called once per frame
   /* void Update()
    {
        Debug.Log(Input.mousePosition.x + " , " + _startFingerPosition + " , " + _newXPos + " , " + _moveFingerDistance);
        //if (Input.touchCount > 1)
        //    return;

        //if (_lastFingerPosition == Input.mousePosition.x)
        //    return;

        //if (Input.GetButtonDown("Horizontal"))
        //{
        //    //_newXPos = Mathf.Clamp(transform.position.x + Input.GetAxisRaw("Horizontal") * _playerXValue, _startXPos + _clampValues.x, _startXPos + _clampValues.y);
        //}

        //if (Input.GetMouseButtonDown(0))
        //{
        //    _startFingerPosition = Input.mousePosition.x;
        //}
        //else if (Input.GetMouseButton(0))
        //{
        //    _moveFingerDistance = Mathf.Clamp(_startFingerPosition - Input.mousePosition.x, -1, 1);

        //    _newXPos = Mathf.Clamp(transform.position.x + _moveFingerDistance * -_playerXValue, _startXPos + _clampValues.x, _startXPos + _clampValues.y);
        //}
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    _moveFingerDistance = 0f;
        //}

        //_lastFingerPosition = Input.mousePosition.x;
    }*/

    private void Update()
    {
        // Player Movement
        if (run)
        {
            
        }
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.transform.gameObject.layer.Equals(LayerMaskExtensionMethods.LayerMask2Int(LayerManager.Instance.rotateplatform)))
        {
            //transform.parent = col.transform;
            RotatingPlatform rotatingPlatform = col.gameObject.GetComponent<RotatingPlatform>();
            if(rotatingPlatform != null)
            {
                RotatingPlatform.ObstacleDirection obstacleDirection = rotatingPlatform.getObstacleDirection();
                
                if(obstacleDirection == RotatingPlatform.ObstacleDirection.Left2Right)
                {
                    offsetX = Mathf.MoveTowards(offsetX, maxOffsetX, 3f * Time.fixedDeltaTime);
                }
                else if(obstacleDirection == RotatingPlatform.ObstacleDirection.Right2Left)
                {
                    offsetX = Mathf.MoveTowards(offsetX, -maxOffsetX, 3f * Time.fixedDeltaTime);
                }
            }
        }
    }


    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.gameObject.layer.Equals(LayerMaskExtensionMethods.LayerMask2Int(LayerManager.Instance.obstacle)))
        {
            Debug.Log("Hit Obstacle");
            animator.SetTrigger("hitWall");
            run = false;
            StartCoroutine(Reset());
        }

        if (col.transform.gameObject.layer.Equals(LayerMaskExtensionMethods.LayerMask2Int(LayerManager.Instance.halfdonut)))
        {
            Debug.Log("Slide");
            float direct;
            run = false;
            var direction = transform.InverseTransformPoint(col.transform.position);
            direct = Mathf.Clamp(direction.x, -1, 1);
            _newXPos = direct;
        }

        if (col.transform.gameObject.layer.Equals(LayerMaskExtensionMethods.LayerMask2Int(LayerManager.Instance.rotator)))
        {
            Debug.Log(col.contacts);
        }


    }

    public void HitPlayer(Vector3 velocityF, float time)
    {
        _rb.velocity = velocityF;

        pushForce = velocityF.magnitude;
        pushDir = Vector3.Normalize(velocityF);
        StartCoroutine(Decrease(velocityF.magnitude, time));
    }

    private IEnumerator Decrease(float value, float duration)
    {
        float delta = 0;
        delta = value / duration;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            yield return null;
                pushForce = pushForce - Time.deltaTime * delta;
                pushForce = pushForce < 0 ? 0 : pushForce;
                //Debug.Log(pushForce);
            
            _rb.AddForce(new Vector3(0, -10 * GetComponent<Rigidbody>().mass, 0)); //Add gravity
        }
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(2f);

        transform.gameObject.SetActive(false);
        transform.position = startPosition;
        transform.gameObject.SetActive(true);
        run = true;
        OnIsFirstInput();
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.transform.tag == "Half-Donut")
        {
            run = true;
        }

        if (col.transform.gameObject.layer.Equals(LayerMaskExtensionMethods.LayerMask2Int(LayerManager.Instance.rotateplatform)))
        {
            offsetX = 0;
        }
    }
}
