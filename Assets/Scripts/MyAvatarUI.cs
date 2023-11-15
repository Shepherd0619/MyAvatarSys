// MyAvatarUI
// 朱梓瑞 Shepherd0619
// 用于开发期间测试换装
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static AvatarRes;

public class MyAvatarUI : MonoBehaviour
{
	#region 常量

	private const int typeWidth = 240;
	private const int typeheight = 100;
	private const int buttonWidth = 60;

	#endregion

	#region 变量

	[SerializeField] private MyAvatarSys mAvatarSys;
	private AvatarRes mAvatarRes = null;
	[SerializeField] private MyAvatarCharacter mMaleCharacter = null;
	[SerializeField] private MyAvatarCharacter mFemaleCharacter = null;
	private MyAvatarCharacter mCharacter = null;
	private bool mCombine = false;

	#endregion

	#region 内置函数

	// Use this for initialization
	void Start()
	{
		mAvatarSys.CreateAvatarRes();

		mMaleCharacter.gameObject.SetActive(false);
		mFemaleCharacter.gameObject.SetActive(false);
	}

	private void OnGUI()
	{
		GUI.skin.box.fontSize = 50;
		GUI.skin.button.fontSize = 50;

		GUILayout.BeginArea(new Rect(10, 10, typeWidth + 2 * buttonWidth + 8, 1000));

		// Buttons for changing the active character.
		GUILayout.BeginHorizontal();

		if (GUILayout.Button("男", GUILayout.Width(buttonWidth), GUILayout.Height(typeheight)))
		{
			if(mAvatarRes != null)
				mAvatarRes.ReleaseCurrentClothes();
			mAvatarRes = mAvatarSys.MaleAvatarRes;
			mMaleCharacter.Generate(mAvatarRes, mCombine);
			mCharacter = mMaleCharacter;
			mFemaleCharacter.gameObject.SetActive(false);
			Destroy(mFemaleCharacter.Top);
			Destroy(mFemaleCharacter.Btm);
			Destroy(mFemaleCharacter.Shoes);
			Destroy(mFemaleCharacter.Hair);
			mMaleCharacter.gameObject.SetActive(true);
		}

		GUILayout.Box("Character", GUILayout.Width(typeWidth), GUILayout.Height(typeheight));

		if (GUILayout.Button("女", GUILayout.Width(buttonWidth), GUILayout.Height(typeheight)))
		{
			if (mAvatarRes != null)
				mAvatarRes.ReleaseCurrentClothes();
			mAvatarRes = mAvatarSys.FemaleAvatarRes;
			mFemaleCharacter.Generate(mAvatarRes, mCombine);
			mCharacter = mFemaleCharacter;
			mMaleCharacter.gameObject.SetActive(false);
			Destroy(mMaleCharacter.Top);
			Destroy(mMaleCharacter.Btm);
			Destroy(mMaleCharacter.Shoes);
			Destroy(mMaleCharacter.Hair);
			mFemaleCharacter.gameObject.SetActive(true);
		}

		GUILayout.EndHorizontal();

		// Buttons for changing character elements.
		AddCategory((int)EPart.EP_Hair, "Hair", null);
		AddCategory((int)EPart.EP_Top, "Body", "item_shirt");
		AddCategory((int)EPart.EP_Btm, "Btm", "item_pants");
		AddCategory((int)EPart.EP_Shoes, "Feet", "item_boots");

		GUILayout.EndArea();
	}

	#endregion

	#region 函数

	private void AddCategory(int parttype, string displayName, string anim)
	{
		GUILayout.BeginHorizontal();

		if (GUILayout.Button("<", GUILayout.Width(buttonWidth), GUILayout.Height(typeheight)))
		{
			mAvatarRes.ReduceIndex(parttype);

			if (!mCombine)
				mCharacter.ChangeEquipUnCombine(parttype, mAvatarRes);
			else
				mCharacter.Generate(mAvatarRes, mCombine);
		}

		GUILayout.Box(displayName, GUILayout.Width(typeWidth), GUILayout.Height(typeheight));

		if (GUILayout.Button(">", GUILayout.Width(buttonWidth), GUILayout.Height(typeheight)))
		{
			mAvatarRes.AddIndex(parttype);

			if (!mCombine)
				mCharacter.ChangeEquipUnCombine(parttype, mAvatarRes);
			else
				mCharacter.Generate(mAvatarRes, mCombine);
		}

		GUILayout.EndHorizontal();
	}

	#endregion
}
