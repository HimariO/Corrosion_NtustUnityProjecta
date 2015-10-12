using UnityEngine;
using System.Collections;

public class camFollow : MonoBehaviour
{
	public Transform target;            // The position that that camera will be following.
	public float smoothing = 5f;        // The speed with which the camera will be following.
	
	Vector3 offset;                     // The initial offset from the target.
	
	void Start ()
	{
		// Calculate the initial offset.
		offset = transform.position - target.position;
	}
	
	void FixedUpdate ()
	{
		// Create a postion the camera is aiming for based on the offset from the target.
		Vector3 targetCamPos = target.position + offset;
		
		// Smoothly interpolate between the camera's current position and it's target position.
		Follow ();
		

	}

	private void Follow(){
		
		// orientation as an angle when projected onto the XZ plane
		// this functionality is modularise into a separate method because
		// I use it elsewhere
		float playerAngle = AngleOnXZPlane (target);
		float cameraAngle = AngleOnXZPlane (transform);
		
		// difference in orientations
		float rotationDiff = Mathf.DeltaAngle(cameraAngle, playerAngle);
		
		// rotate around target by time-sensitive difference between these angles
		transform.RotateAround(transform.position, Vector3.up, rotationDiff);
	}
	
	// Find the angle made when projecting the rotation onto the xz plane.
	// You could pass in the rotation as a parameter instead of the transform.
	private float AngleOnXZPlane(Transform item){
		// get rotation as vector (relative to parent)
		Vector3 direction = item.rotation * target.forward;
		
		// return angle in degrees when projected onto xz plane
		return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
	}
}