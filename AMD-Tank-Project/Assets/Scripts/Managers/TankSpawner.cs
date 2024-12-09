using UnityEngine;

public class TankSpawner : MonoBehaviour, ITankSpawnable
{
    public bool isPrimarySpawner;

    private void OnEnable() =>
        GameManagerBase.OnInitialiseScene += Initialise;

    private void OnDisable() =>
        GameManagerBase.OnInitialiseScene -= Initialise;

    private void Initialise()
    {
        if (isPrimarySpawner) PrimarySpawner = this;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private static ITankSpawnable PrimarySpawner;
    public static ITankSpawnable GetPrimarySpawner()
    {
        return PrimarySpawner;
    }
}
