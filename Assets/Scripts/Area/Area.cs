using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Area : MonoBehaviour
{
    public enum OPERATION { ADDITION, SUBTRACTION, MULTIPLICATION, DIVISION}
    [SerializeField] float operationValue;
    [SerializeField] OPERATION operation;
    [SerializeField] TextMeshProUGUI operationText;

    // Start is called before the first frame update
    void Start()
    {
        string op = GetOperationSign();
        operationText.text = op + operationValue.ToString();
    }

    string GetOperationSign()
    {
        if (operation == OPERATION.ADDITION) return "+";
        else if (operation == OPERATION.SUBTRACTION) return "-";
        else if (operation == OPERATION.MULTIPLICATION) return "×";
        else return "÷";
    }

    public float GetOperationValue()
    {
        return operationValue;
    }

    public string GetOperation()
    {
        return operation.ToString();
    }
}
