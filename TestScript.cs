using System.Collections;
using DarkenSoda.CustomTools.Scripts.Attributes;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public int number;
    public TestScript2 testScript2;
    public bool AA;

    [Button]
    public void TestButton(string s, bool a)
    {
        Debug.Log(s + " " + a);
        Debug.Log("Button pressed!");
    }

    [Button(playModeOnly = true)]
    public IEnumerator E(int n)
    {
        for (int i = 0; i < n; i++)
        {
            Debug.Log("E" + i);
            yield return new WaitForSecondsRealtime(1);
        }
    }

}
