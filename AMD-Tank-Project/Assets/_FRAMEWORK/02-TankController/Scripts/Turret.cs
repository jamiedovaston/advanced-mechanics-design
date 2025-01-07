using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
	private TankSO m_Data;
	private CameraRig m_Rig;

	[SerializeField] private Transform m_Turret;
	[SerializeField] private Transform m_Barrel;

	private bool m_RotationDirty;
	private Coroutine m_CRAimingTurret;

	Vector3 m_ProjectedDirection;

	private void Awake()
	{
		m_RotationDirty = false;
	}

	public void Init(TankSO inData, CameraRig _rig)
	{
		m_Data = inData;
		m_Rig = _rig;
	}

	public void SetRotationDirty()
	{
		//if already dirty then return
		//else set the value and start the below coroutine

        if (m_CRAimingTurret == null)
		{
			m_CRAimingTurret = StartCoroutine(C_AimTurret());
		}

		// 30.0f.Remap360To180PN();
	}

	private IEnumerator C_AimTurret()
	{
		//Fix this to loop while the rotation is dirty, rotate towards the needed vector and unset dirty when facing
		//TIP: simplfy the problem into 2D with the Vector3.ProjectOnPlane function and diagram it out. The turret rotates around the local y and the barrel around the local x
		//Make use of the remap function as shown in Camera.cs to help
		//extention here could include some SUVAT formula work to adjust the aim to hit where the camera is pointing, accounting for gravity

		m_RotationDirty = true;

		while (m_RotationDirty)
        {
            m_ProjectedDirection = Vector3.ProjectOnPlane(m_Rig.GetForward(), m_Turret.up).normalized;

            Vector3 cross = Vector3.Cross(m_Turret.forward, m_ProjectedDirection);

			//m_Turret.Rotate(m_Turret.up, cross.normalized.y);
			m_Turret.localEulerAngles += ((Vector3.up * cross.y) * m_Data.TurretData.TurretTraverseSpeed) * Time.fixedDeltaTime;

			Debug.Log("Update!");

			yield return new WaitForFixedUpdate();

            Debug.DrawRay(m_Turret.position, m_ProjectedDirection, Color.red);
            Debug.DrawRay(m_Turret.position, m_Turret.forward, Color.blue);

            if (Vector3.Angle(m_Turret.forward, m_ProjectedDirection) < .66f)
            {
                m_RotationDirty = false;
            }
        }

        // Quaternion finalRotation = Quaternion.LookRotation(m_ProjectedDirection); 
		// m_Turret.localEulerAngles = new Vector3(m_Turret.localEulerAngles.x, finalRotation.eulerAngles.y, m_Turret.localEulerAngles.z);

		// m_Turret.localEulerAngles = Quaternion.LookRotation(m_ProjectedDirection).eulerAngles;

        m_CRAimingTurret = null;
    }
}