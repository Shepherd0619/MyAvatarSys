// MyAvatarSys
// 朱梓瑞 Shepherd0619
// 换装系统
using System.Collections;
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

    public int mHairIdx = 0;
    public int mBtmIdx = 0;
    public int mShoesIdx = 0;
    public int mTopIdx = 0;

    public void Reset()
    {
		mHairIdx = 0;
        mBtmIdx = 0;
        mShoesIdx = 0;
        mTopIdx = 0;
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
	}

    public void AddIndex(int type)
    {
        if (type == (int)EPart.EP_Hair)
        {
			MyAvatarAssetLoader.ReleaseAsset(mHairList[mHairIdx].PrimaryKey);
            mHairIdx++;
            if (mHairIdx >= mHairList.Count)
                mHairIdx = 0;
        }
        else if (type == (int)EPart.EP_Btm)
        {
	        MyAvatarAssetLoader.ReleaseAsset(mBtmList[mBtmIdx].PrimaryKey);
			mBtmIdx++;
            if (mBtmIdx >= mBtmList.Count)
                mBtmIdx = 0;
        }
        else if (type == (int)EPart.EP_Shoes)
        {
	        MyAvatarAssetLoader.ReleaseAsset(mShoesList[mShoesIdx].PrimaryKey);
			mShoesIdx++;
            if (mShoesIdx >= mShoesList.Count)
                mShoesIdx = 0;
        }
        else if (type == (int)EPart.EP_Top)
        {
	        MyAvatarAssetLoader.ReleaseAsset(mTopList[mTopIdx].PrimaryKey);
			mTopIdx++;
            if (mTopIdx >= mTopList.Count)
                mTopIdx = 0;
        }
    }

    public void ReduceIndex(int type)
    {
        if (type == (int)EPart.EP_Hair)
        {
            mHairIdx--;
            if (mHairIdx < 0)
                mHairIdx = mHairList.Count - 1;
        }
        else if (type == (int)EPart.EP_Btm)
        {
            mBtmIdx--;
            if (mBtmIdx < 0)
                mBtmIdx = mBtmList.Count - 1;
        }
        else if (type == (int)EPart.EP_Shoes)
        {
            mShoesIdx--;
            if (mShoesIdx < 0)
                mShoesIdx = mShoesList.Count - 1;
        }
        else if (type == (int)EPart.EP_Top)
        {
            mTopIdx--;
            if (mTopIdx < 0)
                mTopIdx = mTopList.Count - 1;
        }
    }

    public enum EPart
    {
        EP_Hair,
        EP_Top,
        EP_Btm,
        EP_Shoes,
    }
}

public class MyAvatarSys : MonoBehaviour
{
    /// <summary>
    /// 骨架名字
    /// </summary>
    private const string HairName = "Hair";
    private const string BtmName = "Btm";
    private const string ShoesName = "Sh";
    private const string TopName = "Top";

    private const string MaleName = "Boy";
    private const string FemaleName = "Girl";

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
        Addressables.LoadResourceLocationsAsync("AvatarSys")
            .Completed += OnLoadResourceLocationsCompleted;
    }

    /// <summary>
    /// 从Addressables中读出Group内容的清单的回调
    /// </summary>
    /// <param name="handle"></param>
    private void OnLoadResourceLocationsCompleted(AsyncOperationHandle<IList<IResourceLocation>> handle)
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
            m_FemaleAvatarRes = new AvatarRes();
            foreach (var location in locations)
            {
                // 拆字符串，用switch进行分类处理
                string[] split = location.PrimaryKey.Split("_");
                switch (split[1])
                {
                    case FemaleName:
                    {
                        switch (split[2])
                        {
                            case BtmName:
                            {
                                m_FemaleAvatarRes.mBtmList.Add(location);
                                break;
                            }

                            case HairName:
                            {
                                m_FemaleAvatarRes.mHairList.Add(location);
                                break;
                            }

                            case ShoesName:
                            {
                                m_FemaleAvatarRes.mShoesList.Add(location);
                                break;
                            }

                            case TopName:
                            {
                                m_FemaleAvatarRes.mTopList.Add(location);
                                break;
                            }
                        }

                        break;
                    }

                    case MaleName:
                    {
                        switch (split[2])
                        {
                            case BtmName:
                            {
                                m_MaleAvatarRes.mBtmList.Add(location);
                                break;
                            }

                            case HairName:
                            {
                                m_MaleAvatarRes.mHairList.Add(location);
                                break;
                            }

                            case ShoesName:
                            {
                                m_MaleAvatarRes.mShoesList.Add(location);
                                break;
                            }

                            case TopName:
                            {
                                m_MaleAvatarRes.mTopList.Add(location);
                                break;
                            }
                        }

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
