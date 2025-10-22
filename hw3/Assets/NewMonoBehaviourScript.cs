using UnityEngine;
using UnityEngine.UI;

public class WheelController : MonoBehaviour
{
    public Transform wheel;             // ���V��L���� Transform
    public float spinDuration = 4f;     // �����`�ɪ�
    public int sectorCount = 6;         // ���ϼƶq�]�o�̬O6�^
    public Text resultText;             // ��ܵ��G�� UI Text�]�i��^

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
        float totalAngle = 360f * Random.Range(5, 8); // �H����5~8��
        float targetAngle = totalAngle + Random.Range(0, 360f); // �A�[�@�I����

        float finalAngle = startAngle + targetAngle;

        while (elapsed < spinDuration)
        {
            float t = elapsed / spinDuration;
            float currentAngle = Mathf.Lerp(startAngle, finalAngle, EaseOutCubic(t));
            wheel.eulerAngles = new Vector3(0, 0, currentAngle);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // �T�O�̲ר��׹��
        wheel.eulerAngles = new Vector3(0, 0, finalAngle);

        int landedSector = GetSectorIndex(wheel.eulerAngles.z);
        Debug.Log("Landed on Sector: " + landedSector);
        if (resultText != null)
            resultText.text = "���G: �� " + landedSector + " ��";

        isSpinning = false;
    }

    // �p�⸨�b���Ӯ��ϡ]0 ~ 5�^
    private int GetSectorIndex(float angle)
    {
        float normalizedAngle = angle % 360;
        float sectorAngle = 360f / sectorCount;
        int index = sectorCount - 1 - Mathf.FloorToInt(normalizedAngle / sectorAngle);
        return index;
    }

    // �w�ʨ�ơ]Ease Out Cubic�^
    private float EaseOutCubic(float t)
    {
        t--;
        return t * t * t + 1;
    }
}