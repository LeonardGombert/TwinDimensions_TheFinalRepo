using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public struct Dialogue 
{
	public string name;

	[TextArea(3, 10)]
	public string[] mySentences;

	[TextArea(3, 10)]
	public string[] englishSentences;
	[TextArea(3, 10)]
	public string[] frenchSentences;
}
