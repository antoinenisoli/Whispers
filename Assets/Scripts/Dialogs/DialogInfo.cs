using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogInfo", menuName = "Dialogs/Profile")]
public class DialogInfo : ScriptableObject
{
    [TextArea] public string subText;
    public AudioClip clip;
}
