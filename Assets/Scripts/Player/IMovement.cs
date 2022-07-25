
using System.Collections;
using UnityEngine;

public interface IMovement
{
    void OnIsFirstInput();
    void HitPlayer(Vector3 velocityF, float time);
}
