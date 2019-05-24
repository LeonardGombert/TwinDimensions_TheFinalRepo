using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Text nameText;
	public Text dialogueText;

	public Animator animator;

	private Queue<string> sentenceQueue = new Queue<string>();

	// Use this for initialization
	void Start () 
	{

	}

	public void StartDialogue (Dialogue dialogue)
	{
		animator.SetBool("IsOpen", true);

		nameText.text = dialogue.name;

		sentenceQueue.Clear();

		if (LanguageSelection.frenchDialogue)
        {
            foreach (string sentence in dialogue.frenchSentences)
            {
                sentenceQueue.Enqueue(sentence);
            }
        }

		if (LanguageSelection.englishDialogue)
        {
            foreach (string sentence in dialogue.englishSentences)
            {
                sentenceQueue.Enqueue(sentence);
            }
        }

        DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if (sentenceQueue.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentenceQueue.Dequeue();
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
