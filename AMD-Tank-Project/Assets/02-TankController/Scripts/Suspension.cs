using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{
	public event Action<bool> OnGroundedChanged; 

	[SerializeField] private Transform m_Wheel;
	[SerializeField] private Rigidbody m_RB;

	private SuspensionSO m_Data;
	private float m_SpringSize;
	private bool m_Grounded;

	public void Init(SuspensionSO inData, Rigidbody _RBRef)
	{
		m_RB ??= _RBRef;
		m_Data = inData;

		m_SpringSize = (m_Data.WheelDiameter / 2f) + Mathf.Abs(m_Wheel.localPosition.y);
	}

	public bool GetGrounded()
	{
		return m_Grounded;
	}

	private void FixedUpdate()
	{
		//start this function by using the StatefulRaycast2D from IMD to work out how to do a grounded check using the springSize as a length and fire the event when the value for grounded changes
		//use the result of this ground check for the suspension spring
		//TIP: the tank is the moving part of the spring not the floor, draw the diagram
		//The tanks mass never changes either so is there any need to simulate forces in ForceMode.Force maybe ForceMode.Acceleration would keep the numbers smaller and easier to deal with????

		Debug.DrawRay(transform.position, -transform.up.normalized * m_SpringSize, Color.blue);

		RaycastHit hit;
		if (Physics.Raycast(transform.position, -transform.up.normalized, out hit, m_SpringSize, m_Data.SuspensionLayermask))
		{
			Vector3 worldVel = m_RB.GetPointVelocity(transform.position);

			float susOffset = m_SpringSize - hit.distance;

			float susVel = Mathf.Abs(Vector3.Dot(worldVel, (transform.localRotation * Vector3.down).normalized));

			float susForce = (susOffset * m_Data.SuspensionStrength) - (susVel * m_Data.SuspensionDamper);

			m_RB.AddForceAtPosition(-(transform.localRotation * Vector3.down) * susForce, transform.position, ForceMode.Acceleration);
        }

		//to stop the tank from sliding you also need to conssider how much velocity is in the left/right direction and counter it here
	}
}
