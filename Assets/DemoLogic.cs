using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoLogic : MonoBehaviour
{
    public GameObject package;
    public GameObject parachute;
    public float deploymentHeight = 7.5f;
    public float parachuteDrag = 5f;
    public float landingHeight = 1f;
    public float chuteOpenDuration = 0.5f;

    private float originalDrag;
    
    // Start is called before the first frame update
    void Start()
    {
        originalDrag = package.GetComponent<Rigidbody>().drag;
        //parachute.SetActive(false);
        StartCoroutine(ExpandParachute());
    }

    IEnumerator ExpandParachute()
    {
        // animate the parachute
        parachute.transform.localScale = Vector3.zero;
        float timeElapsed = 0f;
        
        while (timeElapsed < chuteOpenDuration)
        {
            float newScale = timeElapsed / chuteOpenDuration;
            parachute.transform.localScale = new Vector3(newScale, newScale, newScale);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        parachute.transform.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(package.transform.position, Vector3.down, out var hitInfo, deploymentHeight)) // checks if true
        {
            package.GetComponent<Rigidbody>().drag = parachuteDrag;
            parachute.SetActive(true);
            Debug.DrawRay(package.transform.position, Vector3.down * deploymentHeight, Color.red);
            Debug.Log(hitInfo.distance);
            
            if (hitInfo.distance <= landingHeight) // turn off parachute when hitting ground
            {
                Debug.Log("landed");
                parachute.SetActive(false);
            }
        }
        else
        {
            package.GetComponent<Rigidbody>().drag = originalDrag;
            parachute.SetActive(false);
            Debug.DrawRay(package.transform.position, Vector3.down * deploymentHeight, Color.cyan);
        }
        
        // Debug.Break(); pauses unity editor
    }
    
    
}
