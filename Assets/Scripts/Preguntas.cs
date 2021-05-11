using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class Preguntas : MonoBehaviour
{
    private readonly string baseURL = "http://localhost:8000";
    private WWW www;
    public string usuarioensesion;
    public Text pregunta, resA, resB, resC, resD, prueba;
    public int nPreguntas = 0;

    [System.Serializable]
    public struct estructuraDatosWeb
    {
        public string Pregunta;
        public string Ra;
        public string Rb;
        public string Rc;
        public string Rd;
    }

    static public estructuraDatosWeb datos;
    public estructuraDatosWeb[] lista;

    // Start is called before the first frame update
    void Start()
    {
        Leer();
        
    }
    // Update is called once per frame
    [ContextMenu("Leer")]
    public void Leer()
    {

        StartCoroutine(CorrutinaLeer());
    }

    IEnumerator CorrutinaLeer()
    {
        bool estadoResponse = true;

        string preguntasURL = baseURL + "/listapreguntas/";

        UnityWebRequest userInfoRequest = UnityWebRequest.Get(preguntasURL);

        yield return userInfoRequest.SendWebRequest();

        if (userInfoRequest.isNetworkError || userInfoRequest.isHttpError)
        {
            Debug.LogError(userInfoRequest.error);
            //yield break;
            estadoResponse = false;
            prueba.text = "error";
        }

        if (estadoResponse)
        {

            JSONNode info = JSON.Parse(userInfoRequest.downloadHandler.text);
            
            lista = new estructuraDatosWeb[info.Count];

            for (int i = 0; i < info.Count; i++)
            {
                lista[i].Pregunta = info[i]["fields"]["Pregunta"];
                lista[i].Ra = info[i]["fields"]["RespuestaA"];
                lista[i].Rb = info[i]["fields"]["RespuestaB"];
                lista[i].Rc = info[i]["fields"]["RespuestaC"];
                lista[i].Rd = info[i]["fields"]["RespuestaD"];
            }
            siguientepreg();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void siguientepreg()
    {
        estructuraDatosWeb preActual = getPregunta();

        pregunta.text = preActual.Pregunta;
        resA.text = preActual.Ra;
        resB.text = preActual.Rb;
        resC.text = preActual.Rc;
        resD.text = preActual.Rd;
    }

    public estructuraDatosWeb getPregunta()
    {

        if (nPreguntas < lista.Length)
        {
            estructuraDatosWeb aux = lista[nPreguntas];
            nPreguntas++;
            return aux;
        }
        else
        {
            nPreguntas = 0;
            return lista[0];
        }
    }

}
