// MyAvatarAssetLoader
// 朱梓瑞 Shepherd0619
// 精细管理换装系统所引用的Addressables句柄
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class MyAvatarAssetLoader
{
	private static Dictionary<string, AsyncOperationHandle<GameObject>> loadedAssets = new Dictionary<string, AsyncOperationHandle<GameObject>>();
	private static Dictionary<string, AsyncOperationHandle<Material>> loadedMaterials = new Dictionary<string, AsyncOperationHandle<Material>>();
	private static Dictionary<string, int> referenceCount = new Dictionary<string, int>();

	private static Dictionary<string, AsyncOperationHandle<GameObject>> m_LoadingGameObjecteHandles = new Dictionary<string, AsyncOperationHandle<GameObject>>();
	private static Dictionary<string, AsyncOperationHandle<Material>> m_LoadingMaterialHandles = new Dictionary<string, AsyncOperationHandle<Material>>();

	/// <summary>
	/// 异步读取GameObject
	/// </summary>
	/// <param name="primaryKey"></param>
	/// <param name="onComplete"></param>
	public static void LoadAssetAsync(string primaryKey, System.Action<GameObject> onComplete)
	{
		// 是否已经加载完成
		if (loadedAssets.TryGetValue(primaryKey, out var asset))
		{
			referenceCount[primaryKey]++;
			Debug.Log($"[MyAvatarAssetLoader] Asset {primaryKey} is already loaded. Incrementing reference count to {referenceCount[primaryKey]}");
			onComplete?.Invoke(asset.Result);
			return;
		}

		// 是否正在加载
		if (m_LoadingGameObjecteHandles.TryGetValue(primaryKey, out var loadingHandle))
		{
			Debug.Log($"[MyAvatarAssetLoader] Asset {primaryKey} is loading.");
			loadingHandle.Completed += handle => onComplete?.Invoke(handle.Result);
			return;
		}

		// 执行加载
		AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(primaryKey);
		m_LoadingGameObjecteHandles.Add(primaryKey, handle);
		handle.Completed += (op) =>
		{
			m_LoadingGameObjecteHandles.Remove(primaryKey);
			if (op.Status == AsyncOperationStatus.Succeeded)
			{
				loadedAssets.Add(primaryKey, handle);
				referenceCount.Add(primaryKey, 1);
				Debug.Log($"[MyAvatarAssetLoader] Asset {primaryKey} loaded. Reference count: {referenceCount[primaryKey]}");
				onComplete?.Invoke(op.Result);
			}
			else Debug.LogError($"[MyAvatarAssetLoader] Failed to load asset {primaryKey}: {op.OperationException}");
		};
	}

	/// <summary>
	/// 异步读取Material
	/// </summary>
	/// <param name="primaryKey"></param>
	/// <param name="onComplete"></param>
	public static void LoadAssetAsync(string primaryKey, System.Action<Material> onComplete)
	{
		// 是否已经加载完成
		if (loadedMaterials.TryGetValue(primaryKey, out var materialsHandle))
		{
			referenceCount[primaryKey]++;
			Debug.Log($"[MyAvatarAssetLoader] Asset {primaryKey} is already loaded. Incrementing reference count to {referenceCount[primaryKey]}");
			onComplete?.Invoke(materialsHandle.Result);
			return;
		}


		// 是否正在加载
		if (m_LoadingMaterialHandles.TryGetValue(primaryKey, out var loadingHandle))
		{
			Debug.Log($"[MyAvatarAssetLoader] Asset {primaryKey} is loading.");
			loadingHandle.Completed += handle => onComplete?.Invoke(handle.Result);
			return;
		}

		AsyncOperationHandle<Material> handle = Addressables.LoadAssetAsync<Material>(primaryKey);
		m_LoadingMaterialHandles.Add(primaryKey, handle);
		handle.Completed += (op) =>
		{
			m_LoadingMaterialHandles.Remove(primaryKey);
			if (op.Status == AsyncOperationStatus.Succeeded)
			{
				loadedMaterials.Add(primaryKey, handle);
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
			if (loadedMaterials.ContainsKey(primaryKey))
			{
				referenceCount[primaryKey]--;
				Debug.Log($"[MyAvatarAssetLoader] Releasing asset {primaryKey}. Reference count: {referenceCount[primaryKey]}");
				if (referenceCount[primaryKey] <= 0)
				{
					Addressables.Release(loadedMaterials[primaryKey]);
					loadedMaterials.Remove(primaryKey);
					referenceCount.Remove(primaryKey);
					Debug.Log($"[MyAvatarAssetLoader] Asset {primaryKey} released.");
				}
			}
			else
			{
				Debug.LogWarning($"[MyAvatarAssetLoader] Asset {primaryKey} is not loaded.");
			}
		}
	}

	public static void ReleaseAllAssets()
	{
		foreach (var handle in loadedAssets.Values)
		{
			Addressables.Release(handle);
		}
		loadedAssets.Clear();
		loadedMaterials.Clear();
		referenceCount.Clear();
		Debug.Log("[MyAvatarAssetLoader] All assets released.");
	}
}
