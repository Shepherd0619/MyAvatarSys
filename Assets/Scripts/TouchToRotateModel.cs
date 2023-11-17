// TouchToRotateModel
// 朱梓瑞 Shepherd0619
// 触摸/鼠标旋转模型
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchToRotateModel : MonoBehaviour
{
	public Transform target;

	private void Awake()
	{
		if (target == null)
			target = transform;
	}

	void Update()
	{
		// 检查是否有UI遮挡
		if (EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}

		// 触摸旋转
		if (Input.touchCount > 0)
		{
			//单点触摸， 水平上下旋转
			if (Input.touchCount == 1)
			{
				Touch touch = Input.GetTouch(0);
				Vector2 deltaPos = touch.deltaPosition;
				target.Rotate(Vector3.down * deltaPos.x, Space.World);//绕Y轴进行旋转
				//target.Rotate(Vector3.right * deltaPos.y, Space.World);//绕X轴进行旋转，下面我们还可以写绕Z轴进行旋转
			}
		}
		// 鼠标旋转
		else if (Input.GetMouseButton(0))
		{
			float mouseX = Input.GetAxis("Mouse X");
			target.Rotate(Vector3.down * mouseX * 5.0f, Space.World);//绕Y轴进行旋转
			//float mouseY = Input.GetAxis("Mouse Y");
			//target.Rotate(Vector3.right * mouseY, Space.World);//绕X轴进行旋转，下面我们还可以写绕Z轴进行旋转
		}
	}
}
