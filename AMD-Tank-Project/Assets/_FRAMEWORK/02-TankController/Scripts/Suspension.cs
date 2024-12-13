using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Suspension : MonoBehaviour
{
	public event Action<bool> OnGroundedChanged; 

	[SerializeField] private Transform m_Wheel;
	[SerializeField] private Rigidbody m_RB;

	private SuspensionSO m_Data;
	private float m_SpringRestingDistance;
	private bool m_Grounded;

	public void Init(SuspensionSO inData, Rigidbody _RBRef)
	{
		m_RB ??= _RBRef;
		m_Data = inData;

		m_SpringRestingDistance = (m_Data.WheelDiameter / 2f) + Mathf.Abs(m_Wheel.localPosition.y);
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

		Debug.Assert(m_Data != null, $"Data asset missing from suspension! ({this})");

		Debug.DrawRay(transform.position, -transform.up.normalized * m_SpringRestingDistance, Color.blue);

		RaycastHit hit;
		if (Physics.Raycast(transform.position, -transform.up.normalized, out hit, m_SpringRestingDistance, m_Data.SuspensionLayermask))
		{
			m_Wheel.position = hit.point + (Vector3.up * (m_Data.WheelDiameter / 2f));
			Vector3 springDir = m_Wheel.up;
			Vector3 tireWorldVel = m_RB.GetPointVelocity(m_Wheel.position);

			float offset = m_SpringRestingDistance - hit.distance;
			float vel = Vector3.Dot(springDir, tireWorldVel);
			float force = (offset * m_Data.SuspensionStrength) - (vel * m_Data.SuspensionDamper);
			m_RB.AddForceAtPosition(springDir * force, m_Wheel.position, ForceMode.Acceleration);
		}

		if (m_Grounded != hit.collider)
		{
			m_Grounded = hit.collider;
			OnGroundedChanged?.Invoke(m_Grounded);
		}

		// Stop sliding
		Vector3 localVelocity = transform.InverseTransformDirection(m_RB.linearVelocity);
		float lateralVelocity = localVelocity.x;
		Vector3 lateralForce = -transform.right * lateralVelocity * 10.0f;

		m_RB.AddForceAtPosition(lateralForce, m_Wheel.position, ForceMode.Acceleration);

        //Friction
        //float FloorAngle = Vector3.Angle(hit.normal, Vector3.up);
        //if (FloorAngle < m_Data.MaximumSlope)
        //{
        //    float ObjectDownForce = 9.8f * m_RB.mass * Mathf.Cos(FloorAngle * Mathf.Deg2Rad);
        //    float FrictionForce = m_Data.FrictionCoefficient * ObjectDownForce;
        //    Vector3 Friction = -m_RB.linearVelocity.normalized * FrictionForce;
		//
        //    m_RB?.AddForceAtPosition(Friction, transform.position, ForceMode.Force);
        //}

    }
}
