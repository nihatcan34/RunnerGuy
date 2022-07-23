using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuniorOpponent : Opponent
{
    float direct;
    float speed;
    bool run = true;
    private Animator animator;
    Vector3 startPosition;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        speed = 4f;
    }

    public override void Run()
    {
        if(run == true)
        {
            float posX = transform.position.x - direct * speed * Time.deltaTime;
            posX = Mathf.Clamp(posX, -7f, 7f);
            _rb.MovePosition(new Vector3(posX, transform.position.y, transform.position.z + 5f * Time.deltaTime));

            animator.SetFloat("speed", speed);
        }
       
    }

    public override void FixedUpdate()
    {
       // Debug.Log(transform.position.x);
        Run();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Obstacle")
        {
            Debug.Log("Hit Obstacle");
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1f);
            animator.SetTrigger("hitWall");
            run = false;
            StartCoroutine(Reset());
        }
    }


    IEnumerator Reset()
    {
        yield return new WaitForSeconds(2f);

        transform.gameObject.SetActive(false);
        transform.position = startPosition;
        transform.gameObject.SetActive(true);
        direct = 0;
        run = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            var direction = transform.InverseTransformPoint(other.transform.position);
            direct = Mathf.Clamp(direction.x, -1, 1);
            Debug.Log(direct + " trigger ");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            var direction = transform.InverseTransformPoint(other.transform.position);
            direct = 0f;
        }
    }
}
