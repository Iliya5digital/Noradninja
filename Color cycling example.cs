using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script expects a stadard color eg. Color or Albedo in the shader. Use Particles/Stadard Unlit for best results*/

public class General_Blinker : MonoBehaviour {

	public Color dimColor;
	public Color midColor;
	public Color brightColor;
	public Color blinkerColor;
	public Renderer objectRenderer;
	public float repeatRate;
	public bool blinkerOn;

	void OnEnable () {
		objectRenderer = this.GetComponent<Renderer>(); //get renderer of object we are blinking
	}
	void OnDisable () {
		blinkerOn = false; //disable blinking when object is enabled so we are only calling the routine loop once
	}
	
	// Update is called once per frame
	void Update () {
		//only call this if blinkerOn is disabled, so we only run Blinker() once
		if(!blinkerOn){
			Blinker(dimColor, midColor, repeatRate);
		}
	}

	void Blinker(Color startColor, Color endColor, float duration){
		blinkerOn = true;
		StartCoroutine(colorAnimator(startColor, endColor, (repeatRate/4)));
	}

	IEnumerator colorAnimator (Color startColor, Color endColor, float duration){
		float time = 0;
		while (time < duration){	
				blinkerColor = Color.Lerp(startColor, endColor, time/duration); //lerp the colors from dark to mid
				objectRenderer.material.SetColor("_Color", blinkerColor); //get the color of the material and set it to our current lerped value
				time += Time.deltaTime;
				yield return null;
			}
		StopCoroutine("colorAnimator");
		blinkerColor = endColor; //make sure we have hit the target color so if/else block doesn't fail
		
		if (blinkerColor == midColor){
			
			StartCoroutine(colorAnimator(midColor, brightColor, repeatRate)); //call the routine again to lerp the color back from mid to bright
		}
		else if(blinkerColor == brightColor){
			StartCoroutine(colorAnimator(brightColor, dimColor, repeatRate)); //call the routine again to lerp the color back from bright to dark
		}
		else if(blinkerColor == dimColor){
			Blinker(dimColor, midColor, repeatRate); //if we are back at dark, rerun Blinker() to start the cycle again
		}
	}
}