using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TaskManager : MonoBehaviour
{
    public int countHouses;

    public int countTasks;

    [SerializeField] private TMP_Text _taskText;

    private GameObject _currentHouse;
    
    private string _task;

    private bool _showTask = false; 

    private GameObject[] _houses;
    private GameObject[] _usedHouses;

    private void Start()
    {
        _houses = GameObject.FindGameObjectsWithTag("House");
        _usedHouses = new GameObject[_houses.Length];

        loadTask();
        _taskText.gameObject.SetActive(_showTask);
    }

    private void loadTask()
    {
        _task = ""; // Очищаем задание перед началом загрузки новых задач
        for (int i = 0; i < countTasks; i++)
        {
            if (i > 0)
                _task += "\n\n";

            #region choose home
            int homeID = 0;
            if (_usedHouses != null)
            {
                for (int j = 0; j < _usedHouses.Length; j++)
                {
                    while (_currentHouse == _usedHouses[j])
                    {
                        homeID = Random.Range(0, countHouses);
                        _currentHouse = _houses[homeID];
                    }
                }
            }
            else
            {
                homeID = Random.Range(0, countHouses);
                _currentHouse = _houses[homeID];
            }

            _currentHouse = _houses[homeID];
            _usedHouses[homeID] = _currentHouse;
            #endregion

            string[] taskFiles = Directory.GetFiles("Assets/Tasks/usual", "*.txt");
            int numberTask = Random.Range(0, taskFiles.Length);
            _task += $"{File.ReadAllText(taskFiles[numberTask])} {_currentHouse.name}"; // Изменил здесь, чтобы получить имя дома, а не объект
        }
        _taskText.text = _task;
    }

    public void ShowTask()
    {
        _showTask = !_showTask;
        _taskText.gameObject.SetActive(_showTask);
    }


}
