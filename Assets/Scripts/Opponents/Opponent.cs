using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Opponent : MonoBehaviour, IOpponent
{
    protected Rigidbody _rb;
    protected float directionX;
    

    public virtual void MoveHorizontal(float x)
    {
        directionX = x;
    }

    public void Restart()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Run()
    {
       //_rb.MovePosition(new Vector3((transform.position.x + directionX * Time.deltaTime), 0, transform.position.z + 5f * Time.deltaTime));
    }

    public virtual void FixedUpdate()
    {
        //Run();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        _rb =this.transform.GetComponent<Rigidbody>();
    }

}
