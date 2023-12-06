// MyAvatarSys
// 朱梓瑞 Shepherd0619
// 换装系统
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

/// <summary>
/// 按照性别分的运行时换装数据库
/// </summary>
public class AvatarRes
{
	public List<IResourceLocation> mHairList = new List<IResourceLocation>();
	public List<IResourceLocation> mTopList = new List<IResourceLocation>();
	public List<IResourceLocation> mBtmList = new List<IResourceLocation>();
	public List<IResourceLocation> mShoesList = new List<IResourceLocation>();
	public List<IResourceLocation> mFaceList = new List<IResourceLocation>();
	public List<IResourceLocation> mEyeList = new List<IResourceLocation>();

	public int mHairIdx = 0;
	public int mBtmIdx = 0;
	public int mShoesIdx = 0;
	public int mTopIdx = 0;
	public int mFaceIdx = 0;
	public int mEyeIdx = 0;

	public void Reset()
	{
		mHairIdx = 0;
		mBtmIdx = 0;
		mShoesIdx = 0;
		mTopIdx = 0;
		mFaceIdx = 0;
		mEyeIdx = 0;
	}

	/// <summary>
	/// 释放当前衣服，适用于需要隐藏人物的情况。
	/// </summary>
	public void ReleaseCurrentClothes()
	{
		MyAvatarAssetLoader.ReleaseAsset(mHairList[mHairIdx].PrimaryKey);
		MyAvatarAssetLoader.ReleaseAsset(mBtmList[mBtmIdx].PrimaryKey);
		MyAvatarAssetLoader.ReleaseAsset(mShoesList[mShoesIdx].PrimaryKey);
		MyAvatarAssetLoader.ReleaseAsset(mTopList[mTopIdx].PrimaryKey);
		MyAvatarAssetLoader.ReleaseAsset(mFaceList[mFaceIdx].PrimaryKey);
		MyAvatarAssetLoader.ReleaseAsset(mEyeList[mEyeIdx].PrimaryKey);
	}

	public void AddIndex(int type)
	{
		switch (type)
		{
			case (int)EPart.EP_Hair:
				MyAvatarAssetLoader.ReleaseAsset(mHairList[mHairIdx].PrimaryKey);
				mHairIdx = (mHairIdx + 1) % mHairList.Count;
				break;
			case (int)EPart.EP_Btm:
				MyAvatarAssetLoader.ReleaseAsset(mBtmList[mBtmIdx].PrimaryKey);
				mBtmIdx = (mBtmIdx + 1) % mBtmList.Count;
				break;
			case (int)EPart.EP_Shoes:
				MyAvatarAssetLoader.ReleaseAsset(mShoesList[mShoesIdx].PrimaryKey);
				mShoesIdx = (mShoesIdx + 1) % mShoesList.Count;
				break;
			case (int)EPart.EP_Top:
				MyAvatarAssetLoader.ReleaseAsset(mTopList[mTopIdx].PrimaryKey);
				mTopIdx = (mTopIdx + 1) % mTopList.Count;
				break;
			case (int)EPart.EP_Face:
				MyAvatarAssetLoader.ReleaseAsset(mFaceList[mFaceIdx].PrimaryKey);
				mFaceIdx = (mFaceIdx + 1) % mFaceList.Count;
				break;
			case (int)EPart.EP_Eye:
				MyAvatarAssetLoader.ReleaseAsset(mEyeList[mEyeIdx].PrimaryKey);
				mEyeIdx = (mEyeIdx + 1) % mEyeList.Count;
				break;
		}
	}

	public void ReduceIndex(int type)
	{
		switch (type)
		{
			case (int)EPart.EP_Hair:
				mHairIdx = (mHairIdx - 1 + mHairList.Count) % mHairList.Count;
				break;
			case (int)EPart.EP_Btm:
				mBtmIdx = (mBtmIdx - 1 + mBtmList.Count) % mBtmList.Count;
				break;
			case (int)EPart.EP_Shoes:
				mShoesIdx = (mShoesIdx - 1 + mShoesList.Count) % mShoesList.Count;
				break;
			case (int)EPart.EP_Top:
				mTopIdx = (mTopIdx - 1 + mTopList.Count) % mTopList.Count;
				break;
			case (int)EPart.EP_Face:
				mFaceIdx = (mFaceIdx - 1 + mFaceList.Count) % mFaceList.Count;
				break;
			case (int)EPart.EP_Eye:
				mEyeIdx = (mEyeIdx - 1 + mEyeList.Count) % mEyeList.Count;
				break;
		}
	}


	public enum EPart
	{
		EP_Hair,
		EP_Top,
		EP_Btm,
		EP_Shoes,
		EP_Face,
		EP_Eye
	}
}

public class MyAvatarSys : MonoBehaviour
{
	public AvatarRes MaleAvatarRes => m_MaleAvatarRes;
	public AvatarRes FemaleAvatarRes => m_FemaleAvatarRes;
	private AvatarRes m_MaleAvatarRes = null;
	private AvatarRes m_FemaleAvatarRes = null;

