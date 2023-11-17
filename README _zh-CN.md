# MyAvatarSys

作者：朱梓瑞 (Shepherd0619)

基于 [UnityDemo_Avatar](https://github.com/xieliujian/UnityDemo_Avatar).

## 描述
MyAvatarSys 是一个按性别划分的运行时换装数据库，同时也支持Addressables等系统。

## AvatarRes 类
AvatarRes 是一个根据性别存储运行时换装数据库的类。

### 属性
- `mHairList`：头发资源的列表。
- `mTopList`：上衣资源的列表。
- `mBtmList`：裤子资源的列表。
- `mShoesList`：鞋子资源的列表。
- `mFaceList`：脸部资源的列表。
- `mEyeList`：眼睛资源的列表。
- `mHairIdx`：当前头发资源的索引。
- `mBtmIdx`：当前裤子资源的索引。
- `mShoesIdx`：当前鞋子资源的索引。
- `mTopIdx`：当前上衣资源的索引。
- `mFaceIdx`：当前脸部资源的索引。
- `mEyeIdx`：当前眼睛资源的索引。

### 方法
- `Reset()`：将服装索引重置为初始状态。
- `ReleaseCurrentClothes()`：释放当前的服装资源。
- `AddIndex(int type)`：增加指定类型的服装索引。
- `ReduceIndex(int type)`：减少指定类型的服装索引。

## MyAvatarSys 类
MyAvatarSys 是一个管理 AvatarRes 对象并进行初始化的类。

### 属性
- `MaleAvatarRes`：男性角色的 AvatarRes 对象。
- `FemaleAvatarRes`：女性角色的 AvatarRes 对象。

### 方法
- `CreateAvatarRes()`：通过从 Addressables 中加载资源位置来初始化 AvatarRes 对象。

## MyAvatarCharacter 类
MyAvatarCharacter 是一个将 AvatarRes 应用到具体角色模型上的类。

### 属性
- `Hair`：头发游戏对象。
- `Btm`：裤子游戏对象。
- `Shoes`：鞋子游戏对象。
- `Top`：上衣游戏对象。
- `Face`：脸部游戏对象。
- `Eye`：眼睛游戏对象。

### 方法
- `Generate(AvatarRes avatarres, bool combine)`：使用指定的 AvatarRes 和合并选项生成角色模型。
- `ChangeEquipUnCombine(int type, AvatarRes avatarres)`：更换角色模型的装备，不合并网格。
- `ChangeEquipUnCombine(ref GameObject go, GameObject resgo)`：更换角色模型的装备，不合并网格，针对特定游戏对象。
- `ShareSkeletonInstanceWith(SkinnedMeshRenderer selfSkin, GameObject target)`：与目标游戏对象共享骨骼实例。
- `FindChildRecursion(Transform t, string name)`：递归查找具有指定名称的子变换。

## MyAvatarAssetLoader 类
MyAvatarAssetLoader 是一个精细管理换装系统所使用的 Addressables 句柄的类。

### 方法
- `LoadAssetAsync(string primaryKey, System.Action<GameObject> onComplete)`：异步加载具有指定主键的资源。
- `ReleaseAsset(string primaryKey)`：释放具有指定主键的资源。
- `ReleaseAllAssets()`：释放所有已加载的资源。

## MyAvatarUI 类
MyAvatarUI 是一个用于开发期间测试换装的类。

### 属性
- `mAvatarSys`：MyAvatarSys 对象。
- `mAvatarRes`：当前的 AvatarRes 对象。
- `mMaleCharacter`：男性 MyAvatarCharacter 对象。
- `mFemaleCharacter`：女性 MyAvatarCharacter 对象。
- `mCharacter`：当前的 MyAvatarCharacter 对象。
- `mCombine`：合并网格的标志。

### 方法
- `AddCategory(int parttype, string displayName)`：添加用于更换角色元素的类别。