using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongratulationWritting : MonoBehaviour
{
    public List<GameObject> writtings;
    void Start()
    {
        GameEvent.ShowCongratulationWritting += ShowCongratulationWritting;
    }

    void OnDisable()
    {
        GameEvent.ShowCongratulationWritting -= ShowCongratulationWritting;
    }

    void ShowCongratulationWritting()
    {
        var index = UnityEngine.Random.Range(0, writtings.Count);
        writtings[index].SetActive(true);
    }
}
