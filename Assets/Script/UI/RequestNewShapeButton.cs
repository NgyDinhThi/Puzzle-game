using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class RequestNewShapeButton : MonoBehaviour
{
    public int numberRequest = 3;
    public TextMeshProUGUI numberText;
   

    private int _currentNumberRequest;
    private Button _button;
    private bool _isLocked;

    private void Start()
    {
        _currentNumberRequest = numberRequest;
        numberText.text = _currentNumberRequest.ToString();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonDown);
        Unlock();
    }
    private void OnEnable()
    {
        GameEvents.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.GameOver -= OnGameOver;
    }

    private void OnGameOver(bool newBestScore)
    {
        Lock(); 
    }

    private void OnButtonDown()
    {
        if (_isLocked == false)
        {
            _currentNumberRequest--;
            GameEvents.RequestNewShape();
            GameEvents.CheckifPlayerLost();

            if (_currentNumberRequest <= 0)
            {
                Lock() ;
            }
            numberText.text = _currentNumberRequest.ToString();
        }
    }

    private void Lock()
    {
       _isLocked = true;
        _button.interactable = false;
        numberText.text = _currentNumberRequest.ToString();
    }

    private void Unlock()
    {
        _isLocked = false;
        _button.interactable = true;
    }    
}
