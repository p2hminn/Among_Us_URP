using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
	private void Awake()
	{
		// 현재 씬에 있는 같은 오브젝트가 몇 개인지 검사한 뒤 2개 이상이라면 오브젝트 파괴
		var objs = FindObjectsOfType<DontDestroyObject>();
		if (objs.Length == 1)
		{
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}