using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderValueReciver : MonoBehaviour {

	public GameObject SliderController;

	public float SliderValue = 0f;

	void Start ()
    {
	
	}
	
	void Update ()
    {

		transform.GetComponent<Slider> ().value = SliderValue;
	}
}