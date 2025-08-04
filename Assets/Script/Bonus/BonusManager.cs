using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
   public List<GameObject> bonusList;

    private void Start()
    {
        GameEvents.ShowBonus += ShowBonusScreen;
    }

    private void OnDisable()
    {
        GameEvents.ShowBonus -= ShowBonusScreen;
        
    }

    private void ShowBonusScreen(Config.squareColor color)
    {
        GameObject obj = null;
        foreach (var bonus in bonusList)
        {
            var bonusComp = bonus.GetComponent<Bonus>();
            if (bonusComp.color == color)
            {
                obj = bonus;
                bonus.SetActive(true);
            }
        }

        StartCoroutine(DeactiveBonus(obj)); 

    }   
    
    private IEnumerator DeactiveBonus(GameObject obj)
    {
        yield return new WaitForSeconds(2);
        obj.SetActive(false);


    }    
}
