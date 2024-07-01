using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitLocationQuestStep : QuestStep
{
    private HashSet<string> locationsVisited = new HashSet<string>();
    private int locationsToVisit = 4;

    void OnEnable()
    {
        // Подписываемся на событие посещения точки
        Location.LocationVisited += OnLocationVisited;
    }

    void OnDisable()
    {
        // Отписываемся от события при отключении компонента
        Location.LocationVisited -= OnLocationVisited;
    }

    void OnLocationVisited(string locationName)
    {
        // Проверяем, если точка не была еще посещена
        if (!locationsVisited.Contains(locationName))
        {
            locationsVisited.Add(locationName);
            // Проверяем, достигли ли мы необходимого количества посещенных точек для завершения квеста
            if (locationsVisited.Count >= locationsToVisit)
            {
                // Завершаем этап квеста
                FinishQuestStep();
            }
        }
    }
}
