using System.Collections;
using UnityEngine;

public class Tank : Entity, IPossessable
{
    private TankSO m_Data;
    private CameraRig m_Rig;

    private Rigidbody m_RB;
	private Turret m_TurretController;

    [Header("Property Display")]
	[SerializeField] private Barrel m_BarrelController;
	[SerializeField] private DriveWheel[] m_DriveWheels;

	private float m_InAccelerate;

	private float m_InSteer;
	private bool m_IsSteering;
	private Coroutine m_CRSteer;

	private bool m_IsFiring;

    private void Awake()
    {
        m_RB ??= GetComponent<Rigidbody>();
        m_TurretController ??= GetComponent<Turret>();
    }

    public void Init(TankSO _data, CameraRig _rig)
	{
        m_Data = _data;
        m_Rig = _rig;

        foreach (DriveWheel wheel in m_DriveWheels)
        {
            wheel.Init(_data, m_RB);
        }

        m_TurretController.Init(_data, _rig);
    }

    public void Accelerate(float _inAccelerate)
    {
        foreach (DriveWheel wheel in m_DriveWheels)
        {
            wheel.SetAcceleration(_inAccelerate);
        }
        m_TurretController.SetRotationDirty();
    }

    public void Steer(float _inSteer, bool _isSteering)
    {
        Debug.Log($"In Steer: {_inSteer}");
        m_InSteer = _inSteer;

        if (m_IsSteering == _isSteering) return;

        m_IsSteering = _isSteering;

        m_CRSteer = StartCoroutine(C_SteerUpdate());
    }

    public void Fire(bool _isFiring)
    {
		m_IsFiring = _isFiring;

		if(_isFiring) m_BarrelController.Fire();
		else
		{
            //this is here in the case you want to expand to support magazine style auto-loaders but thats large scope
        }
    }

    public void Aim()
    {
        m_TurretController.SetRotationDirty();
    }

    public void Zoom()
    {
        m_TurretController.SetRotationDirty();
    }

    private IEnumerator C_SteerUpdate()
    {
        while (m_IsSteering)
        {
            for (int i = 0; i < m_DriveWheels.Length; i++)
            {
                m_DriveWheels[i].SetSteer(m_InSteer);
            }

            m_TurretController.SetRotationDirty();

            yield return null;
        }

        for (int i = 0; i < m_DriveWheels.Length; i++)
        {
            m_DriveWheels[i].SetSteer(0.0f);
        }
    }

    public Transform GetTransform() => transform;
}