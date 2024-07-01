using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemGetEffect : MonoBehaviour
{
    [SerializeField] GameObject[] spritePrefabs; // 0: 스톤 이펙트 이미지 1: 트리 이펙트 이미지
    [SerializeField] Text[] texts;               // 0: 스톤 UI Text      1: Tree UI Text
    Dictionary<string, Text> dTexts;
    [SerializeField] int poolSize = 30;          // 풀 크기
    [SerializeField] float effectSpeed = 0.2f;  // 이펙트 효과 시간

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
        
        // 스프라이트 풀 생성
        for (int i = 0; i < spritePrefabs.Length; i++)
        {
            GameObject[] pool = new GameObject[poolSize];
            GameObject parent = new GameObject(spritePrefabs[i].name.Split('_')[1] + 's');
            parent.transform.SetParent(this.transform);
            for (int j = 0; j < poolSize; j++)
            {
                GameObject spriteObject = Instantiate(spritePrefabs[i]);
                spriteObject.SetActive(false); // 비활성화 상태로 설정
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
        // pooling한 것을 개수만큼 위치에 만드는 함수, GameManager에서 작동 예정
        // 이미지 구현
        // 1. 딕셔너리에서 해당 이미지를 갖고 온다.
        // 이미지가 잘 보이게 하고 싶으면 이것도 코루틴으로 작성해서 0.1f초마다 띄우는 것도 방법임.
        // 2. 그 이미지를 위치에 setactive On 한다
        // 3. 이미지를 Enumrator로 Lerp로 while로 날려서 도착하면 SetActive false로 한다.
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
        // tree, stone의 위치는 이 함수 작동시키는 곳에서 판별할 것.
        Vector3 startPosition = imageTrs.position;
        float elapsedTime = 0;
        float moveTime = 1.0f;

        // 시간이 moveTime보다 작을 동안 계속해서 움직임
        while (elapsedTime < moveTime)
        {
            // 시간 경과에 따른 보간
            imageTrs.position = Vector3.Lerp(startPosition, targetPos, (elapsedTime / moveTime));

            // 시간 업데이트
            elapsedTime += Time.deltaTime;

            if (elapsedTime > 0.95f)
            {
                imageTrs.gameObject.SetActive(false);
                // 여기서 UIText ++, 플레이어에게 Give Item to Player

                break;
            }
            // 한 프레임 대기
            yield return null;
        }

        GameManager.Instance.GiveMaterialItemToPlayer(name, 1);
        dTexts[name].text = GameManager.Instance.GetPlayerInventory(name).ToString(); // 몇 개인지 받는 함수
    }
}
