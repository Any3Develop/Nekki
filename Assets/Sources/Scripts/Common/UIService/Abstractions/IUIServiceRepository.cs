using System.Collections.Generic;
using Nekki.Common.UIService.Data;

namespace Nekki.Common.UIService.Abstractions
{
	public interface IUIServiceRepository
	{
		WindowItem Get<T>(string windowId, string groupId);
		
		IEnumerable<WindowItem> GetAll<T>(string groupId);
		
		void Add(WindowItem value);
		
		bool Remove(WindowItem value);
	}
}