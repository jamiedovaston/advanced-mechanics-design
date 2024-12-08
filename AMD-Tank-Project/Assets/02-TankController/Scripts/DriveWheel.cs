using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveWheel : MonoBehaviour
{
	public event Action<bool> OnGroundedChanged;

	[SerializeField] private Rigidbody m_RB;
	[SerializeField] private TankSO m_Data;
	[SerializeField] private Suspension[] m_SuspensionWheels;
	private int m_NumGroundedWheels;
	private bool m_Grounded;

	private float m_Acceleration;
	public void SetAcceleration(float amount)
	{
		Debug.Log($"Acceleration set to: {amount}");
		m_Acceleration = amount;
	}

	public void Init(TankSO inData, Rigidbody _RBRef)
	{
		m_Data = inData;
		m_RB ??= _RBRef;

		m_NumGroundedWheels = 0;

		foreach (Suspension wheel in m_SuspensionWheels)
		{
			wheel.Init(m_Data.SuspensionData, m_RB);
			wheel.OnGroundedChanged += Handle_WheelGroundedChanged;
		}
	}

	private void Handle_WheelGroundedChanged(bool newGrounded)
	{
		if (newGrounded)
		{
			m_NumGroundedWheels++;
		}
		else
		{
			m_NumGroundedWheels--;
		}
	}

    private void FixedUpdate()
    {
        /*
		// Ensure correct ground percentage calculation
		float groundPercent = (float)m_NumGroundedWheels / m_SuspensionWheels.Length;
		// Calculate forward force
		float forwardForce = m_Acceleration * m_Data.EngineData.HorsePower * groundPercent; 
		//Create force vector 
		Vector3 force = transform.forward * forwardForce; 
		// Apply force to rigidbody
		m_RB.AddForceAtPosition(force, transform.position, ForceMode.Acceleration);
        // Limit angular velocity to prevent uncontrollable spinning
		m_RB.angularVelocity = Vector3.ClampMagnitude(m_RB.angularVelocity, 10.0f);*/

        if (m_NumGroundedWheels == 0 || m_Acceleration == 0)
            return;

        Vector3 forcePosition = Vector3.zero;
        Vector3 force = Vector3.zero;

        float acceleration = m_Data.EngineData.HorsePower / (m_RB.mass / 1000);
        float traction = (float)m_NumGroundedWheels / (float)m_SuspensionWheels.Length;
		
        for (int i = 0; i < m_SuspensionWheels.Length; i++)
        {
            if (!m_SuspensionWheels[i].GetGrounded())
                continue;
            forcePosition += m_SuspensionWheels[i].transform.position;
        }

        forcePosition = forcePosition / m_NumGroundedWheels;
        force = ((m_RB.transform.forward * m_Acceleration) * acceleration) * traction;

        m_RB?.AddForceAtPosition(force, forcePosition, ForceMode.Acceleration);

        if (m_RB.linearVelocity.magnitude > 24.0f)
        {
            m_RB.linearVelocity = m_RB.linearVelocity.normalized * 24;
        }
    }
}