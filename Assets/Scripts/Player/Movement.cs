using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour,IMovement
{
    [SerializeField] protected float _forwardSpeed = 5f;
    [SerializeField] protected float _lerpSpeed = 1f;
    [SerializeField] protected float _maxOffsetX;

    private Vector3 _startPosition;
    private Vector3 _pushDir;

    private float _pushForce;

    public Vector3 newPos;

    public bool _run = false;

    public float newXPos;
    public float offsetX;

    protected Rigidbody _rb;

    public Animator animator;

    
    public void OnEnable()
    {
        InputController.IsFirstInput += OnIsFirstInput;
        InputController.CharacterMove += OnCharacterMove;
    }

    public void OnDisable()
    {
        InputController.IsFirstInput -= OnIsFirstInput;
        InputController.CharacterMove -= OnCharacterMove;
    }
    public void OnIsFirstInput()
    {
        animator.SetFloat("speed", _forwardSpeed);
        _run = true;
    }

    private void OnCharacterMove(float targetPosX, float rightPosX, float leftPosX)
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x += targetPosX;

        if (_run)
        {
            newXPos = Mathf.Clamp(currentPosition.x + offsetX, leftPosX, rightPosX);
            newPos.x = newXPos;
        }

    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        _startPosition = transform.position;
        Debug.Log("start");
    }

    private void FixedUpdate()
    {
        if (_run)
            _rb.MovePosition(new Vector3(newPos.x, transform.position.y, transform.position.z + _forwardSpeed * Time.fixedDeltaTime));
    }

    public void HitPlayer(Vector3 velocityF, float time)
    {
        _rb.velocity = velocityF;

        _pushForce = velocityF.magnitude;
        _pushDir = Vector3.Normalize(velocityF);
        StartCoroutine(Decrease(velocityF.magnitude, time));
    }
    IEnumerator Decrease(float value, float duration)
    {
        float delta = 0;
        delta = value / duration;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            yield return null;
            _pushForce = _pushForce - Time.deltaTime * delta;
            _pushForce = _pushForce < 0 ? 0 : _pushForce;

            _rb.AddForce(new Vector3(0, -10 * GetComponent<Rigidbody>().mass, 0));
        }
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(2f);

        transform.gameObject.SetActive(false);
        transform.position = _startPosition;
        transform.gameObject.SetActive(true);
        _run = true;
        OnIsFirstInput();
    }
    IEnumerator FinishLine()
    {
        yield return new WaitForSeconds(1f);

        _run = false;
        _forwardSpeed = 0f;
        animator.SetTrigger("win");
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
                    offsetX = Mathf.Lerp(offsetX, _maxOffsetX, 3f * Time.fixedDeltaTime);
                }
                else if (obstacleDirection == RotatingPlatform.ObstacleDirection.Right2Left)
                {
                    offsetX = Mathf.Lerp(offsetX, -_maxOffsetX, 3f * Time.fixedDeltaTime);
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
            _run = false;
            StartCoroutine(Reset());
        }

        if (col.transform.gameObject.layer.Equals(LayerMaskExtensionMethods.LayerMask2Int(LayerManager.Instance.halfdonut)))
        {
            Debug.Log("Slide");
        }

        if (col.transform.gameObject.layer.Equals(LayerMaskExtensionMethods.LayerMask2Int(LayerManager.Instance.rotator)))
        {
            Debug.Log(col.contacts);
        }


    }
    public virtual void OnCollisionExit(Collision col)
    {
        if (col.transform.gameObject.layer.Equals(LayerMaskExtensionMethods.LayerMask2Int(LayerManager.Instance.halfdonut)))
        {
            _run = true;
        }

        if (col.transform.gameObject.layer.Equals(LayerMaskExtensionMethods.LayerMask2Int(LayerManager.Instance.rotateplatform)))
        {
            offsetX = 0;
        }

        if (col.transform.gameObject.layer.Equals(LayerMaskExtensionMethods.LayerMask2Int(LayerManager.Instance.finishline)))
        {
            StartCoroutine(FinishLine());
        }

    }
}
