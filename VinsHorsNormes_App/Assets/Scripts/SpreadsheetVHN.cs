using System.Collections.Generic;
using System.Linq;
using GoogleSheetsForUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_WEBGL 
    public class FixRectMask2dWebGL : MonoBehaviour {
        private void Awake() {
            var items = GetComponentsInChildren<MaskableGraphic>(true);
            for (int i = 0; i < items.Length; i++) {
                Material m = items[i].materialForRendering;
                if (m != null) m.EnableKeyword("UNITY_UI_CLIP_RECT");
            }
        }
    }
#endif

public class Item {
    public string Domaine { get; set; }
    public string Lieu { get; set; }
    public string Paris { get; set; }
    public string Livraison { get; set; }
}

public class SpreadsheetVHN : MonoBehaviour {

    [SerializeField]
    private GameObject[] createFields, editFields;
    
    [System.Serializable]
    public struct Product
    {
        public string domaine;
        public string cuvee;
        public string millesime;
        public string format;
        public string quantite;
        public string conditionnement;
        public string tarifCaviste;
        public string tarifRestaurateur;
        public string id;

        public string lieu;
        public string paris;
        public string couleur;
        public string livraison;
    }
    
    [System.Serializable]
    public struct Region {
        public string region;
    }

    private Product _product;
    private string _tableName = "Stock";
    private Region _region;

    private Product[] _products;
    private string quantity, condition, costC, costR, productID, location, paris, wineColor, livraison;
    private Region[] _regions;

    public TMP_Dropdown[] DropdownLieux;

    private float timer;
    private bool validationDisplayed;

    [Header("Validation Fields")] 
    [SerializeField] private GameObject validateButton;
    [SerializeField] private GameObject validateEditionButton;
    [SerializeField] private GameObject[] validationTexts;

    [Header("Change Stock")] 
    [SerializeField] private GameObject stockDomaineDropdown;
    [SerializeField] private GameObject stockCuveeDropdown;
    [SerializeField] private GameObject stockMillesimeDropdown;
    [SerializeField] private GameObject stockFormatDropdown;
    [SerializeField] private GameObject stockQuantityInputField;
    
    [SerializeField] private GameObject stockSellInputField;
    
    [Header("Edition Produit")] 
    [SerializeField] private GameObject editionDomaineDropdown;
    [SerializeField] private GameObject editionCuveeDropdown;
    [SerializeField] private GameObject editionMillesimeDropdown;
    [SerializeField] private GameObject editionFormatDropdown;
    
    [Header("Suppression Produit")] 
    [SerializeField] private GameObject suppressionDomaineDropdown;
    [SerializeField] private GameObject suppressionCuveeDropdown;
    [SerializeField] private GameObject suppressionMillesimeDropdown;
    [SerializeField] private GameObject suppressionFormatDropdown;

    [Header("Liste")] 
    [SerializeField] private GameObject domainePrefab;
    [SerializeField] private GameObject productPrefab;
    [SerializeField] private GameObject locationPrefab;
    [SerializeField] private GameObject domaineParent;
    [SerializeField] private Color[] productColors;

    private int FavoriteCount;

    private void Update() {
        
        foreach (var validationText in validationTexts) {
            validationText.SetActive(validationDisplayed);
        }
        
        if (!validationDisplayed) return;
        timer += Time.deltaTime;
        if (!(timer > 3f)) return;
        timer = 0;
        validationDisplayed = false;
    }

    private void LateUpdate() {
        if (stockSellInputField.GetComponent<InputField>().text == "") stockSellInputField.GetComponent<InputField>().text = "0";
        
        DefineIfCanValidate();
        DefineIfCanValidateEdition();
        if (Data.CreateProduct && !Data.SendingFiles) ResetCreateProductFields();
        if (Data.UpdateStock && !Data.SendingFiles) StockUpdated();
        if (Data.EditProduct && !Data.SendingFiles) ProductUpdated();
        if (Data.DeleteProduct && !Data.SendingFiles) ProductDeleted();
    }
    
