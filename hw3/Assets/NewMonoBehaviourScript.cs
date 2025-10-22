using UnityEngine;
using UnityEngine.UI;

public class WheelController : MonoBehaviour
{
    public Transform wheel;             // 指向轉盤物件的 Transform
    public float spinDuration = 4f;     // 旋轉總時長
    public int sectorCount = 6;         // 扇區數量（這裡是6）
    public Text resultText;             // 顯示結果的 UI Text（可選）

    private bool isSpinning = false;

    public void Spin()
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinWheel());
        }
    }

    private System.Collections.IEnumerator SpinWheel()
    {
        isSpinning = true;

        float elapsed = 0f;
        float startAngle = wheel.eulerAngles.z;
        float totalAngle = 360f * Random.Range(5, 8); // 隨機轉5~8圈
        float targetAngle = totalAngle + Random.Range(0, 360f); // 再加一點偏移

        float finalAngle = startAngle + targetAngle;

        while (elapsed < spinDuration)
        {
            float t = elapsed / spinDuration;
            float currentAngle = Mathf.Lerp(startAngle, finalAngle, EaseOutCubic(t));
            wheel.eulerAngles = new Vector3(0, 0, currentAngle);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 確保最終角度對齊
        wheel.eulerAngles = new Vector3(0, 0, finalAngle);

        int landedSector = GetSectorIndex(wheel.eulerAngles.z);
        Debug.Log("Landed on Sector: " + landedSector);
        if (resultText != null)
            resultText.text = "結果: 第 " + landedSector + " 格";

        isSpinning = false;
    }

    // 計算落在哪個扇區（0 ~ 5）
    private int GetSectorIndex(float angle)
    {
        float normalizedAngle = angle % 360;
        float sectorAngle = 360f / sectorCount;
        int index = sectorCount - 1 - Mathf.FloorToInt(normalizedAngle / sectorAngle);
        return index;
    }

    // 緩動函數（Ease Out Cubic）
    private float EaseOutCubic(float t)
    {
        t--;
        return t * t * t + 1;
    }
}