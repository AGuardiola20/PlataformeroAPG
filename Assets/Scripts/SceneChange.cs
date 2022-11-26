using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void CambiarEscena(string sceneName){
        SceneManager.LoadScene(sceneName);
    }

    public void SalirJuego(){
        Debug.Log("Saliendo del Juego");
        Application.Quit();
    }

    IEnumerator retrasoEscena(string sceneName){
        yield return new WaitForSecondRealTime(1f);
        SceneManager.LoadScene(sceneName);
    }
}
