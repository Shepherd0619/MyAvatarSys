// MyAvatarAssetLoader
// 朱梓瑞 Shepherd0619
// 精细管理换装系统所引用的Addressables句柄
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class MyAvatarAssetLoader
{
	private static Dictionary<string, AsyncOperationHandle<GameObject>> loadedAssets = new Dictionary<string, AsyncOperationHandle<GameObject>>();
	private static Dictionary<string, int> referenceCount = new Dictionary<string, int>();

	public static void LoadAssetAsync(string primaryKey, System.Action<GameObject> onComplete)
	{
		if (loadedAssets.ContainsKey(primaryKey))
		{
			referenceCount[primaryKey]++;
			Debug.Log($"[MyAvatarAssetLoader] Asset {primaryKey} is already loaded. Incrementing reference count to {referenceCount[primaryKey]}");
			onComplete?.Invoke(loadedAssets[primaryKey].Result);
		}
		else
		{
			AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(primaryKey);
			handle.Completed += (op) =>
			{
				if (op.Status == AsyncOperationStatus.Succeeded)
				{
					loadedAssets.Add(primaryKey, handle);
					referenceCount.Add(primaryKey, 1);
					Debug.Log($"[MyAvatarAssetLoader] Asset {primaryKey} loaded. Reference count: {referenceCount[primaryKey]}");
					onComplete?.Invoke(op.Result);
				}
				else
				{
					Debug.LogError($"[MyAvatarAssetLoader] Failed to load asset {primaryKey}: {op.OperationException}");
				}
			};
		}
	}

	public static void ReleaseAsset(string primaryKey)
	{
		if (loadedAssets.ContainsKey(primaryKey))
		{
			referenceCount[primaryKey]--;
			Debug.Log($"[MyAvatarAssetLoader] Releasing asset {primaryKey}. Reference count: {referenceCount[primaryKey]}");
			if (referenceCount[primaryKey] <= 0)
			{
				Addressables.Release(loadedAssets[primaryKey]);
				loadedAssets.Remove(primaryKey);
				referenceCount.Remove(primaryKey);
				Debug.Log($"[MyAvatarAssetLoader] Asset {primaryKey} released.");
			}
		}
		else
		{
			Debug.LogWarning($"[MyAvatarAssetLoader] Asset {primaryKey} is not loaded.");
		}
	}

	public static void ReleaseAllAssets()
	{
		foreach (var handle in loadedAssets.Values)
		{
			Addressables.Release(handle);
		}
		loadedAssets.Clear();
		referenceCount.Clear();
		Debug.Log("[MyAvatarAssetLoader] All assets released.");
	}
}
