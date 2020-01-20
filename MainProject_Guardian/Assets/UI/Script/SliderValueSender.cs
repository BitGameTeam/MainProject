using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderValueSender : MonoBehaviour {

    [SerializeField]
	private GameObject sliderValueTarget;
    public float maxValue = 100;
	public float sliderValue = 0f;

	void Update () {
        switch (sliderValueTarget.tag)
        {
            case "PercentageText":
                ShowValuePercentage();
                break;
            case "NumberText":
                ShowValueNum();
                break;
        }
    }
    void ShowValuePercentage()
    {
        sliderValue = transform.GetComponent<Slider>().value;

        sliderValueTarget.GetComponent<Text>().text = ((int)sliderValue).ToString() + "%";
    }
    void ShowValueNum()
    {
        sliderValue = transform.GetComponent<Slider>().value;

        sliderValueTarget.GetComponent<Text>().text = ((int)sliderValue).ToString(); // + " / " + maxValue.ToString();
    }
}