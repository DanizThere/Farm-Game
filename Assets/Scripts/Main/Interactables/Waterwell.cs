using UnityEngine;

public class Waterwell : DefaultSelectable
{
    [SerializeField] private float _maxVolumeInDay = 50f;

    [SerializeField] private WaterwellUI _ui;
    private float _currentVolume;

    protected override void SLStart()
    {
        base.SLStart();

        ServiceLocator.Instance.GetService<TimeManager>().OnDayChange += () =>
        {
            _currentVolume = _maxVolumeInDay;
            _ui.UpdateText($"{_currentVolume:0000}L. remains from {_maxVolumeInDay}L.");
        };

        _currentVolume = _maxVolumeInDay;

        _ui.Setup(this);
        _ui.UpdateText($"{_currentVolume:00}L. remains from {_maxVolumeInDay}L.");
    }

    public void Fill(Bucket bucket)
    {
        if (_currentVolume < 0) return; 
        var fill = Time.deltaTime;
        bucket.Fill(fill);

        _currentVolume -= fill;

        _ui.UpdateText($"{_currentVolume:00}L. remains from {_maxVolumeInDay}L.");
    }
}
