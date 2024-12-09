using System;
using UnityEngine;

public abstract class GameManagerBase : MonoBehaviour
{
    protected abstract void OnEnable();
    protected abstract void OnDisable();
    protected virtual void Start() => Initialise();

    protected virtual void Initialise()
    {

    }
}


public class TankGameManager : GameManagerBase
{
    private AM_02Tank m_ActionMap;
    [SerializeField] private TankSO m_TankData;

    [SerializeField] private TankController controller;

    protected override void OnDisable()
    {

    }

    protected override void OnEnable()
    {

    }

    protected override void Initialise()
    {
        m_ActionMap = new AM_02Tank();
        controller.Init(m_ActionMap, m_TankData);
    }
}