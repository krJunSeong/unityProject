using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemGetEffect : MonoBehaviour
{
    [SerializeField] GameObject[] spritePrefabs; // 0: ���� ����Ʈ �̹��� 1: Ʈ�� ����Ʈ �̹���
    [SerializeField] Text[] texts;               // 0: ���� UI Text      1: Tree UI Text
    Dictionary<string, Text> dTexts;
    [SerializeField] int poolSize = 30;          // Ǯ ũ��
    [SerializeField] float effectSpeed = 0.2f;  // ����Ʈ ȿ�� �ð�

    Dictionary<string, GameObject[]> dSpritePool = new Dictionary<string, GameObject[]>();
    private void Awake()
    {
        InitializePool();
        InitText();
    }

    void InitializePool()
    {
        GameObject gameObject = new GameObject("poolingImage");
        gameObject.transform.SetParent(this.transform);
        
        // ��������Ʈ Ǯ ����
        for (int i = 0; i < spritePrefabs.Length; i++)
        {
            GameObject[] pool = new GameObject[poolSize];
            GameObject parent = new GameObject(spritePrefabs[i].name.Split('_')[1] + 's');
            parent.transform.SetParent(this.transform);
            for (int j = 0; j < poolSize; j++)
            {
                GameObject spriteObject = Instantiate(spritePrefabs[i]);
                spriteObject.SetActive(false); // ��Ȱ��ȭ ���·� ����
                spriteObject.transform.SetParent(parent.transform);
                pool[j] = spriteObject;
            }

            //dSpritePool[pool[0].name] = pool;
            dSpritePool[spritePrefabs[i].name.Split('_')[1]] = pool;
        }
    }
    void InitText()
    {
        dTexts = new Dictionary<string, Text>();
        for(int i = 0; i < texts.Length; i++)
        {
            // (img_Stone).split -> [0]: img, [1]: Stone
            dTexts[texts[i].name.Split('_')[1]] = texts[i];
        }
    }
    public void UseImage(string imageName, Vector3 pos, int cnt)
    {
        // pooling�� ���� ������ŭ ��ġ�� ����� �Լ�, GameManager���� �۵� ����
        // �̹��� ����
        // 1. ��ųʸ����� �ش� �̹����� ���� �´�.
        // �̹����� �� ���̰� �ϰ� ������ �̰͵� �ڷ�ƾ���� �ۼ��ؼ� 0.1f�ʸ��� ���� �͵� �����.
        // 2. �� �̹����� ��ġ�� setactive On �Ѵ�
        // 3. �̹����� Enumrator�� Lerp�� while�� ������ �����ϸ� SetActive false�� �Ѵ�.
        StartCoroutine(PresentImage(imageName, pos, cnt));
    }

    IEnumerator PresentImage(string imageName, Vector3 pos, int cnt)
    {
        for (int j = 0; j < dSpritePool[imageName].Length; j++)
        {
            if (dSpritePool[imageName][j].activeSelf) continue;

            dSpritePool[imageName][j].SetActive(true);
            dSpritePool[imageName][j].transform.position = Camera.main.WorldToScreenPoint(pos);
            if (imageName == "Stone")
                StartCoroutine(ThrowImage(dSpritePool[imageName][j].transform, texts[0].rectTransform.position, imageName));
            else
                StartCoroutine(ThrowImage(dSpritePool[imageName][j].transform, texts[1].rectTransform.position, imageName));
            cnt--;

            if (cnt < 1) break;

            yield return new WaitForSeconds(effectSpeed);
        }
    }
    IEnumerator ThrowImage(Transform imageTrs, Vector3 targetPos, string name)
    {
        // tree, stone�� ��ġ�� �� �Լ� �۵���Ű�� ������ �Ǻ��� ��.
        Vector3 startPosition = imageTrs.position;
        float elapsedTime = 0;
        float moveTime = 1.0f;

        // �ð��� moveTime���� ���� ���� ����ؼ� ������
        while (elapsedTime < moveTime)
        {
            // �ð� ����� ���� ����
            imageTrs.position = Vector3.Lerp(startPosition, targetPos, (elapsedTime / moveTime));

            // �ð� ������Ʈ
            elapsedTime += Time.deltaTime;

            if (elapsedTime > 0.95f)
            {
                imageTrs.gameObject.SetActive(false);
                // ���⼭ UIText ++, �÷��̾�� Give Item to Player

                break;
            }
            // �� ������ ���
            yield return null;
        }

        GameManager.Instance.GiveMaterialItemToPlayer(name, 1);
        dTexts[name].text = GameManager.Instance.GetPlayerInventory(name).ToString(); // �� ������ �޴� �Լ�
    }
}
