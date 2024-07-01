using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitLocationQuestStep : QuestStep
{
    private HashSet<string> locationsVisited = new HashSet<string>();
    private int locationsToVisit = 4;

    void OnEnable()
    {
        // ������������� �� ������� ��������� �����
        Location.LocationVisited += OnLocationVisited;
    }

    void OnDisable()
    {
        // ������������ �� ������� ��� ���������� ����������
        Location.LocationVisited -= OnLocationVisited;
    }

    void OnLocationVisited(string locationName)
    {
        // ���������, ���� ����� �� ���� ��� ��������
        if (!locationsVisited.Contains(locationName))
        {
            locationsVisited.Add(locationName);
            // ���������, �������� �� �� ������������ ���������� ���������� ����� ��� ���������� ������
            if (locationsVisited.Count >= locationsToVisit)
            {
                // ��������� ���� ������
                FinishQuestStep();
            }
        }
    }
}
