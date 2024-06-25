using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FontManager : MonoBehaviour
{
    public static FontManager Instance { get; private set; }

    [SerializeField] private Text damageTextPrefab; // ������ �ؽ�Ʈ ������
    [SerializeField] private Canvas canvas; // �������� ǥ���� ĵ����
    [SerializeField] private int poolSize = 10; // Ǯ�� ũ��

    private Queue<Text> textPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeTextPool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeTextPool()
    {
        textPool = new Queue<Text>();
        for (int i = 0; i < poolSize; i++)
        {
            Text instance = Instantiate(damageTextPrefab, canvas.transform);
            instance.gameObject.SetActive(false);
            textPool.Enqueue(instance);
        }
    }

    public void ShowDamage(float damage, Vector3 position, Color color = default(Color))
    {
        if (color == default(Color))
        {
            color = Color.white; // �⺻ ������ ����
        }

        if (textPool.Count > 0)
        {
            Text instance = textPool.Dequeue();
            instance.text = damage.ToString();
            instance.color = color;
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
            instance.transform.position = screenPosition;
            instance.gameObject.SetActive(true);

            StartCoroutine(FadeOutAndReturnToPool(instance));
        }
    }

    private IEnumerator FadeOutAndReturnToPool(Text textInstance)
    {
        CanvasGroup canvasGroup = textInstance.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = textInstance.gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1f;
        float duration = 1f; // �ؽ�Ʈ�� ����� �������� �ð�
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
            yield return null;
        }

        textInstance.gameObject.SetActive(false);
        textPool.Enqueue(textInstance);
    }
}
