using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Chankiyu22.DialogueSystem.Plots
{

[Serializable]
public class UnityIntEvent : UnityEvent<int> {}

[Serializable]
public class UnityFloatEvent : UnityEvent<float> {}

[Serializable]
public class UnityBoolEvent : UnityEvent<bool> {}

[Serializable]
public class UnityStringEvent : UnityEvent<string> {}

[Serializable]
public class UnityVoidEvent : UnityEvent {}

[Serializable]
public class PlotAdditionalDataListUnityEvent : UnityEvent<List<PlotAdditionalData>> {}

public class PlotAdditionalDataListEventArgs : EventArgs
{
    public List<PlotAdditionalData> additionalDataList;
}

}
