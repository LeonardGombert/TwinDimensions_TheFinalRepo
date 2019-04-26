using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Keybindings", menuName = "Keybindings")]
public class KeybindingManager : SerializedScriptableObject
{
    public KeyCode forward;
    public KeyCode backward;
    public KeyCode left;
    public KeyCode right;
    
    public KeyCode teleport;
    public KeyCode selectEnemies;
    public KeyCode placeStatue;
    public KeyCode kickStatue;

    public KeyCode CheckKey(string key)
    {
        switch(key)
        {
            case "teleport": return teleport;
            case "selectEnemies": return selectEnemies;
            case "placeStatue": return placeStatue;
            case "kickStatue": return kickStatue;
            default: return KeyCode.None;
        }
    }
}
