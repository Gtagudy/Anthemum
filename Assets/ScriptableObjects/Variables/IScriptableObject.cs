using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IScriptableObject : ScriptableObject
{
	[TextArea] public string entityName;
	[TextArea] public string entityDescription;
    //[Icon] private string m_Icon;

    public class ResetData { };

    public class Initialize { };
}
