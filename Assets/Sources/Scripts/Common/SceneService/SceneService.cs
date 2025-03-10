using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Nekki.Common.SceneService.Abstractions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nekki.Common.SceneService
{
	public class SceneService : ISceneService, IDisposable
	{
		public event Action<SceneId> OnSceneLoaded;

		public SceneId Current { get; private set; }

		public async UniTask LoadAsync(SceneId sceneId)
		{
			Debug.Log($"Previous scene : {Current}");
			Current = sceneId;
			Debug.Log($"Scene loading : {sceneId}");
			
			var operation = SceneManager.LoadSceneAsync((int)sceneId, LoadSceneMode.Single);
			while (!operation.isDone)
				await Task.Yield();
			
			OnSceneLoaded?.Invoke(sceneId);
		}
		
		public void Dispose()
		{
			OnSceneLoaded = null;
		}
	}
}