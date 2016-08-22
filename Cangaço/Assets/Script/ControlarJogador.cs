using UnityEngine;
using System.Collections;

public class ControlarJogador : MonoBehaviour
{

    public GameObject bala;
    private Animator animator;
    private SpriteRenderer spriteJogador;
    private Transform alvo;
    public Vector3 posicaoDestino;
    private Rigidbody2D rgb2d;
    private bool taColidindo = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteJogador = GetComponent<SpriteRenderer>();
        rgb2d = GetComponent<Rigidbody2D>();
        animator.SetTrigger("idle");
        posicaoDestino = transform.position;
    }

    void FixedUpdate()
    {
        this.Andar(Vector3.zero);  

        if (animator.GetBool("apontar") == false && animator.GetBool("atirar") == false)
        {

            AjustarDirecao(posicaoDestino);
  
        }

        if (animator.GetBool("andar") == false && animator.GetBool("apontar") == false && animator.GetBool("atirar") == false)
        {

            animator.SetTrigger("idle");

        }

        if (Input.GetKeyDown("space"))
        {

            animator.ResetTrigger("idle");
            animator.SetTrigger("apontar");
            animator.SetTrigger("atirar");

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        taColidindo = false;
    }

    //dispara quanto tem cosisão	
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "cangaceiro")
        {

            Vector3 directionOfTravel = new Vector3();
            if (posicaoDestino.x <= collision.gameObject.GetComponent<Renderer>().bounds.center.x)
            {

                directionOfTravel.x = directionOfTravel.x - 1;

            }

            if (posicaoDestino.x > collision.gameObject.GetComponent<Renderer>().bounds.center.x)
            {

                directionOfTravel.x = directionOfTravel.x + 1;

            }

            if (posicaoDestino.y <= collision.gameObject.GetComponent<Renderer>().bounds.center.y)
            {

                directionOfTravel.y = directionOfTravel.y - 1;

            }

            if (posicaoDestino.y > collision.gameObject.GetComponent<Renderer>().bounds.center.y)
            {

                directionOfTravel.y = directionOfTravel.y + 1;

            }


            directionOfTravel.Normalize();
            Andar(directionOfTravel);

        }

    }

    void OnCollisionExit2D(Collision2D collision)
    {
        taColidindo = false;
    }
    //dispara quanto tem colisão de trigger
    //outro é o outro objeto que colidiu
    void OnTriggerEnter2D(Collider2D outro)
    {
   
    }

    //uma cópia do de cima mas esse aqui atualiza a cada frame que o personagem entra na area de colisão
    void OnTriggerStay2D(Collider2D outro)
    {

        if (outro.tag == "cenario" || outro.tag == "cangaceiro" || outro.tag == "soldado")
        {

            float minOutro = outro.GetComponent<SpriteRenderer>().bounds.min.y;

            //ve se o pé do jogador está mais alto ou mais baixo do que o outro objeto
            if ((spriteJogador.bounds.min.y) < (minOutro))
            {
                if (spriteJogador.sortingOrder < outro.GetComponent<SpriteRenderer>().sortingOrder)
                {
                    spriteJogador.sortingOrder = outro.GetComponent<SpriteRenderer>().sortingOrder + 1;
                }
            }
            else {

                spriteJogador.sortingOrder = outro.GetComponent<SpriteRenderer>().sortingOrder - 1;
            }

        }
    }

    public void Andar(Vector3 desvio)
    {
        Vector3 pos = new Vector3();
        RaycastHit2D hit;

        if (Input.GetMouseButtonUp(0))
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit.transform != null && hit.collider != null && hit.transform.tag == "soldado")
            {
                alvo = hit.transform;
                posicaoDestino = transform.position;
                animator.ResetTrigger("idle");
                animator.ResetTrigger("andar");
                animator.SetTrigger("apontar");
                animator.SetTrigger("atirar");

            }

        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {

            pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            hit = Physics2D.Raycast(pos, Vector2.zero);

        if (hit.transform != null && hit.collider != null && hit.transform.tag == "soldado")
        {
            alvo = hit.transform;
            posicaoDestino = transform.position;
            animator.ResetTrigger("idle");
            animator.ResetTrigger("andar");
            animator.SetTrigger("apontar");
            animator.SetTrigger("atirar");

        }
       } 


        if ((Vector3.Distance(posicaoDestino, transform.position) > 0.1F) &&
         (!animator.GetCurrentAnimatorStateInfo(0).IsName("Morrer")
         && !animator.GetCurrentAnimatorStateInfo(0).IsName("apontar")
         ) && !taColidindo)
        {

            Vector3 directionOfTravel = (posicaoDestino - transform.position) + desvio;
            directionOfTravel.Normalize();
            this.transform.Translate(
                (directionOfTravel.x * 5 * Time.deltaTime),
                (directionOfTravel.y * 5 * Time.deltaTime),
                (directionOfTravel.z * 5 * Time.deltaTime));
            animator.ResetTrigger("apontar");
            animator.ResetTrigger("atirar");
            animator.SetTrigger("andar");

        }
        else {

            animator.ResetTrigger("andar");

        }

    }

    public void AjustarDirecao(Vector2 pos)
    {

        if ((pos.x < transform.position.x && transform.localScale.x > 0 ||
             pos.x > transform.position.x && transform.localScale.x < 0)
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Morrer"))
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

    public void Morrer()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Morrer"))
        {
            animator.SetTrigger("morrer");
        }


    }

    public void Destruir()
    {

        Destroy(gameObject);
        Destroy(this);

    }

    public void atirar(Transform alvo2)
    {

        alvo = alvo2;
        posicaoDestino = transform.position;
        animator.ResetTrigger("idle");
        animator.ResetTrigger("andar");
        animator.SetTrigger("apontar");
        animator.SetTrigger("atirar");

        this.AjustarDirecao(alvo2.position);

    }

}