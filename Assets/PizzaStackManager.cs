using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaStackManager : MonoBehaviour
{
    [SerializeField] List<GameObject> pizzaGameObjectsInStack;
    private int pizzaCountInStack;
    private int currentPizzaCount;

    private void Start()
    {
        pizzaCountInStack = pizzaGameObjectsInStack.Count;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pizza"))
        {
            if(currentPizzaCount + 1 < pizzaCountInStack)
            {
                currentPizzaCount++;
                UpdatePizzaStackActiveStatus();
                Destroy(other.gameObject);
            }
        }

        if(other.gameObject.CompareTag("Obstacle"))
        {
            if (currentPizzaCount - 1 >= 0)
            {
                currentPizzaCount--;
                UpdatePizzaStackDeactiveStatus();
                Destroy(other.gameObject);
            }
        }
    }

    void UpdatePizzaStackActiveStatus()
    {
        for(int i = 0; i < currentPizzaCount; i++)
        {
            pizzaGameObjectsInStack[i].SetActive(true);
        }
    }

    void UpdatePizzaStackDeactiveStatus()
    {
        for (int i = pizzaCountInStack - 1; i >= currentPizzaCount; i--)
        {
            pizzaGameObjectsInStack[i].SetActive(false);
        }
    }
}
