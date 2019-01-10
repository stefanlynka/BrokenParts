using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraPanner : MonoBehaviour {

    public Vector3 target;
    public GameObject OffText1;
    public GameObject OffText2;
    public GameObject OffText3;

    public GameObject OnText1;
    public GameObject OnText2;
    public GameObject OnText3;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void TextChange() {
        if (OffText1) OffText1.gameObject.SetActive(false);
        if (OffText2) OffText2.gameObject.SetActive(false);
        if (OffText3) OffText3.gameObject.SetActive(false);

        if ((OnText1) && (!OnText1.gameObject.activeInHierarchy)) StartCoroutine(Example());
    

    }

    IEnumerator Example() {
        print(Time.time);
        yield return new WaitForSeconds(5);
        if (OnText1) OnText1.gameObject.SetActive(true);
        if (OnText2) OnText2.gameObject.SetActive(true);
        if (OnText3) OnText3.gameObject.SetActive(true);
        print(Time.time);
    }

}
