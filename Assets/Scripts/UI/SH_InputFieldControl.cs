using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_InputFieldControl : MonoBehaviour
{
	InputField field;

	// Use this for initialization
	void Start()
	{
		field = GetComponent<InputField>();
		field.onValidateInput += delegate (string text, int charIndex, char addedChar) {
			return changeUpperCase(addedChar);
		};

	}

	char changeUpperCase(char _cha)
	{
		char tmpChar = _cha;

		string tmpString = tmpChar.ToString();

		tmpString = tmpString.ToUpper();

		tmpChar = System.Convert.ToChar(tmpString);

		return tmpChar;
	}
}
