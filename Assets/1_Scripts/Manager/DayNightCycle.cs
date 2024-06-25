using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance { get; private set; }

    [SerializeField] private Light directionalLight;
    [SerializeField] private float dayDuration = 24f; // ���� �Ϸ��� ���� (��)
    [SerializeField] private Text timeText; // �ð��� ǥ���� �ؽ�Ʈ

    [SerializeField] private float timeOfDay; // 0: 00��, 0.5: 12�� 1: 24��

    void Awake()
    {
        // �̱��� ���� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (directionalLight == null)
        {
            directionalLight = GetComponent<Light>();
        }
    }

    void Update()
    {
        UpdateTimeOfDay();
        UpdateLightRotation();
        UpdateTimeText();
    }

    void UpdateTimeOfDay()
    {
        // dayDration�� ����ŭ ��� ���ؼ� 1(24��)���� ���ߴ� ��
        timeOfDay += Time.deltaTime / (dayDuration * 60); // dayDuration * 60 : ���� �ʷ� �ٲ۴�
        if (timeOfDay >= 1)
        {
            timeOfDay = 0; 
        }
    }

    void UpdateLightRotation()
    {
        float sunAngle = Mathf.Lerp(0, 360, timeOfDay);
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3(sunAngle - 90, 170, 0)); 
    }

    void UpdateTimeText()
    {
        // 24�ð� �������� ���� �ð��� ���
        float hours = Mathf.Floor(timeOfDay * 24f);
        float minutes = Mathf.Floor((timeOfDay * 24f * 60f) % 60f);

        timeText.text = string.Format("{0:00}:{1:00}", hours, minutes);
    }
}
