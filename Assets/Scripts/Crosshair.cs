using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	// Use this for initialization
	void Start () {       
	}
	
	// Update is called once per frame
	void Update () {
        guiText.material.color = Color.white;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                guiText.material.color = Color.red;
            }
        }
	}
}
