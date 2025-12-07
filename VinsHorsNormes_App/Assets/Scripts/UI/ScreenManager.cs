using System.Collections;
using GoogleSheetsForUnity;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour {

    [SerializeField] private Button enterButton;
    private int counter;
    private float clickTimer = .2f;
    
    [SerializeField] private GameObject[] screens;
    [SerializeField] private InputField passwordText;

    private float timer;
    private bool errorDisplayed;
    [SerializeField] private GameObject errorText;

    private void Start() {
        
        Data.Load();
        
        foreach (var screen in screens) screen.SetActive(false);
        screens[0].SetActive(true);
        
        enterButton.onClick.AddListener(ButtonListener);
    }

    private void Update() {
        errorText.SetActive(errorDisplayed);
        
        if (!errorDisplayed) return;
        timer += Time.deltaTime;
        if (timer > 3f) {
            timer = 0;
            errorDisplayed = false;
        }
    }

    private void ButtonListener() {
        counter++;
        if (counter == 1) {
            StartCoroutine(DoubleClickEvent());
        }
    }

    private IEnumerator DoubleClickEvent() {
        yield return new WaitForSeconds(clickTimer);
        if (counter > 1) {
            ChangeScreenTo(1);
            counter = 0;
        }
        yield return new WaitForSeconds(.05f);
        counter = 0;
    }

    private void EnableSpecificScreen() {
        for (var i = 0; i < screens.Length; i++)
            screens[i].SetActive(i == Data.ScreenIndex);
    }
    
    public void ChangeScreenTo(int index) {
        Data.ScreenIndex = index;
        EnableSpecificScreen();
    }
    
    public void DefineFavorite(bool favorite) {
        Data.Favorite = favorite;
    }
    
    public void DefineState(string state) {
        Data.State = state;
    }

    public void LoadProducts() {
        Drive.GetTable("Regions");
        //Drive.GetTable("Stock");
        Data.SendingFiles = true;
    }

    public void Connection() {
        switch (passwordText.text) {
            case "bouteille":
                ChangeScreenTo(1);
                DefineState("caviste");
                break;
            
            case "sommelier":
                ChangeScreenTo(1);
                DefineState("restaurateur");
                break;
            
            case "lgd34VHN*":
                ChangeScreenTo(4);
                LoadProducts();
                break;
            
            default:
                DisplayError();
                break;
        }
    }

    private void DisplayError() {
        errorDisplayed = true;
    }
    
    public void SendEmail () {
        string email = "ponsalexandre@vinshorsnormes.com";
        string subject = MyEscapeURL("[VHN Vins] Demande d'accès Membre");
        string body = MyEscapeURL("Bonjour,\r\nJe souhaiterai obtenir un accès membre à votre application VHN Vins.");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }
    
    string MyEscapeURL (string url) {
        return WWW.EscapeURL(url).Replace("+","%20");
    }
}
