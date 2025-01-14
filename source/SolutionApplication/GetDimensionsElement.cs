using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SolutionApplication
{
    // Абстрактный класс
    public abstract class BuildingElement
    {
        public abstract double GetLength(Element element);
        public abstract double GetHeight(Element element);
        public abstract double GetWidth(Element element);

        // Переводит числовое значение из метров в футы
        public static double ConvertMetersToFoot(double inputVal)
        {
            inputVal = inputVal / 3.048f;
            inputVal = Math.Round(inputVal, 2);

            return (inputVal);
        }

        // Переводит числовое значение из футов в метры
        public static double ConvertFootToMeters(double inputVal)
        {
            inputVal = inputVal * 3.048f;
            inputVal = Math.Round(inputVal, 2);

            return (inputVal);
        }
    }

    // Класс для стены
    public class WallElement : BuildingElement
    {
        // Получение ширины стены
        public override double GetLength(Element element)
        {
            double length = element.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
            length = ConvertFootToMeters(length);

            return length;
        }

        // Получение высоты стены
        public override double GetHeight(Element element)
        {
            double height = element.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM).AsDouble();
            height = ConvertFootToMeters(height);

            return height;
        }

        // Получение длины стены
        public override double GetWidth(Element element)
        {
            Wall wall = (Wall)element;
            WallType wallType = wall.WallType; // Получаю тип текущей стены

            // И у типа стены уже есть параметр width
            double width = wallType.get_Parameter(BuiltInParameter.WALL_ATTR_WIDTH_PARAM).AsDouble();
            width = ConvertFootToMeters(width);

            return width;
        }
    }

    // Класс для окна
    public class WindowElement : BuildingElement
    {
        // Получение ширины окна
        public override double GetLength(Element element)
        {
            // Получаем ограничивающую рамку объекта
            BoundingBoxXYZ boundingBox = element.get_BoundingBox(null);

            double Blength = 0;

            if (boundingBox != null)
            {
                XYZ minPoint = boundingBox.Min;
                XYZ maxPoint = boundingBox.Max;

                // И вычисляем значени по длине, ширине и высоте
                Blength = maxPoint.Y - minPoint.Y;
            }

            Blength = ConvertFootToMeters(Blength);

            return Blength;
        }

        // Получение высоты окна
        public override double GetHeight(Element element)
        {
            double height = element.get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM).AsDouble();
            height = ConvertFootToMeters(height);

            return height;
        }

        // Получение глубины окна
        public override double GetWidth(Element element)
        {
            double buferHeight = ConvertMetersToFoot(GetHeight(element));
            double buferLength = ConvertMetersToFoot(GetLength(element));

            double width = element.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();
            width = width / buferHeight / buferLength;
            width = ConvertFootToMeters(width);

            return width;
        }
    }

    // Класс для двери
    public class DoorElement : BuildingElement
    {
        // Получение ширины двери
        public override double GetLength(Element element)
        {
            // Получаем ограничивающую рамку объекта
            BoundingBoxXYZ boundingBox = element.get_BoundingBox(null);

            double Blength = 0;

            if (boundingBox != null)
            {
                XYZ minPoint = boundingBox.Min;
                XYZ maxPoint = boundingBox.Max;

                Blength = maxPoint.Y - minPoint.Y;
            }

            Blength = ConvertFootToMeters(Blength);

            return Blength;
        }

        // Получение высоты двери
        public override double GetHeight(Element element)
        {
            double height = element.get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM).AsDouble();
            height = ConvertFootToMeters(height);

            return height;
        }

        // Получение глубины двери
        public override double GetWidth(Element element)
        {
            double buferHeight = ConvertMetersToFoot(GetHeight(element));
            double buferLength = ConvertMetersToFoot(GetLength(element));

            double width = element.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();
            width = width / buferHeight / buferLength;
            width = ConvertFootToMeters(width);

            return width;
        }
    }


    // Основной метод, который определяет тип элемента и вызывает соответствующий метод
    public static class ElementProcessor
    {
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
                    throw new ArgumentException("Неизвестный тип элемента!");
            }

            return buildingElement.GetLength(element);
        }

        public static double GetHeight(Element element)
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
                    throw new ArgumentException("Неизвестный тип элемента!");
            }

            return buildingElement.GetHeight(element);
        }

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
                    throw new ArgumentException("Неизвестный тип элемента!");
            }

            return buildingElement.GetWidth(element);
        }
    }
}