    private void CreateList(bool favorite) {

        domaineParent.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(370, 0);
        foreach (Transform t in domaineParent.transform) Destroy(t.gameObject);
        
        var items = new Item[_products.Length];
        for (var i = 0; i < _products.Length; i++)
            items[i] = new Item {
                Domaine = _products[i].domaine,
                Paris = _products[i].paris,
                Lieu = _products[i].lieu,
                Livraison = _products[i].livraison
            };
        
        Debug.Log(items);

        var sortItems = items.GroupBy(i => i.Domaine).Select(j => j.First()).OrderBy(k => k.Domaine);
        float fact = 0;

        foreach (var t in Data.Lieux) {
            var cloneLocation = Instantiate(locationPrefab, Vector3.zero, Quaternion.identity);
             
            cloneLocation.transform.SetParent(domaineParent.transform);
            cloneLocation.transform.localScale = new Vector3(1,1,1);
            cloneLocation.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, fact, 0);
            cloneLocation.transform.GetChild(0).GetComponent<Text>().text = t;
            
            domaineParent.transform.GetComponent<RectTransform>().sizeDelta += 
                new Vector2(0, cloneLocation.transform.GetComponent<RectTransform>().sizeDelta.y + 15);
            fact -= cloneLocation.transform.GetComponent<RectTransform>().sizeDelta.y + 15;
            
            
            foreach (var item in sortItems) {
                FavoriteCount = 0;
                
                if (item.Lieu != t) continue;
                
                var cloneDomaine = Instantiate(domainePrefab, Vector3.zero, Quaternion.identity);
             
                cloneDomaine.transform.SetParent(domaineParent.transform);
                cloneDomaine.transform.localScale = new Vector3(1,1,1);
                cloneDomaine.transform.GetChild(0).GetComponent<Text>().text = item.Domaine;
                cloneDomaine.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = item.Paris == "Oui";
                cloneDomaine.transform.GetChild(3).gameObject.GetComponent<Image>().enabled = item.Livraison == "Oui";
                
                var cuvees = new List<string>();
                var millesime = new List<string>();
                var couleur = new List<string>();
                var format = new List<string>();
                var conditionnement = new List<string>();
                var quantite = new List<string>();
                var tarif = new List<string>();
                
                var id = new List<string>();
                
                for (var i = 0; i < _products.Length; i++) {
                    if (_products[i].domaine != item.Domaine) continue;
                    cuvees.Add(_products[i].cuvee);
                    millesime.Add(_products[i].millesime);
                    couleur.Add(_products[i].couleur);
                    format.Add(_products[i].format);
                    conditionnement.Add(_products[i].conditionnement);
                    quantite.Add(_products[i].quantite);
                    id.Add(_products[i].id);

                    switch (Data.State) {
                        case "invite":
                            // NO COST
                            break;
                        
                        case "caviste":
                            tarif.Add(_products[i].tarifCaviste);
                            break;
                        
                        case "restaurateur":
                            tarif.Add(_products[i].tarifRestaurateur);
                            break;
                    }
                }
    
                for (var j = 0; j < cuvees.Count; j++) {
                    if (Data.Favorite) {
                        foreach (var favori in Data.Favoris) {
                            if (id[j] != favori) continue;
                            FavoriteCount++;
                    
                            var cloneProduct = Instantiate(productPrefab, Vector3.zero, Quaternion.identity);
                                                
                            cloneProduct.transform.SetParent(cloneDomaine.transform.GetChild(1).transform);
                            cloneProduct.transform.localScale = new Vector3(1,1,1);
                            cloneProduct.transform.GetChild(0).GetComponent<Text>().text = cuvees[j];
                            cloneProduct.transform.GetChild(1).GetComponent<Text>().text = millesime[j];
                            cloneProduct.transform.GetChild(2).GetComponent<Text>().text = couleur[j];
                            cloneProduct.transform.GetChild(3).GetComponent<Text>().text = format[j];
                            cloneProduct.transform.GetChild(4).GetComponent<Text>().text = conditionnement[j];
                            cloneProduct.transform.GetChild(5).GetComponent<Text>().text = quantite[j];
                            if (Data.State != "invite") {
                                cloneProduct.transform.GetChild(6).GetComponent<Text>().text = tarif[j];
                                cloneProduct.transform.GetChild(6).gameObject.SetActive(true);
                            }
                            else cloneProduct.transform.GetChild(6).gameObject.SetActive(true);
                            cloneProduct.transform.GetComponent<Favorite>().ProductID = id[j];
                            
                            cloneProduct.GetComponent<Image>().color = int.Parse(quantite[j]) != 0 ? productColors[0] : productColors[1];
                            
                            cloneProduct.transform.GetComponent<Favorite>().isFavorite = true;
                            cloneProduct.transform.GetChild(7).GetComponent<Image>().color = Color.yellow;
                        }
                    }
                    else {
                        var cloneProduct = Instantiate(productPrefab, Vector3.zero, Quaternion.identity);
                    
                        cloneProduct.transform.SetParent(cloneDomaine.transform.GetChild(1).transform);
                        cloneProduct.transform.localScale = new Vector3(1,1,1);
                        cloneProduct.transform.GetChild(0).GetComponent<Text>().text = cuvees[j];
                        cloneProduct.transform.GetChild(1).GetComponent<Text>().text = millesime[j];
                        cloneProduct.transform.GetChild(2).GetComponent<Text>().text = couleur[j];
                        cloneProduct.transform.GetChild(3).GetComponent<Text>().text = format[j];
                        cloneProduct.transform.GetChild(4).GetComponent<Text>().text = conditionnement[j];
                        cloneProduct.transform.GetChild(5).GetComponent<Text>().text = quantite[j];
                        if (Data.State != "invite") {
                            cloneProduct.transform.GetChild(6).GetComponent<Text>().text = tarif[j];
                            cloneProduct.transform.GetChild(6).gameObject.SetActive(true);
                        }
                        else cloneProduct.transform.GetChild(6).gameObject.SetActive(true);
                        cloneProduct.transform.GetComponent<Favorite>().ProductID = id[j];
                        cloneProduct.GetComponent<Image>().color = int.Parse(quantite[j]) != 0 ? productColors[0] : productColors[1];
                    
                        foreach (var favori in Data.Favoris) {
                            if (id[j] != favori) continue;
                            cloneProduct.transform.GetComponent<Favorite>().isFavorite = true;
                            cloneProduct.transform.GetChild(7).GetComponent<Image>().color = Color.yellow;
                        }
                    }
                }
                
                cloneDomaine.transform.GetComponent<RectTransform>().sizeDelta = 
                    Data.Favorite ? new Vector2(320, 210 + 165 * (FavoriteCount - 1)) : new Vector2(320, 210 + 165 * (cuvees.Count - 1));
                domaineParent.transform.GetComponent<RectTransform>().sizeDelta += 
                    new Vector2(0, cloneDomaine.transform.GetComponent<RectTransform>().sizeDelta.y + 15);
                cloneDomaine.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, fact, 0);
                fact -= cloneDomaine.transform.GetComponent<RectTransform>().sizeDelta.y + 15;
            }
        }
    }

    #region Validate Region

    private void DefineIfCanValidate() {
        var canValidate = createFields[0].transform.GetChild(2).GetComponent<InputField>().text != ""
                          && createFields[1].transform.GetChild(2).GetComponent<InputField>().text != ""
                          && createFields[4].transform.GetChild(2).GetComponent<InputField>().text != ""
                          && createFields[6].transform.GetChild(2).GetComponent<InputField>().text != ""
                          && createFields[7].transform.GetChild(2).GetComponent<InputField>().text != "";

       if (!Data.SendingFiles) validateButton.GetComponent<Button>().interactable = canValidate;
    }
    
    private void DefineIfCanValidateEdition() {
        var canValidate = editFields[0].transform.GetChild(2).GetComponent<InputField>().text != ""
                          && editFields[1].transform.GetChild(2).GetComponent<InputField>().text != ""
                          && editFields[4].transform.GetChild(2).GetComponent<InputField>().text != ""
                          && editFields[6].transform.GetChild(2).GetComponent<InputField>().text != ""
                          && editFields[7].transform.GetChild(2).GetComponent<InputField>().text != "";

        if (!Data.SendingFiles) validateEditionButton.GetComponent<Button>().interactable = canValidate;
    }

    private void StockUpdated() {
        validationDisplayed = true;
        foreach (var validationText in validationTexts) {
            validationText.GetComponent<TMP_Text>().text = "Stock mis à jour";
        }
        Data.UpdateStock = false;
    }
    
    private void ProductUpdated() {
        editFields[0].transform.GetChild(2).GetComponent<InputField>().text = "";
        editFields[1].transform.GetChild(2).GetComponent<InputField>().text = "";
        editFields[4].transform.GetChild(2).GetComponent<InputField>().text = "";
        editFields[6].transform.GetChild(2).GetComponent<InputField>().text = "";
        editFields[7].transform.GetChild(2).GetComponent<InputField>().text = "";
        
        GetComponent<ScreenManager>().ChangeScreenTo(4);
        validationDisplayed = true;
        foreach (var validationText in validationTexts) {
            validationText.GetComponent<TMP_Text>().text = "Produit modifié";
        }
        Data.EditProduct = false;
    }

    private void ProductDeleted() {
        GetComponent<ScreenManager>().ChangeScreenTo(4);
        validationDisplayed = true;
        foreach (var validationText in validationTexts) {
            validationText.GetComponent<TMP_Text>().text = "Produit supprimé";
        }
        Data.DeleteProduct = false;
    }

    #endregion 

    #region DropDowns & InputFields
    
    public void UpdateDropdowns() {
        if (Data.ScreenIndex == 7) GetDomainToDropdown(stockDomaineDropdown, stockCuveeDropdown, stockMillesimeDropdown, stockFormatDropdown);
        if (Data.ScreenIndex == 8) GetDomainToDropdown(editionDomaineDropdown, editionCuveeDropdown, editionMillesimeDropdown, editionFormatDropdown);
        if (Data.ScreenIndex == 11) GetDomainToDropdown(suppressionDomaineDropdown, suppressionCuveeDropdown, suppressionMillesimeDropdown, suppressionFormatDropdown);
    }

    private void GetDomainToDropdown(GameObject domaineDD, GameObject cuveeDD, GameObject millesimeDD, GameObject formatDD) {        
        domaineDD.GetComponent<TMP_Dropdown>().ClearOptions();
        var _domaines = new string[_products.Length];
        for (var i = 0; i < _products.Length; i++) {
            _domaines[i] = _products[i].domaine;
        }

        var distinctDomaines = _domaines.Distinct().ToList();
        distinctDomaines.Sort();
        domaineDD.GetComponent<TMP_Dropdown>().AddOptions(distinctDomaines);
        
        GetCuveeToDropdown(domaineDD, cuveeDD, millesimeDD, formatDD);
    }
    
    private void GetCuveeToDropdown(GameObject domaineDD, GameObject cuveeDD, GameObject millesimeDD, GameObject formatDD) {
        cuveeDD.GetComponent<TMP_Dropdown>().ClearOptions();
        var currentDomain = domaineDD.GetComponent<TMP_Dropdown>().captionText.text;

        var cuvees = new List<string>();
        for (var i = 0; i < _products.Length; i++) {
            if (_products[i].domaine == currentDomain) cuvees.Add(_products[i].cuvee);
        }

        var disctinctCuvees = cuvees.Distinct().ToList();
        disctinctCuvees.Sort();
        cuveeDD.GetComponent<TMP_Dropdown>().AddOptions(disctinctCuvees);
        
        GetMillesimeToDropdown(domaineDD, cuveeDD, millesimeDD, formatDD);
    }
    
    private void GetMillesimeToDropdown(GameObject domaineDD, GameObject cuveeDD, GameObject millesimeDD, GameObject formatDD) {
        millesimeDD.GetComponent<TMP_Dropdown>().ClearOptions();
        var currentDomain = domaineDD.GetComponent<TMP_Dropdown>().captionText.text;
        var currentCuvee = cuveeDD.GetComponent<TMP_Dropdown>().captionText.text;

        var millesimes = new List<string>();
        for (var i = 0; i < _products.Length; i++) {
            if (_products[i].domaine == currentDomain
                && _products[i].cuvee == currentCuvee) millesimes.Add(_products[i].millesime);
        }

        var disctinctMillesimes = millesimes.Distinct().ToList();
        disctinctMillesimes.Sort();
        millesimeDD.GetComponent<TMP_Dropdown>().AddOptions(disctinctMillesimes);
        
        GetFormatToDropdown(domaineDD, cuveeDD, millesimeDD, formatDD);
    }
    
    private void GetFormatToDropdown(GameObject domaineDD, GameObject cuveeDD, GameObject millesimeDD, GameObject formatDD) {
        formatDD.GetComponent<TMP_Dropdown>().ClearOptions();
        var currentDomain = domaineDD.GetComponent<TMP_Dropdown>().captionText.text;
        var currentCuvee = cuveeDD.GetComponent<TMP_Dropdown>().captionText.text;
        var currentMillesime = millesimeDD.GetComponent<TMP_Dropdown>().captionText.text;

        var format = new List<string>();
        for (var i = 0; i < _products.Length; i++) {
            if (_products[i].domaine == currentDomain
                && _products[i].cuvee == currentCuvee
                && _products[i].millesime == currentMillesime) format.Add(_products[i].format);
        }

        var disctinctFormat = format.Distinct().ToList();
        disctinctFormat.Sort();
        formatDD.GetComponent<TMP_Dropdown>().AddOptions(disctinctFormat);
        
        if (Data.ScreenIndex == 7) GetQuantiteToInputField();
    }
    
    public void GetQuantiteToInputField() {
        var currentDomain = stockDomaineDropdown.GetComponent<TMP_Dropdown>().captionText.text;
        var currentCuvee = stockCuveeDropdown.GetComponent<TMP_Dropdown>().captionText.text;
        var currentMillesime = stockMillesimeDropdown.GetComponent<TMP_Dropdown>().captionText.text;
        var currentFormat = stockFormatDropdown.GetComponent<TMP_Dropdown>().captionText.text;

        for (var i = 0; i < _products.Length; i++) {
            if (_products[i].domaine == currentDomain
                && _products[i].cuvee == currentCuvee
                && _products[i].millesime == currentMillesime
                && _products[i].format == currentFormat) {
                quantity = _products[i].quantite;
                condition = _products[i].conditionnement;
                costC = _products[i].tarifCaviste;
                costR = _products[i].tarifRestaurateur;
                productID = _products[i].id;
                
                wineColor = _products[i].couleur;
                location = _products[i].lieu;
                paris = _products[i].paris;
                livraison = _products[i].livraison;
            }
        }

        stockQuantityInputField.GetComponent<InputField>().text = quantity;
    }

    #endregion

    #region Create Field

    public void CreateProduct() {
        Data.CreateProduct = true;
        
        _product = new Product {
            domaine           = createFields[0].transform.GetChild(2).GetComponent<InputField>().text.ToUpper(),
            cuvee             = createFields[1].transform.GetChild(2).GetComponent<InputField>().text,
            millesime         = createFields[2].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text,
            format            = createFields[3].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text,
            quantite          = createFields[4].transform.GetChild(2).GetComponent<InputField>().text,
            conditionnement   = createFields[5].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text,
            tarifCaviste      = createFields[6].transform.GetChild(2).GetComponent<InputField>().text,
            tarifRestaurateur = createFields[7].transform.GetChild(2).GetComponent<InputField>().text,
            id                = 
                createFields[0].transform.GetChild(2).GetComponent<InputField>().text.ToUpper()
                + createFields[1].transform.GetChild(2).GetComponent<InputField>().text
                + createFields[2].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text
                + createFields[3].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text,
            
            couleur = createFields[8].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text,
            lieu = createFields[9].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text,
            paris = createFields[10].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text,
            livraison = createFields[11].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text
        };
        
        var jsonProduct = JsonUtility.ToJson(_product);
        Drive.CreateObject(jsonProduct, _tableName);

        Data.SendingFiles = true;
    }

    private void ResetCreateProductFields() {
        createFields[0].transform.GetChild(2).GetComponent<InputField>().text = "";
        createFields[1].transform.GetChild(2).GetComponent<InputField>().text = "";
        createFields[4].transform.GetChild(2).GetComponent<InputField>().text = "";
        createFields[6].transform.GetChild(2).GetComponent<InputField>().text = "";
        createFields[7].transform.GetChild(2).GetComponent<InputField>().text = "";
        
        GetComponent<ScreenManager>().ChangeScreenTo(4);
        validationDisplayed = true;
        foreach (var validationText in validationTexts) {
            validationText.GetComponent<TMP_Text>().text = "Produit créé";
        }
        Data.CreateProduct = false;
    }

    #endregion

    #region Edition Fields

    public void GetInformation() {
        var currentDomain = editionDomaineDropdown.GetComponent<TMP_Dropdown>().captionText.text;
        var currentCuvee = editionCuveeDropdown.GetComponent<TMP_Dropdown>().captionText.text;
        var currentMillesime = editionMillesimeDropdown.GetComponent<TMP_Dropdown>().captionText.text;
        var currentFormat = editionFormatDropdown.GetComponent<TMP_Dropdown>().captionText.text;

        for (var i = 0; i < _products.Length; i++) {
            if (_products[i].domaine == currentDomain
                && _products[i].cuvee == currentCuvee
                && _products[i].millesime == currentMillesime
                && _products[i].format == currentFormat) {
                quantity = _products[i].quantite;
                condition = _products[i].conditionnement;
                costC = _products[i].tarifCaviste;
                costR = _products[i].tarifRestaurateur;
                productID = _products[i].id;
                
                wineColor = _products[i].couleur;
                location = _products[i].lieu;
                paris = _products[i].paris;
                livraison = _products[i].livraison;
            }
        }
        
        _product = new Product {
            domaine           = currentDomain,
            cuvee             = currentCuvee,
            millesime         = currentMillesime,
            format            = currentFormat,
            quantite          = quantity,
            conditionnement   = condition,
            tarifCaviste      = costC,
            tarifRestaurateur = costR,
            id                = productID,
            
            couleur = wineColor,
            lieu = location,
            paris = paris,
            livraison = livraison
        };

        editFields[0].transform.GetChild(2).GetComponent<InputField>().text = _product.domaine;
        editFields[1].transform.GetChild(2).GetComponent<InputField>().text = _product.cuvee;
        editFields[2].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text = _product.millesime;
        editFields[3].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text = _product.format;
        editFields[4].transform.GetChild(2).GetComponent<InputField>().text = _product.quantite;
        editFields[5].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text = _product.conditionnement;
        editFields[6].transform.GetChild(2).GetComponent<InputField>().text = _product.tarifCaviste;
        editFields[7].transform.GetChild(2).GetComponent<InputField>().text = _product.tarifRestaurateur;
            
        editFields[8].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text = _product.couleur;
        editFields[9].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text = _product.lieu;
        editFields[10].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text = _product.paris;
        editFields[11].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text = _product.livraison;
    }
    
    public void EditProduct() {
        Data.EditProduct = true;

        var _oldProduct = _product;
        /*var _dltProduct = new Product {
            domaine           = "",
            cuvee             = "",
            millesime         = "",
            format            = "",
            quantite          = "",
            conditionnement   = "",
            tarifCaviste      = "",
            tarifRestaurateur = "",
            id                = "",
            lieu              = "",
            paris             = "",
            couleur           = ""
        };
        
        var jsonDltProduct = JsonUtility.ToJson(_dltProduct);
        Drive.UpdateObjects(_tableName, "id", _oldProduct.id, jsonDltProduct, false);*/
        
        _product = new Product {
            domaine           = editFields[0].transform.GetChild(2).GetComponent<InputField>().text.ToUpper(),
            cuvee             = editFields[1].transform.GetChild(2).GetComponent<InputField>().text,
            millesime         = editFields[2].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text,
            format            = editFields[3].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text,
            quantite          = editFields[4].transform.GetChild(2).GetComponent<InputField>().text,
            conditionnement   = editFields[5].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text,
            tarifCaviste      = editFields[6].transform.GetChild(2).GetComponent<InputField>().text,
            tarifRestaurateur = editFields[7].transform.GetChild(2).GetComponent<InputField>().text,
            id                = 
            editFields[0].transform.GetChild(2).GetComponent<InputField>().text.ToUpper()
            + editFields[1].transform.GetChild(2).GetComponent<InputField>().text
            + editFields[2].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text
            + editFields[3].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text,
            
            couleur = editFields[8].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text,
            lieu = editFields[9].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text,
            paris = editFields[10].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text,
            livraison = editFields[11].transform.GetChild(2).GetComponent<TMP_Dropdown>().captionText.text
        };
        
        var jsonProduct = JsonUtility.ToJson(_product);
        Drive.UpdateObjects(_tableName, "id", _oldProduct.id, jsonProduct, false);
        //Drive.CreateObject(jsonProduct, _tableName);

        Data.SendingFiles = true;
    }

    #endregion

    #region Stock Fields

    public void UpdateStock() {
        Data.UpdateStock = true;
        
        var currentDomain = stockDomaineDropdown.GetComponent<TMP_Dropdown>().captionText.text;
        var currentCuvee = stockCuveeDropdown.GetComponent<TMP_Dropdown>().captionText.text;
        var currentMillesime = stockMillesimeDropdown.GetComponent<TMP_Dropdown>().captionText.text;
        var currentFormat = stockFormatDropdown.GetComponent<TMP_Dropdown>().captionText.text;
        var currentQuantite = stockQuantityInputField.GetComponent<InputField>().text;

        var indexToUpdate = 0;

        for (var i = 0; i < _products.Length; i++) {
            if (_products[i].domaine == currentDomain
                && _products[i].cuvee == currentCuvee
                && _products[i].millesime == currentMillesime
                && _products[i].format == currentFormat) {
                quantity = currentQuantite;
                condition = _products[i].conditionnement;
                costC = _products[i].tarifCaviste;
                costR = _products[i].tarifRestaurateur;
                productID = _products[i].id;
                
                wineColor = _products[i].couleur;
                location = _products[i].lieu;
                paris = _products[i].paris;
                livraison = _products[i].livraison;

                indexToUpdate = i;
            }
        }
        
        _product = new Product {
            domaine           = currentDomain,
            cuvee             = currentCuvee,
            millesime         = currentMillesime,
            format            = currentFormat,
            quantite          = currentQuantite,
            conditionnement   = condition,
            tarifCaviste      = costC,
            tarifRestaurateur = costR,
            id = productID,
            
            couleur = wineColor,
            lieu = location,
            paris = paris,
            livraison = livraison
        };
        
        var jsonProduct = JsonUtility.ToJson(_product);
        Drive.UpdateObjects(_tableName, "id", productID, jsonProduct, false);

        Data.SendingFiles = true;

        _products[indexToUpdate].quantite = currentQuantite;
    }
    
    public void ChangeQuantite(int value) {
        var quantityChanged = int.Parse(stockQuantityInputField.GetComponent<InputField>().text);
        quantityChanged += value;
        stockQuantityInputField.GetComponent<InputField>().text = quantityChanged.ToString();
    }

    public void Sell(int value) {
        var newQuantity = int.Parse(stockQuantityInputField.GetComponent<InputField>().text);
        newQuantity += int.Parse(stockSellInputField.GetComponent<InputField>().text) * value;
        stockQuantityInputField.GetComponent<InputField>().text = newQuantity.ToString();
        stockSellInputField.GetComponent<InputField>().text = "0";
    }

    #endregion

    #region Delete Fields

    public void DeleteProduct() {
        Data.DeleteProduct = true;
        
        var currentDomain = suppressionDomaineDropdown.GetComponent<TMP_Dropdown>().captionText.text;
        var currentCuvee = suppressionCuveeDropdown.GetComponent<TMP_Dropdown>().captionText.text;
        var currentMillesime = suppressionMillesimeDropdown.GetComponent<TMP_Dropdown>().captionText.text;
        var currentFormat = suppressionFormatDropdown.GetComponent<TMP_Dropdown>().captionText.text;
        
        for (var i = 0; i < _products.Length; i++) {
            if (_products[i].domaine == currentDomain
                && _products[i].cuvee == currentCuvee
                && _products[i].millesime == currentMillesime
                && _products[i].format == currentFormat) {
                quantity = _products[i].quantite;
                condition = _products[i].conditionnement;
                costC = _products[i].tarifCaviste;
                costR = _products[i].tarifRestaurateur;
                productID = _products[i].id;
                
                location = _products[i].lieu;
                paris = _products[i].paris;
                livraison = _products[i].livraison;
                wineColor = _products[i].couleur;
            }
        }
        
        _product = new Product {
            domaine           = "",
            cuvee             = "",
            millesime         = "",
            format            = "",
            quantite          = "",
            conditionnement   = "",
            tarifCaviste      = "",
            tarifRestaurateur = "",
            id                = "",
            lieu              = "",
            paris             = "",
            couleur           = "",
            livraison         = ""
        };
        
        var jsonProduct = JsonUtility.ToJson(_product);
        Drive.UpdateObjects(_tableName, "id", productID, jsonProduct, false);
        
        //Drive.DeleteObjects(_tableName, "id", productID);

        Data.SendingFiles = true;
    }

    #endregion
    
    #region Drive Connection

    private void OnEnable() {
        Drive.responseCallback += HandleDriveResponse;
    }

    private void OnDisable() {
        Drive.responseCallback -= HandleDriveResponse;
    }

    public void HandleDriveResponse(Drive.DataContainer dataContainer) {
        Debug.Log(dataContainer.msg);

        if (dataContainer.QueryType == Drive.QueryType.getObjectsByField) {
            var rawJSon = dataContainer.payload;
            Debug.Log(rawJSon);

            if (string.Compare(dataContainer.objType, _tableName) == 0) {
                _products = JsonHelper.ArrayFromJson<Product>(rawJSon);
            }
        }

        if (dataContainer.QueryType == Drive.QueryType.getTable) {
            string rawJSon = dataContainer.payload;
            Debug.Log(rawJSon);

            if (string.Compare(dataContainer.objType, "Regions") == 0) {
                _regions = JsonHelper.ArrayFromJson<Region>(rawJSon);
                Data.Lieux = new string[_regions.Length];

                for (var i = 0; i < _regions.Length; i++) {
                    Data.Lieux[i] = _regions[i].region;
                }

                foreach (var d in DropdownLieux) {
                    if (d.options.Count == 0) d.AddOptions(Data.Lieux.ToList());
                }
                
                
                Drive.GetTable("Stock");
            }
            
            if (string.Compare(dataContainer.objType, _tableName) == 0) {
                _products = JsonHelper.ArrayFromJson<Product>(rawJSon);
                
                if (Data.ScreenIndex == 7) GetDomainToDropdown(stockDomaineDropdown, stockCuveeDropdown, stockMillesimeDropdown, stockFormatDropdown);
                if (Data.ScreenIndex == 8) GetDomainToDropdown(editionDomaineDropdown, editionCuveeDropdown, editionMillesimeDropdown, editionFormatDropdown);
                if (Data.ScreenIndex == 11) GetDomainToDropdown(suppressionDomaineDropdown, suppressionCuveeDropdown, suppressionMillesimeDropdown, suppressionFormatDropdown);
                
                if (Data.ScreenIndex == 3) CreateList(Data.Favorite);
                
                Data.SendingFiles = false;
            }
        }

        if (dataContainer.QueryType == Drive.QueryType.getAllTables) {
            string rawJSon = dataContainer.payload;
            Drive.DataContainer[] tables = JsonHelper.ArrayFromJson<Drive.DataContainer>(rawJSon);

            string logMsg = "<color=yellow>All data tables retrieved from the cloud.\n</color>";
            for (int i = 0; i < tables.Length; i++)
            {
                logMsg += "\n<color=blue>Table Name: " + tables[i].objType + "</color>\n" + tables[i].payload + "\n";
            }
            Debug.Log(logMsg);
            Data.SendingFiles = false;
        }
        
        Data.SendingFiles = false;
    }

    #endregion
}
