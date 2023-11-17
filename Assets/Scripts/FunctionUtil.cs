// FunctionUtil
// 朱梓瑞 Shepherd0619
// 衣服GameObject配置简化

/* 项目“Assembly-CSharp.Player”的未合并的更改
在此之前:
using System.Collections;
在此之后:
using System;
using System.Collections;
*/
using
/* 项目“Assembly-CSharp.Player”的未合并的更改
在此之前:
using UnityEngine;
using System;
using System.IO;
在此之后:
using System.IO;
using UnityEngine;
*/
UnityEngine;

public static class FunctionUtil
{
	public static void Reset(this GameObject go, GameObject parent)
	{
		go.transform.parent = parent.transform;
		go.transform.position = Vector3.zero;
		go.transform.rotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;
	}
}
