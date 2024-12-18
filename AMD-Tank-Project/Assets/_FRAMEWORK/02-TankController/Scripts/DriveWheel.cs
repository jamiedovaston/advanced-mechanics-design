using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
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

	//DEBUG
	public float m_SteerWeighting;
	private float m_Steer;

	public void SetAcceleration(float amount) =>
		m_Acceleration = amount;
    public void SetSteer(float amount) =>
        m_Steer = amount;

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
		if (m_NumGroundedWheels == 0 || (m_Acceleration == 0 && m_Steer == 0))
            return;

        Vector3 steerForce = Vector3.zero;
        Vector3 force = Vector3.zero;

		float tankForward = Vector3.Dot(m_RB.GetPointVelocity(transform.position), transform.forward);

        float acceleration = (m_Data.EngineData.HorsePower * 0.7456992f) / (m_Data.Mass_Tons * Mathf.Max(Mathf.Abs(tankForward), 1));
        float traction = (float)m_NumGroundedWheels / (float)m_SuspensionWheels.Length;

		force = ((transform.forward *  m_Acceleration) * acceleration) * traction;
		steerForce = (transform.forward * ((m_Steer * -m_SteerWeighting) + 1.0f));

        m_RB?.AddForceAtPosition(force + steerForce, transform.position, ForceMode.Acceleration);

        if (m_RB.linearVelocity.magnitude > 10.0f)
        {
            m_RB.linearVelocity = m_RB.linearVelocity.normalized * 10.0f;
        }
    }
}