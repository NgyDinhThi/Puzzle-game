using UnityEngine;

public class AutoResetOnGameOver : MonoBehaviour
{
    [Header("Refs")]
    public GameGrid grid;
    public ShapeStorage shapeStorage;

    private void Awake()
    {
        if (grid == null)
            grid = Object.FindFirstObjectByType<GameGrid>(FindObjectsInactive.Include);
        if (shapeStorage == null)
            shapeStorage = Object.FindFirstObjectByType<ShapeStorage>(FindObjectsInactive.Include);
    }

    // KHÔNG subscribe GameOver nữa — reset chỉ khi gọi tay
    public void ResetNow(bool alsoResetScoreAndRequest = true)
    {
        // 1) Xóa sạch các ô trên grid
        if (grid != null && grid.gridSquares != null)
        {
            foreach (var go in grid.gridSquares)
            {
                var cell = go.GetComponent<GridSquare>();
                if (cell == null) continue;
                cell.Deactivate();
                cell.ClearOccupied();
            }
        }

        // 2) Dọn khay shape
        if (shapeStorage != null && shapeStorage.shapeList != null)
        {
            foreach (var sh in shapeStorage.shapeList)
            {
                sh.ClearCurrentShape();
                sh.DeactivateAfterPlacement(); // coi như slot trống
            }

            // phát bộ shape mới
            GameEvents.RequestNewShape?.Invoke();
        }

        // 3) Xóa save để không load lại ván cũ
        var gp = GamePersistence.Instance;
        if (gp != null)
        {
            gp.BeginHardReset();
            gp.DeleteSave();
            gp.EndHardReset();
        }

        // 4) (tuỳ) reset Request button + điểm hiện tại
        if (alsoResetScoreAndRequest)
        {
            var req = Object.FindFirstObjectByType<RequestNewShapeButton>(FindObjectsInactive.Include);
            if (req != null) req.SetRemaining(req.numberRequest);

            var score = Object.FindFirstObjectByType<Score>(FindObjectsInactive.Include);
            if (score != null) score.RestoreCurrentScore(0);
        }

        // 5) đảm bảo game đang chạy (nếu bạn có pause khi game over)
        Time.timeScale = 1f;
    }
}
