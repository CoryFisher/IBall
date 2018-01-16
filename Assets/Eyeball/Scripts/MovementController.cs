using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Listens to input and controls the physics of the object
/// </summary>
public class MovementController : MonoBehaviour
{
	private Rigidbody2D rb;
	private float horizontal;
	private float vertical;
	private bool grounded;
	private bool jump;
	private bool canAirJump;
	private bool airJump;

	public float moveForce;
	public float maxSpeed;
	public float jumpForce;
	public float movementRestitutionCoefficient;

	public bool IsAiming { get; set; }

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			grounded = true;
			canAirJump = true;
		}
	}

	private void Update()
	{
		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw("Vertical");

		// snap
		if (horizontal < -0.1f)
		{
			horizontal = -1.0f;
		}
		else if (horizontal > 0.1f)
		{
			horizontal = 1.0f;
		}
		else
		{
			horizontal = 0.0f;
		}

		if (vertical < -0.1f)
		{
			vertical = -1.0f;
		}
		else if (vertical > 0.1f)
		{
			vertical = 1.0f;
		}
		else
		{
			vertical = 0.0f;
		}

		PlayerManager.Get().NotifyInputDirection(new Vector2(horizontal, vertical));
	}

	private void FixedUpdate()
	{
		if (IsAiming)
		{
			horizontal = 0.0f;
		}

		if (horizontal != 0.0f)
		{
			// move
			if (horizontal * rb.velocity.x < maxSpeed)
			{
				rb.AddForce(Vector2.right * horizontal * moveForce);
			}

			// limit movement if grounded
			if (grounded && Mathf.Abs(rb.velocity.x) > maxSpeed)
			{
				rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
			}
		}
		else if (grounded)
		{
			rb.velocity = new Vector2(rb.velocity.x * movementRestitutionCoefficient, rb.velocity.y);
		}

		jump = grounded && Input.GetButton("Jump");
		airJump = !grounded && canAirJump && Input.GetButtonDown("Jump");

		if (jump)
		{
			//rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			grounded = false;
		}
		else if (airJump)
		{
			// slam
			if (vertical < 0.0f)
			{
				//rb.AddForce(new Vector2(horizontal * jumpForce, -jumpForce), ForceMode2D.Impulse);
				rb.velocity = new Vector2(horizontal * jumpForce, -jumpForce);
			}
			// dash
			else if (vertical == 0.0f && horizontal != 0.0f)
			{
				rb.velocity = new Vector2(horizontal * jumpForce, rb.velocity.y);
			}
			// double-jump
			else
			{
				//rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
				rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			}

			canAirJump = false;
		}
	}
}
