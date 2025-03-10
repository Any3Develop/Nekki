using Nekki.Common.Audio;
using Nekki.Common.LifecycleService;
using Nekki.Common.UIService;
using Nekki.Common.UIService.Abstractions;

namespace Nekki.Infrastructure.Common.UIService
{
	public class SetupDefaultUIGroup : IInitable
	{
		private readonly IUIService uiService;
		private readonly IUIAudioListener audioListener;

		public SetupDefaultUIGroup(IUIService uiService, IUIAudioListener audioListener)
		{
			this.uiService = uiService;
			this.audioListener = audioListener;
		}

		public void Initialize()
		{
			audioListener.Subscribe(uiService.CreateAll(UILayer.DefaultUIGroup));
		}
	}
}