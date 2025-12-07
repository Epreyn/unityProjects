using UnityEngine;
using UnityEngine.UI;

public class Favorite : MonoBehaviour {

    public bool isFavorite;
    private Image favoriteImg;
    public string ProductID;

    private void Start() {
        favoriteImg = transform.GetChild(7).GetComponent<Image>();
    }

    public void DefineFavorite() {
        isFavorite = !isFavorite;
        if (isFavorite) {
            if (Data.Favoris.Contains(ProductID)) return;
            Data.Favoris.Add(ProductID);
            Data.Save();
        }
        else {
            if (!Data.Favoris.Contains(ProductID)) return;
            Data.Favoris.Remove(ProductID);
            Data.Save();
        }
    }

    public void DefineColor() {
        favoriteImg.color = isFavorite ? Color.yellow : Color.white;
    }
}
