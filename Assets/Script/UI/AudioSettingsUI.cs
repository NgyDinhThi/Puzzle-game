using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    public Slider volumeSlider;

    private void Awake()
    {
        if (!volumeSlider) volumeSlider = GetComponent<Slider>();
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        volumeSlider.wholeNumbers = false;
    }

    private void Start()
    {
        float saved = PlayerPrefs.GetFloat(AudioManager.VolumeKey, 0.5f);

        // set slider mà không bắn event
        volumeSlider.SetValueWithoutNotify(saved);

        // áp dụng volume đã lưu (không cần save lại)
        AudioManager.instance.SetAllVolumes(saved, save: false);

        // lắng nghe kéo slider
        volumeSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnDestroy()
    {
        volumeSlider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        // cập nhật toàn hệ thống + lưu PlayerPrefs
        AudioManager.instance.SetAllVolumes(value, save: true);
    }
}
