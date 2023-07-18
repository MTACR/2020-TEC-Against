using UnityEngine;
using System.Collections.Generic;


//static class that stores the key dictionary. The dictionary is loaded at runtime from Keybinding scripts.
//The keybinding scripts will load from the inspector unless there is a corresponding key in player prefs.
public static class KeyBindingManager  {

	public static Dictionary<KeyAction, KeyCode> keyDict = new Dictionary<KeyAction, KeyCode>(){
			{ KeyAction.none,			KeyCode.None},
			{ KeyAction.up,				KeyCode.W},
			{ KeyAction.down,			KeyCode.S},
			{ KeyAction.left,			KeyCode.A},
			{ KeyAction.right,			KeyCode.D},
			{ KeyAction.jump,			KeyCode.Space},
			{ KeyAction.hack,			KeyCode.E},
			{ KeyAction.fire,			KeyCode.LeftShift},
			{ KeyAction.hackUp,			KeyCode.W},
			{ KeyAction.hackDown,		KeyCode.S},
			{ KeyAction.hackRight,		KeyCode.D},
			{ KeyAction.hackLeft,		KeyCode.A},
			{ KeyAction.hackFire,		KeyCode.LeftShift},
			{ KeyAction.hackAimUp,		KeyCode.UpArrow},
			{ KeyAction.hackAimDown,	KeyCode.DownArrow},
			{ KeyAction.hackAimLeft,	KeyCode.LeftArrow},
			{ KeyAction.hackAimRight,	KeyCode.RightArrow},
			};

	//Returns key code
	public static KeyCode GetKeyCode(KeyAction key)
	{
		KeyCode _keyCode = KeyCode.None;
		keyDict.TryGetValue(key, out _keyCode);
		return _keyCode;
	}

	//Use in place of Input.GetKey
	public static bool GetKey(KeyAction key)
	{
		KeyCode _keyCode = KeyCode.None;
		keyDict.TryGetValue(key, out _keyCode);
		return Input.GetKey(_keyCode);
	}

	//Use in place of Input.GetKeyDown
	public static bool GetKeyDown(KeyAction key)
	{
		KeyCode _keyCode = KeyCode.None;
		keyDict.TryGetValue(key, out _keyCode);
		return Input.GetKeyDown(_keyCode);
	}

	//Use in place of Input.GetKeyUP
	public static bool GetKeyUp(KeyAction key)
	{
		KeyCode _keyCode = KeyCode.None;
		keyDict.TryGetValue(key, out _keyCode);
		return Input.GetKeyUp(_keyCode);
	}

	public static void UpdateDictionary(KeyBinding key)
	{
        if (!keyDict.ContainsKey(key.keyAction))
            keyDict.Add(key.keyAction, key.keyCode);
        else
            keyDict[key.keyAction] = key.keyCode;
	}

	public static void UpdateDictionary(KeyAction keyAction, KeyCode keyCode)
	{
		if (!keyDict.ContainsKey(keyAction))
			keyDict.Add(keyAction, keyCode);
		else
			keyDict[keyAction] = keyCode;
	}
}

//used to safe code inputs
//Add new keys to "bind" here
public enum KeyAction
{
	none,
	up,
	down,
	left,
	right,
	jump,
	hack,
	fire,
	hackUp,
	hackDown,
	hackLeft,
	hackRight,
	hackFire,
	hackAimUp,
	hackAimDown,
	hackAimLeft,
	hackAimRight,
}
