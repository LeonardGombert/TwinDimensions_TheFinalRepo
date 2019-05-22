using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue 
{
	public string name;

	[TextArea(3, 10)]
	public string[] sentences;
	[TextArea(3, 10)]
	public string[] englishSentences;
	[TextArea(3, 10)]
	public string[] frenchSentences;

	void Start () 
	{
		if(LanguageSelection.englishDialogue) sentences = englishSentences;
		if(LanguageSelection.frenchDialogue) sentences = frenchSentences;		
	}
}
