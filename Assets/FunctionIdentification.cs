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

	// logging
	static int moduleIdCounter = 1;
	int moduleId;

	void Awake() {
		moduleId = moduleIdCounter++;

		GetComponent<KMNeedyModule>().OnNeedyActivation += OnNeedyActivation;
        GetComponent<KMNeedyModule>().OnNeedyDeactivation += OnNeedyDeactivation;
        functionButton.OnInteract += delegate() { Function(); return false; };
		rightButton.OnInteract += Right;
		leftButton.OnInteract += Left;
        GetComponent<KMNeedyModule>().OnTimerExpired += OnTimerExpired;

		chosenFunction = functions[Random.Range(0, functions.Length)];
	}

	private bool Function() {
		if(chosenFunction.name.Contains(label.GetComponent<TextMesh>().text)) {
			Debug.LogFormat("[Function Identification #{0}] Successfully disarmed Function Identification!", moduleId);
			ClearDisplay();
			GetComponent<KMNeedyModule>().OnPass();
		} else {
			GetComponent<KMNeedyModule>().OnStrike();
			Debug.LogFormat("[Function Identification #{0}] Incorrect! Pressed: {1}, expected {2}", moduleId, label.GetComponent<TextMesh>().text, RemoveNumber());
		}

		return false;
	}

	private string RemoveNumber() {
		chosenFunction.name.Remove(chosenFunction.name.Length - 1);

		if(chosenFunction.name[chosenFunction.name.Length] == 1) {
			chosenFunction.name.Remove(chosenFunction.name.Length - 1);
		}

		return chosenFunction.name;
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

	private void ClearDisplay() {
		screen.GetComponent<MeshRenderer>().material = defaultMaterial;
	}

	private void GenerateFunction() {
		chosenFunction = functions[Random.Range(0, functions.Length)];

		screen.GetComponent<MeshRenderer>().material = chosenFunction;
		label.GetComponent<TextMesh>().text = functionTypes[index];

		Debug.LogFormat("[Function Identification #{0}] Generated function type: {1}", moduleId, RemoveNumber());
	}

	// default functions
    protected void OnNeedyActivation() {
		GenerateFunction();
	}

    protected void OnNeedyDeactivation() {
		ClearDisplay();
	}

    protected void OnTimerExpired() {
        GetComponent<KMNeedyModule>().OnStrike();
		ClearDisplay();
    }
}
