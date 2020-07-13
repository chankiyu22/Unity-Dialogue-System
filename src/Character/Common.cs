using System;
using UnityEngine.Events;

namespace Chankiyu22.DialogueSystem.Characters
{

[Serializable]
public class CharacterUnityEvent : UnityEvent<Character> {}

public class CharacterEventArgs : EventArgs
{
    public Character character;
}

[Serializable]
public class IntUnityEvent : UnityEvent<int> {}

public class IntEventArgs : EventArgs
{
    public int value;
}

[Serializable]
public class FloatUnityEvent : UnityEvent<float> {}

public class FloatEventArgs : EventArgs
{
    public float value;
}

[Serializable]
public class BoolUnityEvent : UnityEvent<bool> {}

public class BoolEventArgs : EventArgs
{
    public bool value;
}

[Serializable]
public class StringUnityEvent : UnityEvent<string> {}

public class StringEventArgs : EventArgs
{
    public string value;
}

}