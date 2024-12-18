using UnityEngine;

public class TankGameManager : GameManagerBase
{
    private AM_02Tank m_ActionMap;

    [SerializeField] private TankSO m_TankData; // USE RESOURCE FOLDER

    [SerializeField] private TankController tankController;
    [SerializeField] private CameraRig cameraController;

    protected override void Initialise()
    {
        base.Initialise();

        m_ActionMap = new AM_02Tank();

        tankController.Init(m_ActionMap, m_TankData, cameraController);
        tankController.Possess(Instantiate(Resources.Load<Tank>("Tank"), TankSpawner.GetPrimarySpawner().GetPosition(), Quaternion.identity));

        // cameraController.Control(tankController.GetPossessed());
    }
}
