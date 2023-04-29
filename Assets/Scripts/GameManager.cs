using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour

{
    public bool alive = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ReloadLevel(){
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Hazzard"){
            Debug.Log("??");
            StartCoroutine(ReloadLevel());
            alive = false;
        }
    }
}
