using UnityEngine;
using System.Collections;

public class ControladorBala : MonoBehaviour
{
    public Transform atirador;
    public Transform alvo;
    private Rigidbody2D rgb2;
    private float chanceDeAcerto = 0.85f;

    // Use this for initialization
    void Start()
    {
        if (alvo != null)
        {

            Vector3 directionOfTravel = alvo.position - transform.position;

            float inclinacao = Vector3.Angle(alvo.position, transform.position);
            if (inclinacao < 17)
            {
                if (atirador.localScale.x < 0)
                {
                    directionOfTravel = Vector3.left;
                }else {
                    directionOfTravel = Vector3.right;
                }

            }
            float angle = Mathf.Atan2(directionOfTravel.y, directionOfTravel.x) * Mathf.Rad2Deg;
            directionOfTravel.Normalize();
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            rgb2 = this.GetComponent<Rigidbody2D>();
            rgb2.AddForce(directionOfTravel * 100);

            float distancia = Vector3.Distance(atirador.position, alvo.position);

            float porcentagemDisc = distancia / 40;

            if (porcentagemDisc > 1)
            {
                chanceDeAcerto = chanceDeAcerto - 0.8f;
            }
            else {

                chanceDeAcerto = chanceDeAcerto - (porcentagemDisc / 2f);

            }
      
        }
        else {
            Destroy(this);
        }
    }

    void FixedUpdate()
    {



    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro != null && outro.tag == "cenario")
        {

            chanceDeAcerto = chanceDeAcerto - 0.5f;
        }

        if (outro != null && atirador !=null && atirador.tag == "soldado" && outro.tag == "cangaceiro")
        {
            if (Random.value <= chanceDeAcerto)
            {
                if (outro != null) {

                    ControlarJogador cangaceiro = outro.gameObject.GetComponent<ControlarJogador>();

                    cangaceiro.Morrer();
                    Destroy(gameObject);
                    Destroy(this);
                }
                    
            }
        }

        if (outro != null &&  atirador != null && atirador.tag == "cangaceiro" && outro.tag == "soldado")
        {
            if (Random.value <= chanceDeAcerto)
            {
                ControlarPolicial soldado = outro.gameObject.GetComponent<ControlarPolicial>();
                soldado.Morrer();
                Destroy(this);
            }
        }

    }
}