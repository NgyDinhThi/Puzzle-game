using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class SquareTextureData : ScriptableObject
{
    [System.Serializable]
    public class TextureData { public Sprite texture; public Config.squareColor color; }

    [SerializeField] private int startThreshold = 100; // <- cho phép truy cập
    public int tresholdVal = 10;                       // giữ như cũ
    public List<TextureData> activeSquareTexture;

    public Config.squareColor currentColors;
    public Config.squareColor _nextColor;

    private int GetCurrentColorIndex()
    {
        for (int i = 0; i < activeSquareTexture.Count; i++)
            if (activeSquareTexture[i].color == currentColors) return i;
        return 0;
    }

    // advance đúng 1 bước màu
    private void AdvanceColorOnce()
    {
        currentColors = _nextColor;
        int idx = GetCurrentColorIndex();
        _nextColor = (idx == activeSquareTexture.Count - 1)
            ? activeSquareTexture[0].color
            : activeSquareTexture[idx + 1].color;
    }

    // Được gọi khi VƯỢT NGƯỠNG trong lúc chơi
    public void UpdateColors(int _ignoredCurrentScore)
    {
        AdvanceColorOnce();
        tresholdVal += startThreshold; // mỗi lần vượt ngưỡng, tăng thêm 100
    }

    // Dùng khi RESTORE: tính màu từ điểm hiện tại
    public void RecalculateByScore(int score)
    {
        SetStartColors();                       // quay về màu đầu
        int steps = Mathf.Max(0, score / startThreshold);  // số lần đã vượt ngưỡng
        for (int i = 0; i < steps; i++) AdvanceColorOnce();
        tresholdVal = startThreshold * (steps + 1);        // ngưỡng tiếp theo
    }

    public void SetStartColors()
    {
        tresholdVal = startThreshold;
        currentColors = activeSquareTexture[0].color;
        _nextColor = activeSquareTexture[1].color;
    }

    private void Awake() { SetStartColors(); }
    private void OnEnable() { SetStartColors(); }
}
