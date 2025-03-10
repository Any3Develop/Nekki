using System.Collections.Generic;
using Nekki.Common.UIService.Abstractions;

namespace Nekki.Common.Audio
{
	public interface IUIAudioListener
	{
		void Subscribe(IEnumerable<IUIWindow> windows);
	}
}