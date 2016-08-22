using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ControladorCamera : MonoBehaviour
{
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public List<Transform> cangaceiros = new List<Transform>();
    
    // Use this for initialization

    void Start()
    {
      

    }

    // Update is called once per frame
    void Update()
    {

        Vector3 center = new Vector3(0, 0, 0);
        for (int i = 0; i < cangaceiros.Count; i++)
        {
            if (cangaceiros[i] != null)
            {
                center += cangaceiros[i].position;
            }
            else {
                cangaceiros.RemoveAt(i);
            }

        }

        Vector3 theCenter = Vector3.zero ;

        if (cangaceiros.Count == 0)
        {

            
            theCenter = transform.position;
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);


        }
        else {
            
            theCenter = center / cangaceiros.Count;
        }

      
  
        if (theCenter != null)
        {

            Vector3 point = Camera.main.WorldToViewportPoint(theCenter);
            Vector3 delta = theCenter - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            if (theCenter.x < -1 || theCenter.x > 80)
            {
                delta.x = 0;
            }

            if (theCenter.y < -6 || theCenter.y > 5)  {
                delta.y = 0;
            }

            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }

    }

   
}

