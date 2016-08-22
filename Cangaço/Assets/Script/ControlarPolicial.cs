using System.Collections;
using UnityEngine;

public class ControlarPolicial : MonoBehaviour
{
    public GameObject bala;
    private Animator animator;
    private SpriteRenderer spriteSoldado;
    private Vector3 posicaoInicial;
    private Rigidbody2D rgb2d;
    private Transform alvo;
    private bool indoDireita = true;
    private bool indoEsquerda = false;
    private bool indoCima = true;
    private bool indoBaixo = false;
    ArrayList cangaceirosNoRange = new ArrayList();

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteSoldado = GetComponent<SpriteRenderer>();
        rgb2d = GetComponent<Rigidbody2D>();
        animator.SetTrigger("idle");
        posicaoInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        Collider2D[] hitColliders = new Collider2D[4];
        int collisores = Physics2D.OverlapCircleNonAlloc(transform.position, 20f, hitColliders);
        if (collisores >= 1)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].tag == "cangaceiro")
                {
                    if () {
                        

                    }
                    cangaceirosNoRange.Add(hitColliders[i]);
                }
            }
            if (cangaceirosNoRange.Count > 0)
            {

             
                Collider2D cangaceiroMaisPerto = (Collider2D)cangaceirosNoRange[0];

                foreach (Collider2D cangaceiro in cangaceirosNoRange)
                {
                    Debug.Log("angulo: "+(Mathf.Atan2(this.transform.position.y - cangaceiro.transform.position.y,
                          this.transform.position.x - cangaceiro.transform.position.x) * 180 )/ Mathf.PI);

                    if (Vector3.Distance(this.transform.position, cangaceiro.transform.position) <
                        Vector3.Distance(this.transform.position, cangaceiroMaisPerto.transform.position))
                    {
                        
                        cangaceiroMaisPerto = cangaceiro;
                      
                    }
                }
                if (!animator.GetBool("atirar"))
                {
                    alvo = cangaceiroMaisPerto.transform;
                    atirar();
                }
            }
            else {
                andarHorisontal(8);
            }

           
        }else {
            andarHorisontal(8);
        }
        cangaceirosNoRange.Clear();
    }
 
    void andarHorisontal(int distancia)
    {
        if (animator.GetBool("atirar"))
        {
            animator.SetTrigger("guardar");
        }
        else {
            if (Vector3.Distance(posicaoInicial, transform.position) < distancia)
            {
                animator.SetTrigger("andar");
                Vector3 posicaoDestino;

                if (indoDireita)
                {

                    posicaoDestino = new Vector3(posicaoInicial.x + distancia, posicaoInicial.y, posicaoInicial.z);

                }
                else {

                    posicaoDestino = new Vector3(posicaoInicial.x - distancia, posicaoInicial.y, posicaoInicial.z);

                }
                animator.ResetTrigger("apontar");
                animator.ResetTrigger("atirar");
                passoPosicao(posicaoDestino);

            }
            else {
                if (indoEsquerda)
                {
                    indoEsquerda = false;
                    indoDireita = true;
                    Vector3 posicaoDestino = new Vector3(posicaoInicial.x - distancia, posicaoInicial.y, posicaoInicial.z);
                    passoPosicao(posicaoDestino);

                }
                else {
                    Vector3 posicaoDestino = new Vector3(posicaoInicial.x - distancia, posicaoInicial.y, posicaoInicial.z);
                    passoPosicao(posicaoDestino);
                    indoEsquerda = true;
                    indoDireita = false;
                }
            }
        }
    }


    void andarVertical(int distancia)
    {
        if (Vector3.Distance(posicaoInicial, transform.position) < distancia)
        {
            animator.SetTrigger("andar");
            Vector3 posicaoDestino;

            if (indoCima)
            {

                posicaoDestino = new Vector3(posicaoInicial.x, posicaoInicial.y + distancia, posicaoInicial.z);

            }
            else {

                posicaoDestino = new Vector3(posicaoInicial.x, posicaoInicial.y - distancia, posicaoInicial.z);

            }

            passoPosicao(posicaoDestino);

        }
        else {
            if (indoCima)
            {
                indoCima = false;
                indoBaixo = true;
                Vector3 posicaoDestino = new Vector3(posicaoInicial.x, posicaoInicial.y + distancia, posicaoInicial.z);
                passoPosicao(posicaoDestino);


            }
            else {
                Vector3 posicaoDestino = new Vector3(posicaoInicial.x, posicaoInicial.y - distancia, posicaoInicial.z);
                passoPosicao(posicaoDestino);
                indoCima = true;
                indoBaixo = false;
            }

        }

    }

    void atirar()
    { 
        Vector3 directionOfTravel = alvo.transform.position - transform.position;

        if (alvo.transform.position.x < transform.position.x && transform.localScale.x > 0 ||
                alvo.transform.position.x > transform.position.x && transform.localScale.x < 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        animator.ResetTrigger("idle");
        animator.ResetTrigger("andar");
        animator.SetTrigger("apontar");
        animator.SetTrigger("atirar");

    }

    void passoPosicao(Vector3 sentido)
    {

        Vector3 directionOfTravel = sentido - transform.position;
        directionOfTravel.Normalize();
        this.transform.Translate(
            (directionOfTravel.x * 5 * Time.deltaTime),
            (directionOfTravel.y * 5 * Time.deltaTime),
            (directionOfTravel.z * 5 * Time.deltaTime));

        if (sentido.x < transform.position.x && transform.localScale.x > 0 ||
            sentido.x > transform.position.x && transform.localScale.x < 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
  

    }

    public void AdicionarBala()
    {

        GameObject instance = Instantiate(bala, new Vector3(this.transform.position.x + (1.2f * transform.localScale.x), this.transform.position.y + 1.2f, 0f), Quaternion.identity) as GameObject;
        instance.GetComponent<ControladorBala>().atirador = this.transform;
        instance.GetComponent<ControladorBala>().alvo = alvo;
    }

    //dispara quanto tem colisão de trigger
    //outro é o outro objeto que colidiu
    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.tag == "cenario" || outro.tag == "cangaceiro" || outro.tag == "soldado")
        {

            float minOutro = outro.GetComponent<SpriteRenderer>().bounds.min.y;

            //ve se o pé do jogador está mais alto ou mais baixo do que o outro objeto
            if ((spriteSoldado.bounds.min.y) < (minOutro))
            {
                if (spriteSoldado.sortingOrder < outro.GetComponent<SpriteRenderer>().sortingOrder)
                {
                    spriteSoldado.sortingOrder = outro.GetComponent<SpriteRenderer>().sortingOrder + 1;
                }
            }
            else {

                spriteSoldado.sortingOrder = outro.GetComponent<SpriteRenderer>().sortingOrder - 1;
            }

        }
    }

    //uma cópia do de cima mas esse aqui atualiza a cada frame que o personagem entra na area de colisão
    void OnTriggerStay2D(Collider2D outro)
    {

        if (outro.tag == "cenario" || outro.tag == "cangaceiro" || outro.tag == "soldado")  
        {

            float minOutro = outro.GetComponent<SpriteRenderer>().bounds.min.y;

            //ve se o pé do jogador está mais alto ou mais baixo do que o outro objeto
            if ((spriteSoldado.bounds.min.y) < (minOutro))
            {
                if(spriteSoldado.sortingOrder < outro.GetComponent<SpriteRenderer>().sortingOrder)
                {
                    spriteSoldado.sortingOrder = outro.GetComponent<SpriteRenderer>().sortingOrder + 1;
                }
            }
            else {

                spriteSoldado.sortingOrder = outro.GetComponent<SpriteRenderer>().sortingOrder - 1;
            }

        }
    }


    public void TriggerAtirar()
    {
        animator.SetTrigger("atirar");
        animator.SetTrigger("apontar");
    }

    public void TriggerAndar()
    {
        if (animator.GetBool("guardar"))
        {
            animator.SetTrigger("andar");
            animator.ResetTrigger("apontar");
            animator.ResetTrigger("atirar");
            animator.ResetTrigger("guardar");

        }

    }

    public void Morrer()
    {

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("soldadoMorrer"))
        {
            animator.SetTrigger("morrer");
        }
   
 
    }

    public void Destruir()
    {
        Destroy(gameObject);

    }

}
