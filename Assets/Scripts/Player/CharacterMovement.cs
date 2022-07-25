using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : Movement
{
    public override void Finish()
    {
        _run = false;
        _forwardSpeed = 0f;
        animator.SetTrigger("win");
        UIManager.Instance.ShowRank(rank);
    }
}
