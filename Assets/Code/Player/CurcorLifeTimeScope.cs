using VContainer;
using VContainer.Unity;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class CurcorLifeTimeScope : LifetimeScope
    {
        [SerializeField] private CurcorComponents components;

        protected override void Configure(IContainerBuilder builder)
        {
            
        }
    }

    [Serializable]
    public class CurcorComponents
    {
        [SerializeField] public InputActionAsset inputActions;
    }
}