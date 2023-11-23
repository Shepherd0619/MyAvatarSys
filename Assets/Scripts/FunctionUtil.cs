// FunctionUtil
// 朱梓瑞 Shepherd0619
// 衣服GameObject配置简化

using UnityEngine;

public static class FunctionUtil
{
	public static void Reset(this GameObject go, GameObject parent)
	{
		go.transform.parent = parent.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;
	}
}
