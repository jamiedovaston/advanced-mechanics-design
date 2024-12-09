using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
{
	private AM_02Tank m_ActionMap; //input
	private TankSO m_Data;
    private CameraController m_CameraController;

	private IPossessable m_Tank;

	//type object link
	//Component links
	private float m_InAccelerate;
	private float m_InSteer;
	private bool m_IsSteering;
	private Coroutine m_CRSteer;
	private bool m_IsFiring;

    public void Init(AM_02Tank _actionMap, TankSO _data)
	{
		m_ActionMap = _actionMap;
		m_Data = _data;

        //m_CameraController ??= GetComponent<CameraController>();
        //m_CameraController.Init();

        m_ActionMap.Enable();

        m_ActionMap.Default.Accelerate.performed += Handle_AcceleratePerformed;
        m_ActionMap.Default.Accelerate.canceled += Handle_AccelerateCanceled;
        m_ActionMap.Default.Steer.performed += Handle_SteerPerformed;
        m_ActionMap.Default.Steer.canceled += Handle_SteerCanceled;
        m_ActionMap.Default.Fire.performed += Handle_FirePerformed;
        m_ActionMap.Default.Fire.canceled += Handle_FireCanceled;
        m_ActionMap.Default.Aim.performed += Handle_AimPerformed;
        m_ActionMap.Default.Zoom.performed += Handle_ZoomPerformed;
    }

	public void Possess(IPossessable _possessable)
	{
		m_Tank = _possessable;
        _possessable.Init(m_Data);
	}

	public void UnPossess()
	{
        m_Tank = null;
	}

    private void OnDisable()
    {
        m_ActionMap.Disable();

        m_ActionMap.Default.Accelerate.performed -= Handle_AcceleratePerformed;
        m_ActionMap.Default.Accelerate.canceled -= Handle_AccelerateCanceled;
        m_ActionMap.Default.Steer.performed -= Handle_SteerPerformed;
        m_ActionMap.Default.Steer.canceled -= Handle_SteerCanceled;
        m_ActionMap.Default.Fire.performed -= Handle_FirePerformed;
        m_ActionMap.Default.Fire.canceled -= Handle_FireCanceled;
        m_ActionMap.Default.Aim.performed -= Handle_AimPerformed;
        m_ActionMap.Default.Zoom.performed -= Handle_ZoomPerformed;
    }

    private void Handle_AcceleratePerformed(InputAction.CallbackContext context)
	{
		m_Tank?.Accelerate(context.ReadValue<float>());
	}

	private void Handle_AccelerateCanceled(InputAction.CallbackContext context)
	{
        m_Tank?.Accelerate(context.ReadValue<float>());
    }

    private void Handle_SteerPerformed(InputAction.CallbackContext context)
	{
		m_Tank?.Steer(context.ReadValue<float>(), true);
	}

	private void Handle_SteerCanceled(InputAction.CallbackContext context)
	{
        m_Tank?.Steer(context.ReadValue<float>(), false);
    }

    private void Handle_FirePerformed(InputAction.CallbackContext context)
	{
		m_Tank?.Fire(true);
	}

	private void Handle_FireCanceled(InputAction.CallbackContext context)
	{
		m_Tank?.Fire(false);
    }

	private void Handle_AimPerformed(InputAction.CallbackContext context)
	{
        // FRAMEWORK IMPLEMENTATION
        // m_CameraController.RotateSpringArm(context.ReadValue<Vector2>());
        m_Tank?.Aim();
	}

	private void Handle_ZoomPerformed(InputAction.CallbackContext context)
	{
		// FRAMEWORK IMPLEMENTATION
		// m_CameraController.ChangeCameraDistance(context.ReadValue<float>());
		m_Tank?.Zoom();
	}
}
