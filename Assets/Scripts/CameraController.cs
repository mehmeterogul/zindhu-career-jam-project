using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] Transform player;
	[SerializeField] Vector3 offset;
	[SerializeField] float smoothSpeed = 10f;

	// Update is called once per frame
	void LateUpdate()
	{
		Vector3 desiredPosition = player.position + offset;
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
		transform.position = smoothedPosition;
	}
}
