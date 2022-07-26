﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
	public float force = 10f; //Force 10000f
	public float stunTime = 5.5f;
	private Vector3 hitDir;

	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			if (collision.gameObject.tag == "Player")
			{
				hitDir = contact.normal;
				collision.gameObject.GetComponent<CharacterMovement>().HitPlayer(-hitDir * force, stunTime);
				return;
			}
			if (collision.gameObject.tag == "Opponent")
			{
				hitDir = contact.normal;
				collision.gameObject.GetComponent<AIMovement>().HitPlayer(-hitDir * force, stunTime);
				return;
			}
		}
		
	}
}
