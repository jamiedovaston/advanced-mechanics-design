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
			m_Wheel.position = hit.point + (transform.up.normalized * (m_Data.WheelDiameter / 2f));
			Vector3 springDir = transform.up;
			Vector3 tyreWorldVel = m_RB.GetPointVelocity(m_Wheel.position);

			float offset = m_SpringRestingDistance - hit.distance;
			float vel = Vector3.Dot(springDir, tyreWorldVel);
			float slipVel = Vector3.Dot(tyreWorldVel, transform.right);
			float slipForce = slipVel * -0.9f;
			float force = (offset * m_Data.SuspensionStrength) - (vel * m_Data.SuspensionDamper);
			m_RB.AddForceAtPosition((springDir * force) + (transform.right * slipForce), m_Wheel.position, ForceMode.Acceleration);

            float floorAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (floorAngle < m_Data.MaximumSlope)
            {
                float objectDownForce = Mathf.Abs(Physics.gravity.y) * m_RB.mass * Mathf.Cos(floorAngle * Mathf.Deg2Rad);
                float frictionForce = m_Data.FrictionCoefficient * objectDownForce;

                Vector3 lateralVelocity = Vector3.ProjectOnPlane(m_RB.linearVelocity, hit.normal);
                Vector3 friction = -lateralVelocity.normalized * frictionForce;

                m_RB?.AddForceAtPosition(friction, m_Wheel.position, ForceMode.Force);
            }
        }
		else
		{
            m_Wheel.position = transform.position + (-transform.up.normalized * m_SpringRestingDistance) + (transform.up.normalized * (m_Data.WheelDiameter / 2f));
        }
		
		if (m_Grounded != hit.collider)
		{
			m_Grounded = hit.collider;
			OnGroundedChanged?.Invoke(m_Grounded);
		}

        //to stop the tank from sliding you also need to conssider how much velocity is in the left/right direction and counter it here
    }
}
