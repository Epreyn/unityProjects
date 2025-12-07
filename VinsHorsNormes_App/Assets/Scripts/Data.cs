using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public static class Data {

    public static int ScreenIndex;

    public static string[] Lieux;

    public static string State;
    public static bool Favorite;
    public static List<string> Favoris;

    public static bool SendingFiles;
    public static bool CreateProduct, UpdateStock, EditProduct, DeleteProduct;
    
    public static void Save() {
        var bf = new BinaryFormatter();
        var file = File.Create (Application.persistentDataPath + "/saveVHN.gd");
        
        bf.Serialize(file, Favoris);
        
        file.Close();
    }
     
    public static void Load() {
        if (!File.Exists(Application.persistentDataPath + "/saveVHN.gd")) {
            Favoris = new List<string>();
            return;
        }
        
        var bf = new BinaryFormatter();
        var file = File.Open(Application.persistentDataPath + "/saveVHN.gd", FileMode.Open);
            
        Favoris = (List<string>)bf.Deserialize(file);
            
        file.Close();
    }
}
