using UnityEngine;
using UnityEngine.UI;
public class SettingButton : MonoBehaviour
{
    public Button openSettingButton;
    public Button closeSettingButton;
    
    public void SettingOpen()
    {
        openSettingButton.gameObject.SetActive(false);
        closeSettingButton.gameObject.SetActive(true);
        closeSettingButton.interactable = true;
    }    
    public void SettingClose()
    {
        openSettingButton.gameObject.SetActive(true);
        closeSettingButton.gameObject.SetActive(false);
        openSettingButton.interactable = true;


    }    
}
