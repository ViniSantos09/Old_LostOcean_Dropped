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
        // Criando um variével para guardar o controle do personagem
        // Para podermos manipula-lá posteriormente

        animacao = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // GetComponent<CharacterController>().Move(...)
        // ------- controlePersonagem.Move(transform.forward * velocidade * Time.deltaTime); --------
        // aqui nós usamos o (Move) que é um dos "Metodos" que o characterController disponibilizar

        /* ==================== transform.forward =====================
        - Essa parte do código faz o personagem ir para frente.
        - Transform -> é um componente da Unity que contém a posição, rotação e escala do objeto(personagem)
        - .forward -> é um vetor que aponta o movimento para frente do obejto(personagem)
        */

        /* ============ 3 / velocidade ==============
        - comprimento do vetor = vetor * velocidade; 
        Ex: transform. forward * 3 = (3, 0, 0) 
        - Nós indicamos o quão rápido ele vai se mover para frente.
        */

        /* ==================== Time.deltaTime =====================
        - é um fator que faz o movimento do personagem ser mais suave.
        - Pois ele faz com que o movimento não seja afetado pelo aumento e decaimento da taxa de quadros(FPS).
        - Como muitas telas rodando em (FPS) diferentes, o .deltaTime arruma essa variação de frames.
        
        EXEMPLO: 
        - Imagine que você está em um jogo de plataforma, e seu personagem precisa se mover para a direita a uma velocidade constante.

        Sem considerar o Time.deltaTime, a movimentação poderia parecer assim:

        No primeiro quadro, o personagem se move 3 unidades para a direita.
        No segundo quadro, o personagem se move mais 3 unidades para a direita.

        Se a taxa de quadros fosse constante, isso não seria um problema. No entanto, se houver uma variação na taxa de quadros:

        Se o segundo quadro demorar o dobro do tempo para ser renderizado, o personagem terá se movido 6 unidades no total, em vez das 3 unidades desejadas.
        
        - Agora, vamos adicionar o Time.deltaTime para corrigir isso:

        Digamos que Time.deltaTime neste segundo quadro seja 0.02 segundos (ou seja, 20 milissegundos).
        Em vez de mover o personagem 3 unidades diretamente, multiplicamos a velocidade pelo Time.deltaTime, assim:
        Velocidade * Tempo = 3 unidades/segundo * 0.02 segundos = 0.06 unidades.
        Então, movemos o personagem 0.06 unidades para a direita neste quadro.
        */

        /* ============================= COMO ELE FUNCIONA ==================================
        Basicamente, o personagem vai se mover 3 unidades por segundo.

        - Com o Time.deltaTime, o jogador que está jogando a 60 FPS verá o personagem se mover a 3 unid p/ seg.
        e o jogador que está a 30 FPS verá o personagem se mover a 3 unid p/ seg.
        */

        // -------------------- float hor = Input.GetAxis("Horizontal"); ------------------------
        // -------------------- float ver = Input.GetAxis("Vertical"); -----------------------------

        /* ======================== DIFERENÇA ENTRE (Input.GetAxis()) e (Input.GetAxis()) ====================
        
        - .GetAxis (-1, 1) -> vai fazer o movimento ser mais suave quando o personagem começar a se mover.
        - .GetAxisRaw (0, 1) -> vai fazer o movimento ser mais "Cru" ou Bruto.

        EX: quando o jogador clicar para ir para a direita:
        No .GetAxis, o personagem vai começar a correr devagar e vai ir acelerando com o tempo.
        No .GetAxisRaw, o personagem vai começar a correr logo que o jogador clicar.
        */

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 movimento = Vector3.zero;
        float speedmove = 0;

        if (hor != 0 || ver != 0)
        {
            Vector3 frente = camera.forward;
            frente.y = 0;  // eixo y = 0; para o jogar não voar
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
            // O Quaternion.Slerp() é usado para fazer uma interpolação entre rotações, para que a mudança entre elas seja mais suave
            // o (0.2f) determina o quando uma transição deve ir para mudar para a outra
        }

        movimento.y += gravidade * Time.deltaTime;

        controlePersonagem.Move(movimento);

        /* ========================= .Normalize() =============================== 
         .Normalize() -> é usado quando você quer apenas a direção daquele vetor, ignorando a sua "Magnitude"
        ou força, fazendo assim o vetor ser igual a 1 e mantendo a direção do vetor.

         .Magnitude() -> é usado para indicar a magnitude daquele vetor (sua distância ou força) e ignora a 
        direção do vetor. 

        Ex: x = -3 e y = 4, tem magnitude(distância) de 5.
        */

        animacao.SetFloat("YSpeed", speedmove);
        animacao.SetFloat("XSpeed", speedmove);
    }
}
