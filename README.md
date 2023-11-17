# MyAvatarSys
English | [中文](https://github.com/Shepherd0619/MyAvatarSys/blob/master/README%20_zh-CN.md)

Author: Shepherd Zhu

Based on [UnityDemo_Avatar](https://github.com/xieliujian/UnityDemo_Avatar).

## Description
MyAvatarSys is a runtime costume change database that is divided by gender.
It also support Addressables.

## AvatarRes Class
AvatarRes is a class that stores the runtime costume database according to gender.

### Properties
- `mHairList`: A list of hair resources.
- `mTopList`: A list of top resources.
- `mBtmList`: A list of bottom resources.
- `mShoesList`: A list of shoe resources.
- `mFaceList`: A list of face resources.
- `mEyeList`: A list of eye resources.
- `mHairIdx`: The index of the current hair resource.
- `mBtmIdx`: The index of the current bottom resource.
- `mShoesIdx`: The index of the current shoe resource.
- `mTopIdx`: The index of the current top resource.
- `mFaceIdx`: The index of the current face resource.
- `mEyeIdx`: The index of the current eye resource.

### Methods
- `Reset()`: Resets the costume indexes to the initial state.
- `ReleaseCurrentClothes()`: Releases the current clothes assets.
- `AddIndex(int type)`: Increments the index of the specified costume type.
- `ReduceIndex(int type)`: Decrements the index of the specified costume type.

## MyAvatarSys Class
MyAvatarSys is a class that manages the AvatarRes objects and initializes them.

### Properties
- `MaleAvatarRes`: The AvatarRes object for male avatars.
- `FemaleAvatarRes`: The AvatarRes object for female avatars.

### Methods
- `CreateAvatarRes()`: Initializes the AvatarRes objects by loading resource locations from Addressables.

## MyAvatarCharacter Class
MyAvatarCharacter is a class that applies the AvatarRes to the specific character model.

### Properties
- `Hair`: The hair game object.
- `Btm`: The bottom game object.
- `Shoes`: The shoe game object.
- `Top`: The top game object.
- `Face`: The face game object.
- `Eye`: The eye game object.

### Methods
- `Generate(AvatarRes avatarres, bool combine)`: Generates the character model with the specified AvatarRes and combine option.
- `ChangeEquipUnCombine(int type, AvatarRes avatarres)`: Changes the equipment of the character model without combining meshes.
- `ChangeEquipUnCombine(ref GameObject go, GameObject resgo)`: Changes the equipment of the character model without combining meshes for a specific game object.
- `ShareSkeletonInstanceWith(SkinnedMeshRenderer selfSkin, GameObject target)`: Shares the skeleton instance with the target game object.
- `FindChildRecursion(Transform t, string name)`: Recursively finds a child transform with the specified name.

## MyAvatarAssetLoader Class
MyAvatarAssetLoader is a class that finely manages the Addressables handles used by the costume change system.

### Methods
- `LoadAssetAsync(string primaryKey, System.Action<GameObject> onComplete)`: Loads the asset with the specified primary key asynchronously.
- `ReleaseAsset(string primaryKey)`: Releases the asset with the specified primary key.
- `ReleaseAllAssets()`: Releases all loaded assets.

## MyAvatarUI Class
MyAvatarUI is a class used for testing costume changes during development.

### Properties
- `mAvatarSys`: The MyAvatarSys object.
- `mAvatarRes`: The current AvatarRes object.
- `mMaleCharacter`: The male MyAvatarCharacter object.
- `mFemaleCharacter`: The female MyAvatarCharacter object.
- `mCharacter`: The current MyAvatarCharacter object.
- `mCombine`: The flag for combining meshes.

### Methods
- `AddCategory(int parttype, string displayName)`: Adds a category for changing character elements.
