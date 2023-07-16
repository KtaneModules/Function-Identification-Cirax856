using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionIdentification : MonoBehaviour {
#pragma warning disable CS0108
	public KMAudio audio;
#pragma warning restore CS0108

	public KMSelectable functionButton;
	public KMSelectable rightButton;
	public KMSelectable leftButton;

	public GameObject screen;
	public GameObject label;

	public Material defaultMaterial;

	public Material[] functions;
	public string functionName;
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
		functionName = chosenFunction.name;
	}

	private bool Function() {
		if(functionName.Contains(label.GetComponent<TextMesh>().text)) {
			Debug.LogFormat("[Function Identification #{0}] Successfully disarmed Function Identification!", moduleId);
			audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, functionButton.transform);
			ClearDisplay();
			GetComponent<KMNeedyModule>().OnPass();
		} else {
			GetComponent<KMNeedyModule>().OnStrike();
			audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, functionButton.transform);
			Debug.LogFormat("[Function Identification #{0}] Incorrect! Pressed: {1}, expected {2}", moduleId, label.GetComponent<TextMesh>().text, functionName);
		}

		return false;
	}

	private string RemoveNumber() {
		functionName = functionName.Remove(functionName.Length - 1);

		if(functionName[functionName.Length - 1] == '1') {
			functionName = functionName.Remove(functionName.Length - 1);
		}

		return functionName;
	}

	private bool Right() {
		index += 1;
		if(index > 7) {
			index = 0;
		}

		label.GetComponent<TextMesh>().text = functionTypes[index];

		audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, rightButton.transform);

		return false;
	}

	private bool Left() {
		index -= 1;
		if(index < 0) {
			index = 7;
		}

		label.GetComponent<TextMesh>().text = functionTypes[index];

		audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, leftButton.transform);

		return false;
	}

	private void ClearDisplay() {
		screen.GetComponent<MeshRenderer>().material = defaultMaterial;
	}

	private void GenerateFunction() {
		chosenFunction = functions[Random.Range(0, functions.Length)];
		functionName = chosenFunction.name;

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

#pragma warning disable CS0414
	private readonly string TwitchHelpMessage = "Cycle left or right using the commands \"!{0} left\" and \"${0} right\". Submit your answer with \"!${0} submit\"";
#pragma warning restore CS0414

	private IEnumerator ProcessTwitchCommand(string command) {
		command = command.ToLowerInvariant();

		if(command.Equals("right")) {
			yield return null;
			Right();
		} else if(command.Equals("left")) {
			yield return null;
			Left();
		} else if(command.Equals("submit")) {
			yield return null;
			Function();
		}
	}
}
