using Nekki.Common.Audio;
using Nekki.Common.UIService;
using Nekki.Common.UIService.AnimationSource.Factories;
using Nekki.Common.UIService.AudioSource.Factories;
using Nekki.Common.UIService.FullFade;
using Nekki.Common.UIService.Options;
using Zenject;

namespace Nekki.Infrastructure.Common.UIService
{
	public class UIServiceInstaller : Installer<UIServiceInstaller>
	{
		public override void InstallBindings()
		{
			Container
				.BindInterfacesTo<UIRoot>()
				.FromComponentInNewPrefabResource("UIService/UIRoot")
				.AsSingle()
				.NonLazy();

			Container
				.BindInterfacesTo<UIDefaultService>()
				.AsSingle()
				.WithArguments(UILayer.DefaultUIGroup);

			Container
				.BindInterfacesTo<UIWindowPrototypeProvider>()
				.AsSingle()
				.WithArguments("UIService");

			Container
				.BindInterfacesTo<UIWindowFactory>()
				.AsSingle();

			Container
				.BindInterfacesTo<UIAudioSourceFactory>()
				.AsSingle();

			Container
				.BindInterfacesTo<UIAudioHandlersFactory>()
				.AsSingle();

			Container
				.BindInterfacesTo<UIAnimationSourceFactory>()
				.AsSingle();

			Container
				.BindInterfacesTo<UIAnimationsFactory>()
				.AsSingle();

			Container
				.BindInterfacesTo<UIOptionsFactory>()
				.AsSingle();

			Container
				.BindInterfacesTo<UIServiceRepository>()
				.AsSingle();

			Container
				.BindInterfacesTo<UIFullFadePresenter>()
				.AsSingle();

			Container
				.BindInterfacesTo<UIAudioListener>()
				.AsSingle();

			Container
				.BindInterfacesTo<SetupDefaultUIGroup>()
				.AsSingle()
				.NonLazy();
		}
	}
}