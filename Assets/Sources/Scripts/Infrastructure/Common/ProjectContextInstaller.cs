using Nekki.Common.CameraProvider;
using Nekki.Common.SceneService;
using Nekki.Game.Context;
using Nekki.Infrastructure.Common.DependencyInjection;
using Nekki.Infrastructure.Common.InputSystem;
using Nekki.Infrastructure.Common.LifecycleService;
using Nekki.Infrastructure.Common.UIService;
using UnityEngine;
using Zenject;

namespace Nekki.Infrastructure.Common
{
    public class ProjectContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallSettings();
            
            Container
                .BindInterfacesTo<ZenjectServiceProvider>()
                .AsSingle()
                .CopyIntoAllSubContainers();

            Container
                .BindInterfacesTo<ZenjectAbstractFactory>()
                .AsSingle()
                .CopyIntoAllSubContainers();
            
            Container
                .BindInterfacesTo<GlobalCameraProvider>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesTo<ZenjectLifecycleSystem>()
                .AsSingle()
                .CopyIntoAllSubContainers();
            
            Container
                .BindInterfacesTo<SceneService>()
                .AsSingle();
            
            InputSystemInstaller.Install(Container);
            UIServiceInstaller.Install(Container);
        }
        
        public static void InstallSettings()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
        }
    }
}