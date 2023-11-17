// MyAvatarUI
// 朱梓瑞 Shepherd0619
// 用于开发期间测试换装
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

		if (GUILayout.Button("♂", GUILayout.Width(buttonWidth), GUILayout.Height(typeheight)))
		{
			if (mAvatarRes != null)
				mAvatarRes.ReleaseCurrentClothes();
			mAvatarRes = mAvatarSys.MaleAvatarRes;
			mMaleCharacter.Generate(mAvatarRes, mCombine);
			mCharacter = mMaleCharacter;
			mFemaleCharacter.gameObject.SetActive(false);
			Destroy(mFemaleCharacter.Top);
			Destroy(mFemaleCharacter.Btm);
			Destroy(mFemaleCharacter.Shoes);
			Destroy(mFemaleCharacter.Hair);
			Destroy(mFemaleCharacter.Face);
			Destroy(mFemaleCharacter.Eye);
			mMaleCharacter.gameObject.SetActive(true);
		}

		GUILayout.Box("Character", GUILayout.Width(typeWidth), GUILayout.Height(typeheight));

		if (GUILayout.Button("♀", GUILayout.Width(buttonWidth), GUILayout.Height(typeheight)))
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
			Destroy(mFemaleCharacter.Face);
			Destroy(mFemaleCharacter.Eye);
			mFemaleCharacter.gameObject.SetActive(true);
		}

		GUILayout.EndHorizontal();

		// Buttons for changing character elements.
		AddCategory((int)EPart.EP_Hair, "Hair");
		AddCategory((int)EPart.EP_Face, "Face");
		AddCategory((int)EPart.EP_Eye, "Eye");
		AddCategory((int)EPart.EP_Top, "Body");
		AddCategory((int)EPart.EP_Btm, "Btm");
		AddCategory((int)EPart.EP_Shoes, "Feet");

		GUILayout.EndArea();
	}

	#endregion

	#region 函数

	private void AddCategory(int parttype, string displayName)
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
