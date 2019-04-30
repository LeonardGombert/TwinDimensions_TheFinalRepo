using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class CollisionManager : MonoBehaviour
{
    [SerializeField]
    List <GameObject> solids;
    [SerializeField]
    List <Tilemap> edges;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static bool IsWalkable(Tilemap hitTilemap = null, GameObject yes2 = null)
    {
        return true;
    }

}
