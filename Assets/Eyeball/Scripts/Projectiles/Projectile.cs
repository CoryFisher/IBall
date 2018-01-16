using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
	protected Rigidbody2D rb;

	public AbilityColorType type;
	public List<string> collidableTags;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collidableTags != null && collidableTags.Count > 0)
		{
			foreach (var collidableTag in collidableTags)
			{
				if (collider.gameObject.CompareTag(collidableTag))
				{
					OnCollided(collider, collidableTag);
					break;
				}
			}
		}
	}

	protected abstract void OnCollided(Collider2D collider, string collidableTag);
	public abstract void Fire(Vector2 direction);
}
