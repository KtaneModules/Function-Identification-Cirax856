using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionIdentification : MonoBehaviour {
	public KMSelectable functionButton;
	public KMSelectable rightButton;
	public KMSelectable leftButton;

	public GameObject screen;
	public GameObject label;

	public Material defaultMaterial;

	public Material[] functions;
	public string[] functionTypes = {"linear", "constant", "reciprocal", "quadratic", "cubic", "goniometric", "exponential", "logarithmic"};
	public int index = 0;

	public Material chosenFunction;

	void Awake() {
		GetComponent<KMNeedyModule>().OnNeedyActivation += OnNeedyActivation;
        GetComponent<KMNeedyModule>().OnNeedyDeactivation += OnNeedyDeactivation;
        functionButton.OnInteract += Function;
		rightButton.OnInteract += Right;
		leftButton.OnInteract += Left;
        GetComponent<KMNeedyModule>().OnTimerExpired += OnTimerExpired;
	}

	private bool Function() {
		if(chosenFunction.name.Contains(label.GetComponent<TextMesh>().text)) {
			GetComponent<KMNeedyModule>().OnPass();
			screen.GetComponent<MeshRenderer>().material = defaultMaterial;	
		} else {
			GetComponent<KMNeedyModule>().OnStrike();
		}

		return false;
	}

	private bool Right() {
		index += 1;
		if(index > 7) {
			index = 0;
		}

		label.GetComponent<TextMesh>().text = functionTypes[index];

		return false;
	}

	private bool Left() {
		index -= 1;
		if(index < 0) {
			index = 7;
		}

		label.GetComponent<TextMesh>().text = functionTypes[index];

		return false;
	}

	private void GenerateFunction() {
		chosenFunction = functions[Random.Range(0, functions.Length)];

		screen.GetComponent<MeshRenderer>().material = chosenFunction;
		label.GetComponent<TextMesh>().text = functionTypes[index];
	}

	// default functions
    protected void OnNeedyActivation() {
		GenerateFunction();
	}

    protected void OnNeedyDeactivation() {
		screen.GetComponent<MeshRenderer>().material = defaultMaterial;
	}

    protected void OnTimerExpired() {
        GetComponent<KMNeedyModule>().OnStrike();
    }
}
