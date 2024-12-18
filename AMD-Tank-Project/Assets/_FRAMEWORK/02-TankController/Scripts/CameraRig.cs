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

	[SerializeField] private Vector3 m_TargetOffset;

	private Vector3 m_SpringArmRotation;

    public void RotateSpringArm(Vector2 change)
	{
		m_SpringArmRotation += new Vector3(-change.y * m_Data.PitchSensitivity, change.x * m_Data.YawSensitivity);
        m_SpringArmRotation.x = Mathf.Clamp(m_SpringArmRotation.x, m_Data.MinPitch, m_Data.MaxPitch);
    }

	public void ChangeCameraDistance(float amount)
	{
		m_CameraDist += amount;
		//probably want to constrain this value
	}

	private void LateUpdate()
    {
        m_SpringArmKnuckle.rotation = Quaternion.Lerp(m_SpringArmKnuckle.rotation, Quaternion.Euler(m_SpringArmRotation.x, m_SpringArmRotation.y, 0.0f), 0.05f);

        //set the Knuckle to be the position of the tank plus the offset
        //REMEMBER: this script is ON THE TANK. It is pulling the camera each frame

        //Expand here by using a sphere trace from the tank backwards to see if the camera needs to move forward, out the way of geometry
    }

	public void AttachToTarget(Transform _target) => transform.SetParent(_target);
    public void DettachFromTarget() => transform.SetParent(null);
}