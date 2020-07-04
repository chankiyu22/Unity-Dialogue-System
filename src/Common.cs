using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Chankiyu22.DialogueSystem
{

[Serializable]
public class DialogueUnityEvent: UnityEvent<Dialogue> { }

public class DialogueEventArgs: EventArgs
{
    public Dialogue dialogue;
}

[Serializable]
public class DialogueTextUnityEvent: UnityEvent<DialogueText> { }

public class DialogueTextEventArgs: EventArgs
{
    public DialogueText dialogueText;
}

[Serializable]
public class DialogueOptionsUnityEvent: UnityEvent<List<DialogueOption>> { }

public class DialogueOptionsEventArgs: EventArgs
{
    public List<DialogueOption> dialogueOptions;
}

[Serializable]
public class DialogueOptionUnityEvent: UnityEvent<DialogueOption> { }

public class DialogueOptionEventArgs: EventArgs
{
    public DialogueOption dialogueOption;
}

[Serializable]
public class StringUnityEvent: UnityEvent<string> { }

public class StringEventArgs: EventArgs
{
    public string value;
}

}
