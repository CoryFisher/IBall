using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public int health;
	public int maxHealth;
	public bool IsDead { get { return health < 1; } }

	public delegate void OnTakeDamageHandler();
	public event OnTakeDamageHandler OnTakeDamageEvent;

	public delegate void OnDeathHandler();
	public event OnDeathHandler OnDeathEvent;

	private void Awake()
	{
		health = maxHealth;
	}

	private void OnDestroy()
	{
		OnTakeDamageEvent = null;
		OnDeathEvent = null;
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
		if (health < 1)
		{
			Die();
		}
		else
		{
			OnTakeDamageEvent.Invoke();
		}
	}
	
	private void Die()
	{
		OnDeathEvent.Invoke();
	}

}
