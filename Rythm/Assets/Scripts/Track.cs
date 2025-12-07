using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum TapType {
    One,
    Two,
    Three
}

public enum StaticPoints {
    Point_A,
    Point_B,
    Point_C,
    Point_D
}

public enum VerticalPoints {
    Point_A_V,
    Point_B_V,
    Point_C_V,
    Point_D_V
}

public enum HorizontalPoints {
    Point_A_H,
    Point_B_H,
    Point_C_H,
    Point_D_H
}



[CreateAssetMenu(fileName = "New Beats Track.asset", menuName = "Rhythm/New Track", order = 1)]
public class Track : ScriptableObject { 
    [Header("Playback Settings")]
    
    [Tooltip("Number of beats per minute")]
    [Range(30, 360)] public int bpm = 120;
    
    [HideInInspector] public List<int> beats = new List<int>();

    static public int inputs = 3;
    
    [Header("Randomization Settings")]
    
    [Tooltip("Minimum number of beats per block")]
    [Range(1, 20)] [SerializeField] private int _minBlock = 2;
    [Tooltip("Maximum number of beats per block")]
    [Range(1, 20)] [SerializeField] private int _maxBlock = 5;
    [Tooltip("Number of beats blocks")]
    [Range(1, 20)] [SerializeField] private int _blocks = 4;
    
    public void Randomize() {
        beats.Clear();
        for (var i = 0; i < _blocks; i++) {
            var blockLength = Random.Range(_minBlock, _maxBlock + 1);
            for (var j = 0; j < blockLength; j++) {
                beats.Add(Random.Range(0, inputs));
            }
        }
    }
}
