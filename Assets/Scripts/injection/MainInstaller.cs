using System.Collections.Generic;
using blocks;
using hand;
using UnityEngine;
using Zenject;

namespace injection
{
    public class MainInstaller : MonoInstaller
    {
        public List<TileTypeSO> AllTileTypes;


        public override void InstallBindings()
        {
            Container.Bind<HandController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();
            Container.BindInstance(AllTileTypes).AsSingle();
            Container.Bind<ShapeCreator>().AsSingle();
        }
    }
}