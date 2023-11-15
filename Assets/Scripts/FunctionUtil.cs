// FunctionUtil
// 朱梓瑞 Shepherd0619
// 衣服GameObject配置简化
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

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
