using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] List<GameObject> pizzaGameObjectsInStack;
    private int pizzaCountInStack;
    private int currentPizzaCount;

    [SerializeField] GameObject emptyPizzaBox;
    [SerializeField] GameObject pizzaSlice;

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
                UpdatePizzaStackActiveStatus();
                Destroy(other.gameObject);

                ThrowPizzaBox();
            }
        }

        if(other.gameObject.CompareTag("Area"))
        {
            Area area = other.GetComponent<Area>();
            OPERATION OPR;
            OPR = area.GetOperation();
            int value = area.GetOperationValue();

            DoOperation(OPR, value);
        }
    }

    void ThrowPizzaBox()
    {
        GameObject temp = Instantiate(emptyPizzaBox, pizzaGameObjectsInStack[currentPizzaCount - 1].transform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody>().AddForce(ForceVector(), ForceMode.Impulse);
        temp.GetComponent<Rigidbody>().AddTorque(ForceVector() * 5, ForceMode.Impulse);

        temp = Instantiate(pizzaSlice, pizzaGameObjectsInStack[currentPizzaCount - 1].transform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody>().AddForce(ForceVector(), ForceMode.Impulse);
        temp.GetComponent<Rigidbody>().AddTorque(ForceVector() * 5, ForceMode.Impulse);

        temp = Instantiate(pizzaSlice, pizzaGameObjectsInStack[currentPizzaCount - 1].transform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody>().AddForce(ForceVector(), ForceMode.Impulse);
        temp.GetComponent<Rigidbody>().AddTorque(ForceVector() * 5, ForceMode.Impulse);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("FinishLine"))
        {
            Invoke("Finish", 0.25f);
        }
    }

    void Finish()
    {
        FindObjectOfType<GameManager>().FinishLevel();
        FindObjectOfType<PlayerStackPosition>().FinishLevel();
        animator.SetTrigger("levelFinished");
    }

    void DoOperation(OPERATION opr, int value)
    {
        if (opr == OPERATION.ADDITION) currentPizzaCount += value;
        else if (opr == OPERATION.SUBTRACTION) currentPizzaCount -= value;
        else if (opr == OPERATION.MULTIPLICATION) currentPizzaCount *= value;
        else
        {
            if (value == 0) return; Debug.Log(value);
            float temp = (float)currentPizzaCount / value; Debug.Log(temp);
            currentPizzaCount = Mathf.RoundToInt(temp); Debug.Log(currentPizzaCount);
        }

        if (currentPizzaCount < 0) currentPizzaCount = 0;

        UpdatePizzaStackActiveStatus();
    }

    void UpdatePizzaStackActiveStatus()
    {
        for(int i = 0; i < pizzaCountInStack; i++)
        {
            if(i < currentPizzaCount)
                pizzaGameObjectsInStack[i].SetActive(true);
            else
                pizzaGameObjectsInStack[i].SetActive(false);
        }
    }

    Vector3 ForceVector()
    {
        return new Vector3(RandomNumber() * MinusCounter(), RandomNumber(), -RandomNumber());
    }

    float RandomNumber()
    {
        return Random.Range(5, 15);
    }

    float MinusCounter()
    {
        int rand = Random.Range(0, 2);

        if (rand == 0)
            return 1;
        else
            return -1;
    }
}
