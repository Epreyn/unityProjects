using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TrackView : MonoBehaviour {
    [SerializeField] private Track _track;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private List<GameObject> _taps;
    [SerializeField] private List<Transform> _points;
    
    private int _currentBeat;
    public int Multiplier = 1;
    
    public float SecondsPerBeat {get; private set;}
    public float BeatsPerSecond {get; private set;}
    
    private void Awake() {
        _currentBeat = 0;
        SecondsPerBeat = _track.bpm / 60f;
        BeatsPerSecond = 60f / _track.bpm;
    }
    
    public void Start() {
        _audioSource.clip = _audioClip;
        _audioSource.Play();
        NextTap();
    }
    
    private void NextTap() {
        if (_currentBeat >= _track.beats.Count) {
            return;
        }

        var randomPoint = Random.Range(0, _points.Count);
        var tap = Instantiate(_taps[_track.beats[_currentBeat]], _points[randomPoint]);
        tap.GetComponent<Tap>().SetTrack(_track);
        tap.GetComponent<Tap>().SetMultiplier(Multiplier);
        tap.transform.localPosition = Vector3.zero;
        _currentBeat++;
        
        
        //var randomInterval = BeatsPerSecond * (float)Math.PI / intervals[randomIntervalIndex];
        var randomInterval = BeatsPerSecond * 4 / ChooseIntervalByWeight();
        
        Invoke(nameof(NextTap), randomInterval);
    }

    private int ChooseIntervalByWeight() {
        int[] intervals = {1, 2, 4};
        int[] weights = {20, 30, 50};
        var totalWeight = weights.Sum();
        var randomNumber = Random.Range(0, totalWeight);
        var currentSum = 0;

        for (var i = 0; i < intervals.Length; i++) {
            currentSum += weights[i];
            if (randomNumber < currentSum) {
                return intervals[i];
            }
        }

        return intervals[0];
    }
}
