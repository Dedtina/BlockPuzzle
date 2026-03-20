using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RequestNewShapeBtn : MonoBehaviour
{
    public int numberOfRequest = 3;
    public TextMeshProUGUI numberText;

    private int currentNumberOfRequest;
    private Button _button;
    private bool isLocked;

    void Start()
    {
        currentNumberOfRequest = numberOfRequest;
        numberText.text = currentNumberOfRequest.ToString();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonDown);
        Unlock();
    }

    private void OnButtonDown()
    {
        if(isLocked == false)
        {
            currentNumberOfRequest--;
            GameEvent.RequestNewShape();
            GameEvent.CheckIfPlayerLost();

            if(currentNumberOfRequest <= 0)
            {
                Lock();
            }
            numberText.text = currentNumberOfRequest.ToString();
        }
    }

    private void Lock()
    {
        isLocked = true;
        _button.interactable = false;
        numberText.text = currentNumberOfRequest.ToString();
    }

    private void Unlock()
    {
        isLocked = false;
        _button.interactable = true;
    }
}
