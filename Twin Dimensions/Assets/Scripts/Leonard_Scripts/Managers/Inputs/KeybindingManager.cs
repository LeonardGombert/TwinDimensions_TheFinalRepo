using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Keybindings", menuName = "Keybindings")]
public class KeybindingManager : SerializedScriptableObject
{
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    
    public KeyCode teleport;
    public KeyCode selectEnemies;
    public KeyCode placeStatue;
    public KeyCode kickStatue;

    public KeyCode selectedPortalExit;

    public KeyCode cameraZoomOut;
    public KeyCode previewOtherWorld;
    public KeyCode resetScene;

    public KeyCode chargeElephant;

    //public KeyCode ; 


    public KeyCode CheckKey(string key)
    {
        switch(key)
        {
            case "up": return up;
            case "down": return down;
            case "left": return left;
            case "right": return right;
            
            case "teleport": return teleport;
            case "selectEnemies": return selectEnemies;
            case "placeStatue": return placeStatue;
            case "kickStatue": return kickStatue;

            case "selectedPortalExit": return selectedPortalExit;
            case "cameraZoomOut" : return cameraZoomOut;

            case "previewOtherWorld": return previewOtherWorld;
            case "resetScene": return resetScene;
            
            case "chargeElephant": return chargeElephant;

            //case"": return;
            default: return KeyCode.None;
        }
    }
}
