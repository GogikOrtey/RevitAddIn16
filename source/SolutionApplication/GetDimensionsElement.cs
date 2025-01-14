using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionApplication
{
    //internal class GetDimensionsElement
    //{ }



    // Абстрактный класс
    public abstract class BuildingElement
    {
        public abstract double GetWidth(Element element);
        public abstract double GetLength(Element element);
        public abstract double GetWeight(Element element);
    }

    // Класс для стены
    public class WallElement : BuildingElement
    {
        // Получение ширины стены
        public override double GetWidth(Element element)
        {
            return 1;
        }

        // Получение высоты стены
        public override double GetLength(Element element)
        {
            return 2;
        }

        // Получение длины стены
        public override double GetWeight(Element element)
        {
            return 3;
        }
    }

    // Класс для окна
    public class WindowElement : BuildingElement
    {
        // Получение ширины окна
        public override double GetWidth(Element element)
        {
            return 4;
        }

        // Получение высоты окна
        public override double GetLength(Element element)
        {
            return 5;
        }

        // Получение глубины окна
        public override double GetWeight(Element element)
        {
            return 6;
        }
    }

    // Класс для двери
    public class DoorElement : BuildingElement
    {
        // Получение ширины двери
        public override double GetWidth(Element element)
        {
            return 7;
        }

        // Получение высоты двери
        public override double GetLength(Element element)
        {
            return 8;
        }

        // Получение глубины двери
        public override double GetWeight(Element element)
        {
            return 9;
        }
    }


    // Основной метод, который определяет тип элемента и вызывает соответствующий метод
    public static class ElementProcessor
    {
        public static double GetWidth(Element element)
        {
            string categoryElement = element.Category.Name.ToString();
            BuildingElement buildingElement;

            switch (categoryElement)
            {
                case "Walls":
                    buildingElement = new WallElement();
                    break;
                case "Windows":
                    buildingElement = new WindowElement();
                    break;
                case "Doors":
                    buildingElement = new DoorElement();
                    break;
                default:
                    throw new ArgumentException("Неизвестный тип элемента");
            }

            return buildingElement.GetWidth(element);
        }

        public static double GetLength(Element element)
        {
            string categoryElement = element.Category.Name.ToString();
            BuildingElement buildingElement;

            switch (categoryElement)
            {
                case "Walls":
                    buildingElement = new WallElement();
                    break;
                case "Windows":
                    buildingElement = new WindowElement();
                    break;
                case "Doors":
                    buildingElement = new DoorElement();
                    break;
                default:
                    throw new ArgumentException("Неизвестный тип элемента");
            }

            return buildingElement.GetLength(element);
        }

        public static double GetWeight(Element element)
        {
            string categoryElement = element.Category.Name.ToString();
            BuildingElement buildingElement;

            switch (categoryElement)
            {
                case "Walls":
                    buildingElement = new WallElement();
                    break;
                case "Windows":
                    buildingElement = new WindowElement();
                    break;
                case "Doors":
                    buildingElement = new DoorElement();
                    break;
                default:
                    throw new ArgumentException("Неизвестный тип элемента");
            }

            return buildingElement.GetWeight(element);
        }
    }
}