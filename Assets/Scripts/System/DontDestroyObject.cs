using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
	private void Awake()
	{
		// ���� ���� �ִ� ���� ������Ʈ�� �� ������ �˻��� �� 2�� �̻��̶�� ������Ʈ �ı�
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