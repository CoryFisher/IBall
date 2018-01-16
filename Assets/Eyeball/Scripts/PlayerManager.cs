using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum AbilityColorType
{
	Grey,
	Red,
	Green,
	Blue,
	Yellow,

	Count
}

/// <summary>
/// Controls spawning the player, killing the player, slowing time after death, ?reloading the level?
/// </summary>
public class PlayerManager : Singleton<PlayerManager>
{
	// private components
	private GameObject playerObject;
	private Rigidbody2D playerRigidbody;
	private MovementController playerMovementController;
	private Player player;
	private Eyeball eyeball;
	private Iris iris;
	private FollowCamera cam;
	private GameObject spawnPoint;

	private Vector2 inputDirection;
	private AbilityColorType currentType;
	private GameObject aimingProjectile;

	// public components
	public GameObject playerPrefab;
	public GameObject projectilePrefab;

	// public data
	public float playerKillPlaneHeight;
	public int sceneReloadDelaySeconds;
	public float slowTimeAfterDeath_InitialTimescale;
	public float slowTimeAfterDeath_MinTimescale;
	public float slowTimeAfterDeath_ReductionRate;

	public Player Player { get { return player; } }

	private void Awake ()
	{
		RegisterSingletonInstance(this);

		// get spawnpoint
		spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");

		// spawn player and deactivate
		playerObject = Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity);

		// get controller scripts from gameobject
		player = playerObject.GetComponent<Player>();
		player.OnDeathEvent += Player_OnDeathEvent;

		playerMovementController = playerObject.GetComponent<MovementController>();
		playerRigidbody = playerObject.GetComponent<Rigidbody2D>();

		eyeball = playerObject.GetComponentInChildren<Eyeball>();
		iris = playerObject.GetComponentInChildren<Iris>();

		// get camera
		cam = Camera.main.GetComponent<FollowCamera>();
		cam.ObjectToFollow = playerObject;
	}

	private void Update ()
	{
		if (!player.IsDead && playerObject.transform.position.y < playerKillPlaneHeight)
		{
			player.TakeDamage(player.health);
		}

		if (Input.GetButtonDown("Fire1"))
		{
			playerMovementController.IsAiming = true;
			aimingProjectile = Instantiate(projectilePrefab, iris.transform);
			aimingProjectile.GetComponent<Rigidbody2D>().simulated = false;
		}
		else if (Input.GetButtonUp("Fire1"))
		{
			aimingProjectile.transform.parent = null;
			aimingProjectile.GetComponent<Rigidbody2D>().simulated = true;
			var projectileScript = aimingProjectile.GetComponent<Projectile>();
			projectileScript.Fire(iris.GetDesiredDirection());
			playerMovementController.IsAiming = false;
		}

		if (Input.GetButtonDown("Fire3"))
		{
			currentType = (AbilityColorType)(((int)currentType + 1) % (int)AbilityColorType.Count);
			iris.SetIrisType(currentType);
		}
	}

	public void NotifyInputDirection(Vector2 direction)
	{
		iris.SetDesiredDirection(direction);
		inputDirection = direction;
	}

	private void Player_OnDeathEvent()
	{
		playerMovementController.enabled = false;
		playerRigidbody.simulated = false;
		eyeball.SetEyeballType(EyeballType.Dead);

		StartCoroutine("EyeRollAfterDeath");
		StartCoroutine("SlowTimeAfterDeath");
		StartCoroutine("ReloadLevelAfterSeconds", sceneReloadDelaySeconds);
	}

	private IEnumerator EyeRollAfterDeath()
	{	
		Vector2 dir = iris.GetCurrentDirection();
		Vector2 end = Vector2.up * 0.6f;
		float t = 0f;
		while (dir != end)
		{
			t += Time.deltaTime;
			iris.SetDesiredDirectionRaw(Vector2.Lerp(dir, end, t));
			yield return null;
		}
	}

	private IEnumerator SlowTimeAfterDeath()
	{
		Time.timeScale = slowTimeAfterDeath_InitialTimescale;
		while (Time.timeScale > slowTimeAfterDeath_MinTimescale)
		{
			Time.timeScale -= slowTimeAfterDeath_ReductionRate * Time.unscaledDeltaTime;
			yield return null;
		}
	}

	private IEnumerator ReloadLevelAfterSeconds(int time)
	{
		yield return new WaitForSecondsRealtime(time);
		Time.timeScale = 1.0f;
		yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
	}
}
