using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventTitle : MonoBehaviour
{
    [field: SerializeField]
    public float SlideInDuration = 1.0f;

    [field: SerializeField]
    public float DisplayDuration = 1.5f;

    [field: SerializeField]
    public float SlideOutDuration = 1.0f;

    [field: SerializeField]
    public int ShowY = 20;

    [field: SerializeField]
    public int HideY = -100;

    readonly private Queue<Title> _titles = new Queue<Title>();
    private Transition _currentTransition;
    private Title _current;
    private float _timer;

    private GameObject _container;
    private RectTransform _rectTransform;
    private TextMeshProUGUI _textPositive;
    private TextMeshProUGUI _textNeutral;
    private TextMeshProUGUI _textNegative;

    private void Start()
    {
        _container = GameObject.Find("EventTitleContainer");
        _rectTransform = _container.GetComponent<RectTransform>();
        _textPositive = GameObject.Find("EventTitlePositive").GetComponent<TextMeshProUGUI>();
        _textNeutral = GameObject.Find("EventTitleNeutral").GetComponent<TextMeshProUGUI>();
        _textNegative = GameObject.Find("EventTitleNegative").GetComponent<TextMeshProUGUI>();

        Queue(Type.Positive, "Positive Title");
        Queue(Type.Neutral, "Neutral Title");
        Queue(Type.Negative, "Negative Title");
    }

    private void FixedUpdate()
    {
        if (_current == null && _titles.Count > 0)
        {
            _current = _titles.Dequeue();
            _currentTransition = Transition.SlideIn;
            _timer = SlideInDuration;

            switch (_current.Type)
            {
                case Type.Positive:
                    _textPositive.text = _current.Text;
                    _textNeutral.text = "";
                    _textNegative.text = "";
                    break;
                case Type.Neutral:
                    _textPositive.text = "";
                    _textNeutral.text = _current.Text;
                    _textNegative.text = "";
                    break;
                case Type.Negative:
                    _textPositive.text = "";
                    _textNeutral.text = "";
                    _textNegative.text = _current.Text;
                    break;
            }
        }

        var position = _rectTransform.anchoredPosition;
        _timer -= Time.fixedDeltaTime;

        switch (_currentTransition)
        {
            case Transition.SlideIn:
                position.y = EasingFunction.EaseInOutQuart(ShowY, HideY, Mathf.InverseLerp(0f, SlideInDuration, _timer));
                _rectTransform.anchoredPosition = position;

                if (_timer <= 0f)
                {
                    _currentTransition = Transition.Display;
                    _timer = DisplayDuration;
                }
                break;
            case Transition.Display:
                if (_timer <= 0f)
                {
                    _currentTransition = Transition.SlideOut;
                    _timer = SlideOutDuration;
                }
                break;
            case Transition.SlideOut:
                position.y = EasingFunction.EaseInOutQuart(HideY, ShowY, Mathf.InverseLerp(0f, SlideOutDuration, _timer));
                _rectTransform.anchoredPosition = position;

                if (_timer <= 0f) _current = null;
                break;
        }
    }

    public void Queue(Type type, string text) => _titles.Enqueue(new Title { Type = type, Text = text });

    enum Transition
    {
        SlideIn,
        Display,
        SlideOut,
    }

    public enum Type
    {
        Positive,
        Neutral,
        Negative,
    }

    public class Title
    {
        public Type Type;
        public string Text;
    }
}
