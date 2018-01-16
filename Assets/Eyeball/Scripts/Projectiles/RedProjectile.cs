using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedProjectile : Projectile
{
	public float horizontalLaunchForce;
	public float verticalLaunchForce;
	public float torqueForce;

	protected override void OnCollided(Collider2D collider, string collidableTag)
	{
		if (collidableTag != "Player" && collidableTag != "PlayerProjectile")
		{
			Destroy(gameObject);
		}
	}

	public override void Fire(Vector2 direction)
	{
		Vector2 force = new Vector2((direction.x == 0.0f ? 0.0f : Mathf.Sign(direction.x)) * horizontalLaunchForce, (direction.y == 0.0f ? 0.5f : Mathf.Sign(direction.y)) * verticalLaunchForce);
		rb.AddForce(force, ForceMode2D.Impulse);
		rb.AddTorque(-direction.x * torqueForce, ForceMode2D.Impulse);
	}
}