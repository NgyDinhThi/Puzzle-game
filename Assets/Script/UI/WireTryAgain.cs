using UnityEngine;
using UnityEngine.UI;

public class WireTryAgain : MonoBehaviour
{
    public Button tryAgainBtn;
    public GameOverPopup popup;

    void Start()
    {
        tryAgainBtn.onClick.AddListener(popup.OnTryAgainClicked);
    }
}
