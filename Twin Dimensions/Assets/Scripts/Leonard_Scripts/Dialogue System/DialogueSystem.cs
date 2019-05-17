using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Dialogue Sequence", menuName = "Dialogue")]
public class DialogueSystem : SerializedScriptableObject
{
    Queue<string> sentences = new Queue<string>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
