using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Dialogue Sequence", menuName = "Dialogue")]
public class DialogueSystem : MonoBehaviour// : SerializedScriptableObject
{
    Queue<string> sentences = new Queue<string>();

    public string identifier;
    public string text;
    public List<string> options;
    public List<string> optionIdentifiers;
    public string onCloseFunction;

    public Text nameText;
	public Text dialogueText;

	public Animator animator;

    [ShowInInspector] Dictionary<string, Dialogue> dict = new Dictionary<string, Dialogue>();

    void Awake()
    {
        
    }

    public void Dialogue(string identifier, string text, List<string> options, List<string> optionIdentifiers, string onCloseFunction = "")
    {
        this.identifier = identifier;
        this.text = text;
        this.options = options;
        this.optionIdentifiers = optionIdentifiers;
        this.onCloseFunction = onCloseFunction;
    }

    public void StartDialogue (Dialogue dialogue)
	{
		animator.SetBool("IsOpen", true);

		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
	}

    
}
