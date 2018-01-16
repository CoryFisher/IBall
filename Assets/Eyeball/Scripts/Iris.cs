using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iris : MonoBehaviour
{
	private Rigidbody2D rb;
	private Vector2 desiredDirection;
	private Vector2 currentDirection;

	public float irisMaxOffset;
	public float irisLerpCoefficient;

	private Sprite[] sprites = new Sprite[(int)AbilityColorType.Count];

	public SpriteRenderer baseRenderer;
	public SpriteRenderer overlayRenderer;

	public Sprite aliveSprite;
	public Sprite deadSprite;
	public Sprite redSprite;
	public Sprite yellowSprite;
	public Sprite greenSprite;
	public Sprite blueSprite;

	private void Awake()
	{
		// public components
		sprites[(int)AbilityColorType.Grey] = aliveSprite;
		//sprites[(int)AbilityColorType.Dead] = deadSprite;
		sprites[(int)AbilityColorType.Red] = redSprite;
		sprites[(int)AbilityColorType.Yellow] = yellowSprite;
		sprites[(int)AbilityColorType.Green] = greenSprite;
		sprites[(int)AbilityColorType.Blue] = blueSprite;

		// current type
		SetIrisType(AbilityColorType.Grey);
	}

	private void FixedUpdate()
	{
		currentDirection = Vector2.Lerp(currentDirection, desiredDirection, irisLerpCoefficient);
		var offset = currentDirection * irisMaxOffset;
		transform.localPosition = offset;
	}

	public void SetIrisType(AbilityColorType type)
	{
		if (type == AbilityColorType.Grey) // || type == AbilityColorType.Dead)
		{
			baseRenderer.sprite = sprites[(int)type];
			overlayRenderer.sprite = null;
		}
		else
		{
			overlayRenderer.sprite = sprites[(int)type];
		}
	}

	public void SetDesiredDirection(Vector2 direction)
	{
		if (direction != Vector2.zero)
		{
			desiredDirection = direction.normalized;
		}
	}

	public void SetDesiredDirectionRaw(Vector2 direction)
	{
		desiredDirection = direction;
	}

	public Vector2 GetCurrentDirection()
	{
		return currentDirection;
	}

	public Vector2 GetDesiredDirection()
	{
		return desiredDirection;
	}
}
