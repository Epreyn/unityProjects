using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Track))]
public class TrackEditor : Editor {
    Track track;
    private Vector2 _scrollPos;
    private bool _displayBeatsData;
    
    public void OnEnable() {
        track = (Track)target;
    }
    
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        if (track.beats.Count == 0) {
            EditorGUILayout.HelpBox("Empty track", MessageType.Info);
            if (GUILayout.Button("Generate random track", EditorStyles.miniButton)) {
                track.Randomize();
            }
        }
        else {
            if (GUILayout.Button("Update random track", EditorStyles.miniButton)) {
                track.Randomize();
            }

            _displayBeatsData = EditorGUILayout.Foldout(_displayBeatsData, "Display Beats");
            if (_displayBeatsData) {
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
                for (var i = 0; i < track.beats.Count; i++) {
                    track.beats[i] = EditorGUILayout.IntSlider(track.beats[i], 0, Track.inputs - 1);
                }
                EditorGUILayout.EndScrollView();
            }
        }
        
        EditorUtility.SetDirty(target);
    }
}
