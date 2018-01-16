using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	public float screenScrollDistance;
	public GameObject ObjectToFollow;

	private void Update()
	{
		if (ObjectToFollow == null)
		{
			return;
		}

		Transform transformToFollow = ObjectToFollow.transform;
		if (transformToFollow.position.x > transform.position.x + screenScrollDistance)
		{
			transform.position = new Vector3(transformToFollow.position.x - screenScrollDistance, transform.position.y, transform.position.z);
		}
		else if (transformToFollow.position.x < transform.position.x - screenScrollDistance)
		{
			transform.position = new Vector3(transformToFollow.position.x + screenScrollDistance, transform.position.y, transform.position.z);
		}

		if (transformToFollow.position.y > transform.position.y + screenScrollDistance)
		{
			transform.position = new Vector3(transform.position.x, transformToFollow.position.y - screenScrollDistance, transform.position.z);
		}
		else if (transformToFollow.position.y < transform.position.y - screenScrollDistance)
		{
			transform.position = new Vector3(transform.position.x, transformToFollow.position.y + screenScrollDistance, transform.position.z);
		}
	}
}
