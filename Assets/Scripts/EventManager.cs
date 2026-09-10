using System;
using System.Collections.Generic;
using UnityEngine;

public enum Events
{
    NoHorseColl,
    NoWalls,
    DebufHorse
}

public class EventManager : MonoBehaviour
{
    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("EventManager");
                _instance = obj.AddComponent<EventManager>();
            }

            return _instance;
        }
        private set => _instance = value;
    }

    private static Dictionary<Events, Action> _eventsFunc = new Dictionary<Events, Action>()
    {
        {Events.NoHorseColl, NoHorseColls},
        {Events.NoWalls, NoWalls},
        {Events.DebufHorse, StunHorse}
    };

    private float _timeBetweenEvent = 1f;

    private static float _eventChance = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    float _timer = 0f;
    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _timeBetweenEvent)
        {
            _timer = 0f;
            if (ShouldInvokeEvent()) GetRandomEvent();
        }
    }

    private static bool ShouldInvokeEvent()
    {
        float chance = UnityEngine.Random.value;

        return chance <= _eventChance;
    }

    private static void GetRandomEvent()
    {
        int index = UnityEngine.Random.Range(0, _eventsFunc.Count - 1);
        Events randomEvent = (Events)index;

        _eventsFunc[randomEvent]();
    }

    public static void NoHorseColls()
    {
        Debug.Log("Horses don't have collisions");
    }

    private static void NoWalls()
    {
        Debug.Log("No walls");
    }

    private static void StunHorse()
    {
        Debug.Log("A horse is stunned");
    }
}