	#region 初始化
	/// <summary>
	/// 初始化AvatarRes
	/// </summary>
	public void CreateAvatarRes()
	{
		// 从Addressables中读出Group内容的清单
		Addressables.LoadResourceLocationsAsync("AvatarSys/Male", typeof(GameObject))
			.Completed += OnLoadMaleResourceLocationsCompleted;
		// 从Addressables中读出Group内容的清单
		Addressables.LoadResourceLocationsAsync("AvatarSys/Female", typeof(GameObject))
			.Completed += OnLoadFemaleResourceLocationsCompleted;
	}

	/// <summary>
	/// 从Addressables中读出Group内容的清单的回调
	/// </summary>
	/// <param name="handle"></param>
	private void OnLoadMaleResourceLocationsCompleted(AsyncOperationHandle<IList<IResourceLocation>> handle)
	{
		if (handle.Status == AsyncOperationStatus.Succeeded)
		{
			// 干掉重复项
			IList<IResourceLocation> locations = handle.Result;
			IList<IResourceLocation> distinctLocations = new List<IResourceLocation>();

			HashSet<object> primaryKeySet = new HashSet<object>();

			foreach (IResourceLocation location in locations)
			{
				if (!primaryKeySet.Contains(location.PrimaryKey))
				{
					distinctLocations.Add(location);
					primaryKeySet.Add(location.PrimaryKey);
				}
			}

			locations = distinctLocations;
			distinctLocations = null;

			m_MaleAvatarRes = new AvatarRes();
			foreach (var location in locations)
			{
				Debug.Log($"[MyAvatarSys] Now loading {location.PrimaryKey}");
				// Please modify the following code to suit your needs!
				// In normal circumstances, clothes item should have a standard of naming.
				string[] splitStrings = Path.GetFileNameWithoutExtension(location.PrimaryKey).Split("_");
				switch (splitStrings[1].Split("-")[0])
				{
					case "face":
						{
							m_MaleAvatarRes.mFaceList.Add(location);
							break;
						}

					case "hair":
						{
							m_MaleAvatarRes.mHairList.Add(location);
							break;
						}

					case "pants":
						{
							m_MaleAvatarRes.mBtmList.Add(location);
							break;
						}

					case "shoes":
						{
							m_MaleAvatarRes.mShoesList.Add(location);
							break;
						}

					case "top":
						{
							m_MaleAvatarRes.mTopList.Add(location);
							break;
						}

					case "eyes":
						{
							m_MaleAvatarRes.mEyeList.Add(location);
							break;
						}
				}
			}
		}
		else
		{
			Debug.LogError("[MyAvatarSys] Failed to load resource locations: " + handle.OperationException);
		}
	}

	/// <summary>
	/// 从Addressables中读出Group内容的清单的回调
	/// </summary>
	/// <param name="handle"></param>
	private void OnLoadFemaleResourceLocationsCompleted(AsyncOperationHandle<IList<IResourceLocation>> handle)
	{
		if (handle.Status == AsyncOperationStatus.Succeeded)
		{
			// 干掉重复项
			IList<IResourceLocation> locations = handle.Result;
			IList<IResourceLocation> distinctLocations = new List<IResourceLocation>();

			HashSet<object> primaryKeySet = new HashSet<object>();

			foreach (IResourceLocation location in locations)
			{
				if (!primaryKeySet.Contains(location.PrimaryKey))
				{
					distinctLocations.Add(location);
					primaryKeySet.Add(location.PrimaryKey);
				}
			}

			locations = distinctLocations;
			distinctLocations = null;

			m_FemaleAvatarRes = new AvatarRes();
			foreach (var location in locations)
			{
				Debug.Log($"[MyAvatarSys] Now loading resource locations: {location.PrimaryKey}");
				// Please modify the following code to suit your needs!
				// In normal circumstances, clothes item should have a standard of naming.
				string[] splitStrings = Path.GetFileNameWithoutExtension(location.PrimaryKey).Split("_");
				switch (splitStrings[1].Split("-")[0])
				{
					case "face":
						{
							m_FemaleAvatarRes.mFaceList.Add(location);
							break;
						}

					case "hair":
						{
							m_FemaleAvatarRes.mHairList.Add(location);
							break;
						}

					case "pants":
						{
							m_FemaleAvatarRes.mBtmList.Add(location);
							break;
						}

					case "shoes":
						{
							m_FemaleAvatarRes.mShoesList.Add(location);
							break;
						}

					case "top":
						{
							m_FemaleAvatarRes.mTopList.Add(location);
							break;
						}

					case "eyes":
						{
							m_FemaleAvatarRes.mEyeList.Add(location);
							break;
						}
				}
			}
		}
		else
		{
			Debug.LogError("[MyAvatarSys] Failed to load resource locations: " + handle.OperationException);
		}
	}
	#endregion
}
