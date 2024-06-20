using System.Collections;
using DarkenSoda.CustomTools.Scripts.Attributes;
using UnityEngine;

[System.Serializable]
public class TestScript2
{
    public int t;

    [Required]
    public string requiredString;

    [Button]
    public void ClickMe(string s, bool a)
    {
        Debug.Log(s + " " + a);
        Debug.Log("Button pressed!");
    }

    [Button(playModeOnly = true)]
    public IEnumerator Enumerator()
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("E" + i);
            yield return new WaitForSecondsRealtime(1);
        }
    }
}
