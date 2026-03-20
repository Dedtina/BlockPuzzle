using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public List<GameObject> bonusesList;

    void Start()
    {
        GameEvent.ShowBonusScreen += ShowBonusScreen;
    }
    void OnDisable()
    {
        GameEvent.ShowBonusScreen -= ShowBonusScreen;
    }

    private void ShowBonusScreen(Config.SquareColor color)
    {
        GameObject obj = null;
        foreach(var bonus in bonusesList)
        {
            var bonusComp = bonus.GetComponent<Bonus>();
            if(bonusComp.color == color)
            {
                obj = bonus;
                bonus.SetActive(true);
            }
        }
        StartCoroutine(DeactivateBonusScreen(obj));
    }

    private IEnumerator DeactivateBonusScreen(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
        obj.SetActive(false);
    }
}
