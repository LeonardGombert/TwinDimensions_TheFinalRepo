using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using UnityEngine.Tilemaps;

public class ExtensionMethods
{   
    public static void RecenterOnGrid(GameObject objectToCenter, Tilemap generalTilemap)
    {
        objectToCenter.transform.position = generalTilemap.WorldToCell(objectToCenter.transform.position);
    }
    
    #region //USED BY ABILITY1, ABILITY2 and MOVEMENT
    //Get World Position
    public static Vector2 getFlooredWorldPosition(Vector2 worldPosition)
    {
        worldPosition.x = Mathf.Floor(worldPosition.x);
        worldPosition.y = Mathf.Floor(worldPosition.y);
        
        return worldPosition;
    }
    
    //USED BY MOVEMENT
    //Return positive of negative sign of F
    public static Vector2 ReturnSign(Vector2 vector)
    {
        vector.x = Mathf.Sign(vector.x);
        vector.y = Mathf.Sign(vector.y);

        return vector;
    }

    //USED BY ABILITY1 and MOVEMENT
    //Round Value to first int smaller than F
    public static Vector2 FloorVect3Values(Vector2 vector)
    {
        vector.x = Mathf.Floor(vector.x);
        vector.y = Mathf.Floor(vector.y);

        return vector;
    }

    public static Vector2 RemoveDiagonalsForPlayer(Vector2 vector)
    {
        vector.x = Mathf.Abs((int)Input.GetAxisRaw("Horizontal"));
        vector.y = Mathf.Abs((int)Input.GetAxisRaw("Vertical"));

        if (vector.x != 0) vector.y = 0;
               
        return vector;
    }

    public static Vector2 RemoveDiagonalsForStatues(Vector2 vector)
    {
        vector.x = Mathf.Floor(vector.x);
        vector.y = Mathf.Floor(vector.y);
        
        //if (vector.x > vector.y)vector.y = 0;
               
        return vector;
    }

    public static Vector2 RemoveDiagonalsForEntities(Vector2 vector)
    {
        vector.x = Mathf.FloorToInt(vector.x);
        vector.y = Mathf.FloorToInt(vector.y);
               
        return vector;
    }

    //Get position with 0,5
    public static Vector2 floorWithHalfOffset(Vector2 worldPosition)
    {
        worldPosition.x = Mathf.Floor(worldPosition.x * 2f) * 0.5f;
        worldPosition.y = Mathf.Floor(worldPosition.y * 2f) * 0.5f;

        return worldPosition;
    }

    //Round value to the nearest int (larger or smaller)
    public static Vector3 RoundVect3(Vector3 vector)
    {
        vector.x = Mathf.Round(vector.x * 10) / 10;
        vector.y = Mathf.Round(vector.y * 10) / 10;

        return vector;
    }
    #endregion
}
