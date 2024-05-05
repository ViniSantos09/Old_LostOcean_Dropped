using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controlePersonagem;
    private Animator animacao;

    public float velocidade = 3;
    public float gravidade = -9.8f;

    public new Transform camera;

    // Start is called before the first frame update
    void Start()
    {
        controlePersonagem = GetComponent<CharacterController>();
        // Criando um vari�vel para guardar o controle do personagem
        // Para podermos manipula-l� posteriormente

        animacao = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // GetComponent<CharacterController>().Move(...)
        // ------- controlePersonagem.Move(transform.forward * velocidade * Time.deltaTime); --------
        // aqui n�s usamos o (Move) que � um dos "Metodos" que o characterController disponibilizar

        /* ==================== transform.forward =====================
        - Essa parte do c�digo faz o personagem ir para frente.
        - Transform -> � um componente da Unity que cont�m a posi��o, rota��o e escala do objeto(personagem)
        - .forward -> � um vetor que aponta o movimento para frente do obejto(personagem)
        */

        /* ============ 3 / velocidade ==============
        - comprimento do vetor = vetor * velocidade; 
        Ex: transform. forward * 3 = (3, 0, 0) 
        - N�s indicamos o qu�o r�pido ele vai se mover para frente.
        */

        /* ==================== Time.deltaTime =====================
        - � um fator que faz o movimento do personagem ser mais suave.
        - Pois ele faz com que o movimento n�o seja afetado pelo aumento e decaimento da taxa de quadros(FPS).
        - Como muitas telas rodando em (FPS) diferentes, o .deltaTime arruma essa varia��o de frames.
        
        EXEMPLO: 
        - Imagine que voc� est� em um jogo de plataforma, e seu personagem precisa se mover para a direita a uma velocidade constante.

        Sem considerar o Time.deltaTime, a movimenta��o poderia parecer assim:

        No primeiro quadro, o personagem se move 3 unidades para a direita.
        No segundo quadro, o personagem se move mais 3 unidades para a direita.

        Se a taxa de quadros fosse constante, isso n�o seria um problema. No entanto, se houver uma varia��o na taxa de quadros:

        Se o segundo quadro demorar o dobro do tempo para ser renderizado, o personagem ter� se movido 6 unidades no total, em vez das 3 unidades desejadas.
        
        - Agora, vamos adicionar o Time.deltaTime para corrigir isso:

        Digamos que Time.deltaTime neste segundo quadro seja 0.02 segundos (ou seja, 20 milissegundos).
        Em vez de mover o personagem 3 unidades diretamente, multiplicamos a velocidade pelo Time.deltaTime, assim:
        Velocidade * Tempo = 3 unidades/segundo * 0.02 segundos = 0.06 unidades.
        Ent�o, movemos o personagem 0.06 unidades para a direita neste quadro.
        */

        /* ============================= COMO ELE FUNCIONA ==================================
        Basicamente, o personagem vai se mover 3 unidades por segundo.

        - Com o Time.deltaTime, o jogador que est� jogando a 60 FPS ver� o personagem se mover a 3 unid p/ seg.
        e o jogador que est� a 30 FPS ver� o personagem se mover a 3 unid p/ seg.
        */

        // -------------------- float hor = Input.GetAxis("Horizontal"); ------------------------
        // -------------------- float ver = Input.GetAxis("Vertical"); -----------------------------

        /* ======================== DIFEREN�A ENTRE (Input.GetAxis()) e (Input.GetAxis()) ====================
        
        - .GetAxis (-1, 1) -> vai fazer o movimento ser mais suave quando o personagem come�ar a se mover.
        - .GetAxisRaw (0, 1) -> vai fazer o movimento ser mais "Cru" ou Bruto.

        EX: quando o jogador clicar para ir para a direita:
        No .GetAxis, o personagem vai come�ar a correr devagar e vai ir acelerando com o tempo.
        No .GetAxisRaw, o personagem vai come�ar a correr logo que o jogador clicar.
        */

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 movimento = Vector3.zero;
        float speedmove = 0;

        if (hor != 0 || ver != 0)
        {
            Vector3 frente = camera.forward;
            frente.y = 0;  // eixo y = 0; para o jogar n�o voar
            frente.Normalize();

            Vector3 direita = camera.right;
            direita.y = 0;
            direita.Normalize();

            Vector3 direcao = frente * ver + direita * hor;
            speedmove = Mathf.Clamp01(direcao.magnitude);
            direcao.Normalize();

            movimento = direcao * velocidade * speedmove * Time.deltaTime;
            // Multiplicamos pelo 'deltaTime' para que a gravidade seja aplicada de forma suave 

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direcao), 0.2f);
            // O Quaternion.Slerp() � usado para fazer uma interpola��o entre rota��es, para que a mudan�a entre elas seja mais suave
            // o (0.2f) determina o quando uma transi��o deve ir para mudar para a outra
        }

        movimento.y += gravidade * Time.deltaTime;

        controlePersonagem.Move(movimento);

        /* ========================= .Normalize() =============================== 
         .Normalize() -> � usado quando voc� quer apenas a dire��o daquele vetor, ignorando a sua "Magnitude"
        ou for�a, fazendo assim o vetor ser igual a 1 e mantendo a dire��o do vetor.

         .Magnitude() -> � usado para indicar a magnitude daquele vetor (sua dist�ncia ou for�a) e ignora a 
        dire��o do vetor. 

        Ex: x = -3 e y = 4, tem magnitude(dist�ncia) de 5.
        */

        animacao.SetFloat("YSpeed", speedmove);
        animacao.SetFloat("XSpeed", speedmove);
    }
}
