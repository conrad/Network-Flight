using UnityEngine;

[System.Serializable]
public class ClickManager {

    private float _minTimeToClick = 0.05f;
    private float _maxTimeToClick = 0.60f;

    private float _minCurrentTime;
    private float _maxCurrentTime;

    public bool DoubeClick() {
        if (Time.time >= _minCurrentTime && Time.time <= _maxCurrentTime) {
            _minCurrentTime = 0;
            _maxCurrentTime = 0;
        
            return true;
        }

        _minCurrentTime = Time.time + _minTimeToClick;
        _maxCurrentTime = Time.time + _maxTimeToClick;

        return false;
    }
}
