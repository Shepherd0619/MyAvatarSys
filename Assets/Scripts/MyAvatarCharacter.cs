// MyAvatarCharacter
// 朱梓瑞 Shepherd0619
// 将AvatarRes落实到具体模型上，应将本脚本挂在模型上。
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static AvatarRes;

public class MyAvatarCharacter : MonoBehaviour
{
    public GameObject Hair => mHair;
	public GameObject Btm => mBtm;
	public GameObject Shoes => mShoes;
	public GameObject Top => mTop;
	public GameObject Face => mFace;
	public GameObject Eye => mEye;

    private GameObject mHair;
    private GameObject mBtm;
    private GameObject mShoes;
    private GameObject mTop;
    private GameObject mFace;
    private GameObject mEye;

    /// <summary>
    /// 是否组合Mesh
    /// </summary>
    private bool mCombine = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable()
    {

	}

    private void DestroyAll()
    {
		mHair = null;
        mBtm = null;
        mShoes = null;
        mTop = null;
		mFace = null;
		mEye = null;
    }

    public void Generate(AvatarRes avatarres, bool combine = false)
    {
        //TODO: 后续如果需要合并mesh，在这里头放就行，根据combine的布尔值判断
        GenerateUnCombine(avatarres);
    }

    /// <summary>
    /// 开始应用变装，不合并mesh
    /// </summary>
    /// <param name="avatarres"></param>
    private void GenerateUnCombine(AvatarRes avatarres)
    {
        DestroyAll();

        ChangeEquipUnCombine((int)EPart.EP_Hair, avatarres);
        ChangeEquipUnCombine((int)EPart.EP_Btm, avatarres);
        ChangeEquipUnCombine((int)EPart.EP_Shoes, avatarres);
        ChangeEquipUnCombine((int)EPart.EP_Top, avatarres);
		ChangeEquipUnCombine((int)EPart.EP_Face, avatarres);
		ChangeEquipUnCombine((int)EPart.EP_Eye, avatarres);
	}

	public void ChangeEquipUnCombine(int type, AvatarRes avatarres)
	{
		if (type == (int)EPart.EP_Hair)
		{
			MyAvatarAssetLoader.LoadAssetAsync(avatarres.mHairList[avatarres.mHairIdx].PrimaryKey, (obj) =>
			{
				ChangeEquipUnCombine(ref mHair, obj);
			});
		}
		else if (type == (int)EPart.EP_Btm)
		{
			MyAvatarAssetLoader.LoadAssetAsync(avatarres.mBtmList[avatarres.mBtmIdx].PrimaryKey, (obj) =>
			{
				ChangeEquipUnCombine(ref mBtm, obj);
			});
		}
		else if (type == (int)EPart.EP_Shoes)
		{
			MyAvatarAssetLoader.LoadAssetAsync(avatarres.mShoesList[avatarres.mShoesIdx].PrimaryKey, (obj) =>
			{
				ChangeEquipUnCombine(ref mShoes, obj);
			});
		}
		else if (type == (int)EPart.EP_Top)
		{
			MyAvatarAssetLoader.LoadAssetAsync(avatarres.mTopList[avatarres.mTopIdx].PrimaryKey, (obj) =>
			{
				ChangeEquipUnCombine(ref mTop, obj);
			});
		}else if (type == (int)EPart.EP_Face)
		{
			MyAvatarAssetLoader.LoadAssetAsync(avatarres.mFaceList[avatarres.mFaceIdx].PrimaryKey, (obj) =>
			{
				ChangeEquipUnCombine(ref mFace, obj);
			});
		}else if (type == (int)EPart.EP_Eye)
		{
			MyAvatarAssetLoader.LoadAssetAsync(avatarres.mEyeList[avatarres.mEyeIdx].PrimaryKey, (obj) =>
			{
				ChangeEquipUnCombine(ref mEye, obj);
			});
		}
	}


	private void ChangeEquipUnCombine(ref GameObject go, GameObject resgo)
    {
        if (go != null)
        {
            GameObject.DestroyImmediate(go);
        }

        go = GameObject.Instantiate(resgo);
        go.Reset(gameObject);
        go.name = resgo.name;

		/*
        SkinnedMeshRenderer render = go.GetComponentInChildren<SkinnedMeshRenderer>();
        ShareSkeletonInstanceWith(render, mSkeleton);
		*/

		SkinnedMeshRenderer[] renders = go.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer render in renders)
		{
			ShareSkeletonInstanceWith(render, gameObject);
		}
    }

    // 共享骨骼
    public void ShareSkeletonInstanceWith(SkinnedMeshRenderer selfSkin, GameObject target)
    {
        Transform[] newBones = new Transform[selfSkin.bones.Length];
        for (int i = 0; i < selfSkin.bones.GetLength(0); ++i)
        {
            GameObject bone = selfSkin.bones[i].gameObject;

            // 目标的SkinnedMeshRenderer.bones保存的只是目标mesh相关的骨骼,要获得目标全部骨骼,可以通过查找的方式.
            newBones[i] = FindChildRecursion(target.transform, bone.name);
        }

        selfSkin.bones = newBones;
    }

    // 递归查找
    public Transform FindChildRecursion(Transform t, string name)
    {
        foreach (Transform child in t)
        {
            if (child.name == name)
            {
                return child;
            }
            else
            {
                Transform ret = FindChildRecursion(child, name);
                if (ret != null)
                    return ret;
            }
        }

        return null;
    }
}
