using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterLayer : MonoBehaviour {

    public SpriteRenderer Shadow;
    public SpriteRenderer BackFill;
    public SpriteRenderer Fill;
    public SpriteRenderer Orb;
    public MeshRenderer HP_String;
    public Canvas Ability_1_Canvas;
    public MeshRenderer Ability_String;
    
    
    public SpriteRenderer Card_Sprite;
    public MeshRenderer Name;
    public SpriteRenderer Monster_Sprite;
    
    public SpriteRenderer Statistics;
    public MeshRenderer PA_String;
    public MeshRenderer PD_String;
    public MeshRenderer MA_String;
    public MeshRenderer MD_String;

    public void Update() {

        var siblingIndex = transform.GetSiblingIndex();
        switch (siblingIndex) {
            case 0 :
                Shadow.sortingLayerName = "M1_HA";
                BackFill.sortingLayerName = "M1_HA";
                Fill.sortingLayerName = "M1_HA";
                Orb.sortingLayerName = "M1_HA";
                HP_String.sortingLayerName = "M1_HA";
                Ability_1_Canvas.sortingLayerName = "M1_HA";
                Ability_String.sortingLayerName = "M1_HA";
                
                Card_Sprite.sortingLayerName = "M1_Card";
                Name.sortingLayerName = "M1_Card";
                Monster_Sprite.sortingLayerName = "M1_Card";
                
                Statistics.sortingLayerName = "M1_Statistics";
                PA_String.sortingLayerName = "M1_Statistics";
                PD_String.sortingLayerName = "M1_Statistics";
                MA_String.sortingLayerName = "M1_Statistics";
                MD_String.sortingLayerName = "M1_Statistics";
                break;
            
            case 1 :
                Shadow.sortingLayerName    = "M2_HA";
                BackFill.sortingLayerName  = "M2_HA";
                Fill.sortingLayerName      = "M2_HA";
                Orb.sortingLayerName       = "M2_HA";
                HP_String.sortingLayerName = "M2_HA";
                Ability_1_Canvas.sortingLayerName = "M2_HA";
                Ability_String.sortingLayerName = "M2_HA";
                
                Card_Sprite.sortingLayerName    = "M2_Card";
                Name.sortingLayerName           = "M2_Card";
                Monster_Sprite.sortingLayerName = "M2_Card";
                
                Statistics.sortingLayerName = "M2_Statistics";
                PA_String.sortingLayerName  = "M2_Statistics";
                PD_String.sortingLayerName  = "M2_Statistics";
                MA_String.sortingLayerName  = "M2_Statistics";
                MD_String.sortingLayerName  = "M2_Statistics";
                break;
            
            case 2 :
                Shadow.sortingLayerName    = "M3_HA";
                BackFill.sortingLayerName  = "M3_HA";
                Fill.sortingLayerName      = "M3_HA";
                Orb.sortingLayerName       = "M3_HA";
                HP_String.sortingLayerName = "M3_HA";
                Ability_1_Canvas.sortingLayerName = "M3_HA";
                Ability_String.sortingLayerName = "M3_HA";
                
                Card_Sprite.sortingLayerName    = "M3_Card";
                Name.sortingLayerName           = "M3_Card";
                Monster_Sprite.sortingLayerName = "M3_Card";
                
                Statistics.sortingLayerName = "M3_Statistics";
                PA_String.sortingLayerName  = "M3_Statistics";
                PD_String.sortingLayerName  = "M3_Statistics";
                MA_String.sortingLayerName  = "M3_Statistics";
                MD_String.sortingLayerName  = "M3_Statistics";
                break;
        }
    }
}
