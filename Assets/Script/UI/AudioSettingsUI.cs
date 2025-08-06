using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    public Slider volumeSlider;

    private void Start()
    {
        // Nếu muốn mặc định 0.5
        volumeSlider.value = 0.5f;

        // Gán giá trị volume ban đầu cho tất cả sound
        foreach (Sound s in AudioManager.instance.sounds)
        {
            s.volume = volumeSlider.value;
        }

        // Gán sự kiện khi kéo slider
        volumeSlider.onValueChanged.AddListener(HandleVolumeChanged);
    }

    private void HandleVolumeChanged(float value)
    {
        // Cập nhật volume cho tất cả sound
        foreach (Sound s in AudioManager.instance.sounds)
        {
            s.volume = value;
        }
    }
}
