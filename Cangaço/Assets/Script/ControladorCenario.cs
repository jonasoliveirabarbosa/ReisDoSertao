using UnityEngine;
using System.Collections;

public class ControladorCenario : MonoBehaviour {

    public GameObject chao;
    
    void Start () {
        float high = chao.GetComponent<SpriteRenderer>().bounds.size.x;
        float whidth = chao.GetComponent<SpriteRenderer>().bounds.size.y;

        for (int x = -10; x < 50; x++)
        {
            for (int y = -10; y < 10; y++)
            {
                GameObject instance = Instantiate(chao, new Vector3(x * high, y * whidth, 0f), Quaternion.identity) as GameObject;
               
            }
        }
    }
}
