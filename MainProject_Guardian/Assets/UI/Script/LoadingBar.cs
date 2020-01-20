using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//던전 생성 로딩바이며 씬 로딩 바와는 관계없음
public class LoadingBar : MonoBehaviour
{

    [SerializeField]
    private GameObject sliderValueTarget;
    public float maxValue = 100;
    public float sliderValue = 0f;
    float accelateTime = 0f;

    [SerializeField]
    GameObject loadingUI;

    [SerializeField]
    Transform dungeonEntrancePos;
    [SerializeField]
    Transform playerTransform;

    [SerializeField]
    Text loadingKText;

    float fsliderV = 0f;
    float ssliderV = 0f;
    float tsliderV = 0f;
    float valueSpacing = 10f;

    private void Start()
    {
        transform.GetComponent<Slider>().value = sliderValue;
        fsliderV = sliderValue + valueSpacing;
        ssliderV = fsliderV + valueSpacing;
        tsliderV = ssliderV + valueSpacing;


    }

    void FixedUpdate()
    {
        UpdateLoadingValue();
        ShowValuePercentage();
        ShowLoadingText();
    }

    void UpdateLoadingValue()
    {
        if (sliderValue < 100)
        {
            accelateTime = sliderValue*0.1f;
            sliderValue += Time.deltaTime + accelateTime;
        }
        if(sliderValue >= 20f)
        {
            playerTransform.position = new Vector3(dungeonEntrancePos.position.x, dungeonEntrancePos.position.y, dungeonEntrancePos.position.z-1f);
        }
        if (sliderValue > 100f)
        {
            sliderValue = 100f;
            Destroy(loadingUI);
        }
    }
    void ShowLoadingText()
    {
        
        if (sliderValue < tsliderV)
        {
            loadingKText.text = "던전 생성 중...";
            if (sliderValue < ssliderV)
            {
                loadingKText.text = "던전 생성 중..";
                if (sliderValue < fsliderV)
                {
                    loadingKText.text = "던전 생성 중.";
                }
            }
        }
        if(sliderValue > tsliderV)
        {
            valueSpacing += valueSpacing;
            fsliderV = sliderValue + valueSpacing;
            ssliderV = fsliderV + valueSpacing;
            tsliderV = ssliderV + valueSpacing;
        }
    }
    void ShowValuePercentage()
    {
        transform.GetComponent<Slider>().value = sliderValue;

        sliderValueTarget.GetComponent<Text>().text = ((int)sliderValue).ToString() + "%";
    }
}