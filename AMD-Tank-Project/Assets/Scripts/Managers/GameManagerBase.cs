using System;
using UnityEngine;

public abstract class GameManagerBase : MonoBehaviour
{
    public static Action OnInitialiseScene;

    protected virtual void Start() => Initialise();

    protected virtual void Initialise()
    {
        OnInitialiseScene?.Invoke();
    }
}
