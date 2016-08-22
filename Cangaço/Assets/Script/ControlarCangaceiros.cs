using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class ControlarCangaceiros : MonoBehaviour
{


    public List<Transform> cangaceiros = new List<Transform>();
    bool hasDrag = false;
    bool click = true;
    float temps =0;
    private List<Vector2> pontos = new List<Vector2>();
   
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            temps = Time.time;
            click = true;
            hasDrag = false;
        }

        if (click && !hasDrag && (Time.time - temps) > 0.2 )
        {
            // long click effect
            hasDrag = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            click = false;
            if ((Time.time - temps) < 0.2)
            {
                hasDrag = false;
            }
        }

        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0)) {
            ControlaDrag();
        }


        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || (Input.GetMouseButtonUp(0) && !hasDrag))
        {

            Vector3 pos = new Vector3();
            if (Input.touchCount > 0)
            {

                 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else {

                 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            }
               
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.transform != null && hit.collider != null && hit.transform.tag == "soldado")
            {

                for (int i = 0; i < cangaceiros.Count; i++)
                {
                    if (cangaceiros[i] != null)
                    {
                        cangaceiros[i].GetComponent<ControlarJogador>().atirar(hit.transform);
                        
                    }
                }

            }
            else
            {


                Vector3 center = new Vector3(0, 0, 0);
                int count = 0;

                for (int i = 0; i < cangaceiros.Count; i++)
                {
                    if (cangaceiros[i] != null)
                    {
                        center += cangaceiros[i].position;
                        count++;
                    }


                }
                Vector3 theCenter = center / count;

                for (int i = 0; i < cangaceiros.Count; i++)
                {
                    if (cangaceiros[i] != null)
                    {
                        
                        if(cangaceiros[i].GetComponent<ControlarJogador>() != null)
                        {
                            Vector3 cent = cangaceiros[i].position - theCenter;
                            cangaceiros[i].GetComponent<ControlarJogador>().posicaoDestino = (Camera.main.ScreenToWorldPoint(Input.mousePosition) + cent);
                            cangaceiros[i].GetComponent<ControlarJogador>().posicaoDestino.z = cangaceiros[i].position.z;
                        }
                       
                    }
                }
            }

            if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && hasDrag) || (Input.GetMouseButtonUp(0) && hasDrag))
            {
                MoverDrag();
            }
        }
    }

    void ControlaDrag()
    {
        hasDrag = true;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pontos.Add(mousePos);
    }
    void MoverDrag()
    {

        if (hasDrag) {
            int count = 0;
            for (int i = 0; i < cangaceiros.Count; i++)
            {
                if (cangaceiros[i] != null)
                { 

                    count++;

                }
              
            }

            int periodo = pontos.Count / cangaceiros.Count;
            int mult = 0;

            for (int i = 0; i < cangaceiros.Count; i++)
            {
                if (cangaceiros[i] != null)
                {

                    cangaceiros[i].GetComponent<ControlarJogador>().posicaoDestino = pontos[periodo * mult];
                    cangaceiros[i].GetComponent<ControlarJogador>().posicaoDestino.z = cangaceiros[i].position.z;
                    mult++;

                }

            }

            pontos.Clear();
            hasDrag = false;
        }

    }

}