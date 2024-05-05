using events;
using hand;
using UnityEngine;
using Zenject;

namespace injection
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Events>().AsSingle();
            Container.Bind<HandController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();
        }
    }
}