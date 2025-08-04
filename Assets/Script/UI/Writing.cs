using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Writing : MonoBehaviour
{
    public List<GameObject> writing;


    void Start()
    {
        GameEvents.ShowWritingWord += ShowWriting;
    }

    private void OnDisable()
    {
        GameEvents.ShowWritingWord -= ShowWriting;
        
    }

    private void ShowWriting()
    {
        var index = UnityEngine.Random.Range(0, writing.Count);
        writing[index].SetActive(true); 
    }
}
