using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance { get; private set; }

    [SerializeField] private Light directionalLight;
    [SerializeField] private float dayDuration = 24f; // 실제 하루의 길이 (분)
    [SerializeField] private Text timeText; // 시간을 표시할 텍스트

    [SerializeField] private float timeOfDay; // 0: 00시, 0.5: 12시 1: 24시

    void Awake()
    {
        // 싱글톤 패턴 적용
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
        // dayDration의 값만큼 계속 더해서 1(24시)까지 맞추는 것
        timeOfDay += Time.deltaTime / (dayDuration * 60); // dayDuration * 60 : 분을 초로 바꾼다
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
        // 24시간 형식으로 현재 시간을 계산
        float hours = Mathf.Floor(timeOfDay * 24f);
        float minutes = Mathf.Floor((timeOfDay * 24f * 60f) % 60f);

        timeText.text = string.Format("{0:00}:{1:00}", hours, minutes);
    }
}
