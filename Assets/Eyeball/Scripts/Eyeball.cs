using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EyeballType
{
	Alive,
	Dead,

	Count
}

public class Eyeball : MonoBehaviour
{
	private Sprite[] sprites = new Sprite[(int)EyeballType.Count];
	private EyeballType currentType;

	public SpriteRenderer spriteRenderer;
	public Sprite aliveSprite;
	public Sprite deadSprite;

	private void Awake()
	{
		// public components
		sprites[(int)EyeballType.Alive] = aliveSprite;
		sprites[(int)EyeballType.Dead] = deadSprite;

		// current type
		currentType = EyeballType.Alive;
		SetEyeballType(currentType);
	}

	public void SetEyeballType(EyeballType type)
	{
		spriteRenderer.sprite = sprites[(int)type];
	}
}
