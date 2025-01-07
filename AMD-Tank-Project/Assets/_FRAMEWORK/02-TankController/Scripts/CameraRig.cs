using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
	[SerializeField] private Transform m_SpringArmKnuckle;
	[SerializeField] private Transform m_CameraMount;
	[SerializeField] private Camera m_Camera;

	[SerializeField] private CameraSO m_Data;

	private float m_CameraDist = 5f;

	private Transform m_Target;
	[SerializeField] private Vector3 m_TargetOffset;

	private Vector3 m_SpringArmRotation;

    private void Awake()
    {
		m_CameraDist = m_Data.MaxDist;
    }

    public void RotateSpringArm(Vector2 change)
	{
		m_SpringArmRotation += new Vector3(-change.y * m_Data.PitchSensitivity, change.x * m_Data.YawSensitivity);
        m_SpringArmRotation.x = Mathf.Clamp(m_SpringArmRotation.x, m_Data.MinPitch, m_Data.MaxPitch);
    }

	public void ChangeCameraDistance(float amount)
	{
		m_CameraDist = Mathf.Clamp(m_CameraDist + (amount * m_Data.ZoomSensitivity), m_Data.MinDist, m_Data.MaxDist);
		//probably want to constrain this value
	}

	private void LateUpdate()
    {
		if(m_Target != null) m_SpringArmKnuckle.position = m_Target.position + m_TargetOffset;
        m_SpringArmKnuckle.rotation = Quaternion.Lerp(m_SpringArmKnuckle.rotation, Quaternion.Euler(m_SpringArmRotation.x, m_SpringArmRotation.y, 0.0f), 0.05f);

		//set the Knuckle to be the position of the tank plus the offset
		//REMEMBER: this script is ON THE TANK. It is pulling the camera each frame

		RaycastHit hit;
		Physics.SphereCast(m_Target.position + m_TargetOffset, 1.0f, -m_SpringArmKnuckle.forward, out hit, m_CameraDist, m_Data.InteractLayers);
		
		m_CameraMount.position = (m_Target.position + m_TargetOffset) + 
			-m_SpringArmKnuckle.forward.normalized * (hit.collider != null ? hit.distance : m_CameraDist);

        //Expand here by using a sphere trace from the tank backwards to see if the camera needs to move forward, out the way of geometry
    }

	public void AttachToTarget(Transform _target) => m_Target = _target;
	public void DettachFromTarget() => m_Target = null;
	public Vector3 GetForward() => m_Camera.transform.forward;
}