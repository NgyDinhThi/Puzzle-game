using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GamePersistence : MonoBehaviour
{
    public static GamePersistence Instance;

    [Header("Refs (kéo từ scene)")]
    public GameGrid grid;
    public ShapeStorage shapeStorage;
    public RequestNewShapeButton requestBtn;
    public Score scoreManager;

    private const string SaveKey = "gs_grid_v2";
    private bool _isLoading = false;
    private bool _loadedOnce = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        GameEvents.RequestNewShape += OnSomethingChanged;
        GameEvents.CheckIfShapeCanbePlaced += OnSomethingChanged;
        GameEvents.AddScores += _ => OnSomethingChanged();   // điểm đổi cũng lưu
        SceneManager.activeSceneChanged += (_, __) => SaveNow();        // đổi scene thì lưu
    }

    private void OnDisable()
    {
        GameEvents.RequestNewShape -= OnSomethingChanged;
        GameEvents.CheckIfShapeCanbePlaced -= OnSomethingChanged;
        SceneManager.activeSceneChanged -= (_, __) => SaveNow();
    }

    private void OnSomethingChanged() { StartCoroutine(SaveNextFrame()); }

    private IEnumerator SaveNextFrame()
    {
        if (_isLoading || !_loadedOnce) yield break;
        yield return null;
        SaveNow();
    }

    public void SaveNow()
    {
        if (_isLoading || !_loadedOnce) return;
        if (grid == null || grid.gridSquares == null || grid.gridSquares.Count == 0) return;
        if (shapeStorage == null) return;

        var data = new GameStateData
        {
            rows = grid.rows,
            columns = grid.columns,
            cells = new List<CellData>(grid.gridSquares.Count),
            requestLeft = requestBtn != null ? requestBtn.GetRemaining() : 0,
            trayShapeIndices = new List<int>(shapeStorage.shapeList.Count),
            currentScore = scoreManager != null ? scoreManager.GetCurrentScore() : 0
        };

        // Lưu board
        foreach (var go in grid.gridSquares)
        {
            var cell = go.GetComponent<GridSquare>();
            data.cells.Add(new CellData
            {
                o = cell.SquareOccupied,
                c = cell.SquareOccupied ? (int)cell.GetCurrentColor() : (int)Config.squareColor.NotSet
            });
        }

        // Lưu 3 shape trong khay
        for (int i = 0; i < shapeStorage.shapeList.Count; i++)
        {
            var s = shapeStorage.shapeList[i];
            data.trayShapeIndices.Add(s.IsPlaced() ? -1 : shapeStorage.shapeData.IndexOf(s.currentShapeData));
        }

        BinaryDataStream.Save(data, SaveKey);
        // Debug.Log("[Persist] Saved");
    }

    public void LoadNow()
    {
        if (!BinaryDataStream.Exist(SaveKey)) { _loadedOnce = true; return; }
        if (grid == null || grid.gridSquares == null || grid.gridSquares.Count == 0) return;
        if (shapeStorage == null) return;

        _isLoading = true;

        var data = BinaryDataStream.Read<GameStateData>(SaveKey);

        // 1) Khôi phục điểm trước để palette/màu hiện hành đúng ngay từ đầu
        if (scoreManager != null)
            scoreManager.RestoreCurrentScore(data.currentScore);

        // (tùy chọn) bắn lại event màu cho chắc
        GameEvents.UpdateSquareColor?.Invoke(scoreManager.squareTextureData.currentColors);

        // 2) Áp board
        int cnt = Mathf.Min(data.cells.Count, grid.gridSquares.Count);
        for (int i = 0; i < cnt; i++)
        {
            var comp = grid.gridSquares[i].GetComponent<GridSquare>();
            var cd = data.cells[i];
            if (cd.o) comp.PlaceShapeOnBoard((Config.squareColor)cd.c);
            else comp.ClearOccupied();
        }

        // 3) Áp số lần Request
        if (requestBtn != null)
            requestBtn.SetRemaining(data.requestLeft);

        // 4) Khôi phục 3 shape trong khay (lúc này palette đã đúng)
        if (data.trayShapeIndices != null)
        {
            int n = Mathf.Min(data.trayShapeIndices.Count, shapeStorage.shapeList.Count);
            for (int i = 0; i < n; i++)
            {
                var shape = shapeStorage.shapeList[i];
                shape.ClearCurrentShape();

                int idx = data.trayShapeIndices[i];
                if (idx >= 0 && idx < shapeStorage.shapeData.Count)
                {
                    shape.RequestNewshape(shapeStorage.shapeData[idx]); // build theo palette mới
                    shape.ActivateShape();
                }
                else
                {
                    shape.DeactivateAfterPlacement(); // slot trống
                }
            }
        }

        _isLoading = false;
        _loadedOnce = true;
    }


    private void OnApplicationPause(bool pause) { if (pause) SaveNow(); }
    private void OnApplicationQuit() { SaveNow(); }

    public void DeleteSave()
    {
        if (BinaryDataStream.Exist(SaveKey))
            BinaryDataStream.Delete(SaveKey);
    }

    // Dùng khi muốn reset cứng (ví dụ Try Again reload scene, không load lại save vừa xóa)
    public void BeginHardReset() { _isLoading = true; _loadedOnce = false; }
    public void EndHardReset() { _isLoading = false; _loadedOnce = true; }
}
