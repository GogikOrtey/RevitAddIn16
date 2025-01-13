using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionApplication
{
    //internal class GetDimensionsElement
    //{

    //}

    // Абстрактный класс
    public abstract class BuildingElement
    {
        public abstract double GetWidth();
        public abstract double GetLength();
        public abstract double GetWeight();
    }

    // Класс для стены
    public class Wall : BuildingElement
    {
        // Получение ширины стены
        public override double GetWidth()
        {
            
        }

        // Получение высоты стены
        public override double GetLength()
        {
            
        }

        // Получение длины стены
        public override double GetWeight()
        {
            
        }
    }

    // Класс для окна
    public class Window : BuildingElement
    {
        // Получение ширины окна
        public override double GetWidth()
        {
            
        }

        // Получение высоты окна
        public override double GetLength()
        {
            
        }

        // Получение глубины окна
        public override double GetWeight()
        {
            
        }
    }

    // Класс для двери
    public class Door : BuildingElement
    {
        // Получение ширины двери
        public override double GetWidth()
        {
            
        }

        // Получение высоты двери
        public override double GetLength()
        {
            
        }

        // Получение глубины двери
        public override double GetWeight()
        {
            
        }
    }
}
