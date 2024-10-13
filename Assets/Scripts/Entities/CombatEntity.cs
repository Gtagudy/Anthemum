using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatEntity : MonoBehaviour
{
    [SerializeField] EntitySO entity;
    public UnityEvent HealthChange;

    // Start is called before the first frame update
    void Awake()
    {
        entity = GetComponent<EntitySO>();
    }

    public void ChangeHealth()
    {
        HealthChange.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
