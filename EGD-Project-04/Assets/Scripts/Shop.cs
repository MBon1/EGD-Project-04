using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// hi matt if you're seeing this. this script is mainly for aesthetics right now
public class Shop : MonoBehaviour
{
    [SerializeField] List<GameObject> row1, row2, row3, col1, col2, col3;
    private Button selfButton;
    private bool hidden;
    private float moveAmt = 30f;
    // Start is called before the first frame update
    void Start()
    {
        hidden = true;
        foreach (GameObject go in row1) go.SetActive(false);
        foreach (GameObject go in row2) go.SetActive(false);
        foreach (GameObject go in row3) go.SetActive(false);
        foreach (GameObject go in col1) go.SetActive(false);
        foreach (GameObject go in col2) go.SetActive(false);
        foreach (GameObject go in col3) go.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        selfButton = GetComponent<Button>();
    }
    public void Click()
    {
        if (hidden) StartCoroutine(Reveal());
        else StartCoroutine(Hide());
    }

    IEnumerator Reveal()
    {
        selfButton.interactable = false;
        hidden = false;
        for(int i = 0; i < row1.Count; i++) { row1[i].SetActive(true); row1[i].GetComponent<Button>().interactable = false; }
        for(int i = 0; i < row2.Count; i++) { row2[i].SetActive(true); row3[i].GetComponent<Button>().interactable = false; }
        for(int i = 0; i < row3.Count; i++) { row3[i].SetActive(true); row3[i].GetComponent<Button>().interactable = false; }
        for(int i = 0; i < col1.Count; i++) { col1[i].SetActive(true); col1[i].GetComponent<Button>().interactable = false; }
        for(int i = 0; i < col2.Count; i++) { col2[i].SetActive(true); col2[i].GetComponent<Button>().interactable = false; }
        for(int i = 0; i < col3.Count; i++) { col3[i].SetActive(true); col3[i].GetComponent<Button>().interactable = false; }

        for (float i = 0; i < 0.2f; i+=0.02f)
        {
            float percent = i / 0.2f;
            for(int j = 0; j < row1.Count; j++) row1[j].transform.position -= new Vector3(0, percent * moveAmt * 1f, 0);
            for(int j = 0; j < row2.Count; j++) row2[j].transform.position -= new Vector3(0, percent * moveAmt * 2f, 0);
            for(int j = 0; j < row3.Count; j++) row3[j].transform.position -= new Vector3(0, percent * moveAmt * 3f, 0);
            yield return new WaitForSeconds(0.02f);
        }
        for (float i = 0; i < 0.2f; i += 0.02f)
        {
            float percent = i / 0.2f;
            for(int j = 0; j < col2.Count; j++) col2[j].transform.position += new Vector3(percent * moveAmt * 1f, 0, 0);
            for(int j = 0; j < col3.Count; j++) col3[j].transform.position += new Vector3(percent * moveAmt * 2f, 0, 0);
            yield return new WaitForSeconds(0.02f);
        }

        foreach (GameObject go in row1) go.GetComponent<Button>().interactable = true;
        foreach (GameObject go in row2) go.GetComponent<Button>().interactable = true;
        foreach (GameObject go in row3) go.GetComponent<Button>().interactable = true;
        foreach (GameObject go in col1) go.GetComponent<Button>().interactable = true;
        foreach (GameObject go in col2) go.GetComponent<Button>().interactable = true;
        foreach (GameObject go in col3) go.GetComponent<Button>().interactable = true;
        selfButton.interactable = true;
        yield return null;
    }
    IEnumerator Hide()
    {
        hidden = true;
        selfButton.interactable = false;

        for (float i = 0; i < 0.2f; i += 0.02f)
        {
            float percent = i / 0.2f;
            for(int j = 0; j < col2.Count; j++) col2[j].transform.position -= new Vector3(percent * moveAmt * 1f, 0, 0);
            for(int j = 0; j < col3.Count; j++) col3[j].transform.position -= new Vector3(percent * moveAmt * 2f, 0, 0);
            yield return new WaitForSeconds(0.02f);
        }
        for (float i = 0; i < 0.2f; i += 0.02f)
        {
            float percent = i / 0.2f;
            for(int j = 0; j < row1.Count; j++) row1[j].transform.position += new Vector3(0, percent * moveAmt * 1f, 0);
            for(int j = 0; j < row2.Count; j++) row2[j].transform.position += new Vector3(0, percent * moveAmt * 2f, 0);
            for(int j = 0; j < row3.Count; j++) row3[j].transform.position += new Vector3(0, percent * moveAmt * 3f, 0);
            yield return new WaitForSeconds(0.02f);
        }

        for(int i = 0; i < row1.Count; i++) row1[i].SetActive(false);
        for(int i = 0; i < row2.Count; i++) row2[i].SetActive(false);
        for(int i = 0; i < row3.Count; i++) row3[i].SetActive(false);
        for(int i = 0; i < col1.Count; i++) col1[i].SetActive(false);
        for(int i = 0; i < col2.Count; i++) col2[i].SetActive(false);
        for(int i = 0; i < col3.Count; i++) col3[i].SetActive(false);
        selfButton.interactable = true;
        yield return null;
    }
}
