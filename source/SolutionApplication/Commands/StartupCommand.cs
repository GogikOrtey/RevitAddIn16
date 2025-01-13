using System.Xaml;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;

namespace SolutionApplication.Commands
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]


    // Этот модуль выводит информацию об объекте по нажатию на него мышкой
    public class StartupCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            Reference myRef = uidoc.Selection.PickObject(ObjectType.Element, "Выберите элемент для вывода его Id");
            Element element = doc.GetElement(myRef);
            ElementId id = element.Id;
            string categoryElement = element.Category.Name.ToString();

            TaskDialog.Show("Информация", $"Id элемента: {id.ToString()}" + $"\n\nТип элемента: {categoryElement}");

            //Parameter widthParam = element.LookupParameter("Width");

            //TaskDialog.Show("Информация", $"Width элемента = {widthParam.ToString()}");

            //GetWidth(id);
            GetHeightWall(element);

            return Result.Succeeded;
        }

        // Высота стены
        public double GetHeightWall(Element element)
        {
            //TaskDialog.Show("Информация", $"Id элемента: {idElement.ToString()}");

            ElementId id = element.Id;
            string categoryElement = element.Category.Name.ToString();

            double length = 0;
            double height = 0;            
            double width = 0;

            if (categoryElement == "Walls")
            {
                // wall length - длинна 

                length = element.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
                length = ConvertFootToMeters(length);

                // wall height - высота

                height = element.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM).AsDouble();
                height = ConvertFootToMeters(height);

                // wall width - ширина (глубина)

                Wall wall = (Wall)element;
                WallType wallType = wall.WallType; // Получаю тип текущей стены

                // И у типа стены уже есть параметр width
                width = wallType.get_Parameter(BuiltInParameter.WALL_ATTR_WIDTH_PARAM).AsDouble();

                width = ConvertFootToMeters(width);

                // Преобразуем в см
                length = length * 10;
                height = height * 10;
                width = width * 10;

                TaskDialog.Show("Информация",
                    "wall length = " + length + " см" +
                    "\nwall height = " + height + " см" +
                    "\nwall width = " + width + " см");
            }
            else if (categoryElement == "Windows")
            {
                double buferHeight = 0;
                double buferWidth = 0;

                // INSTANCE_HEAD_HEIGHT_PARAM
                // width = walltype
                // length = HOST_AREA_COMPUTED / height

                // wall length - длинна 

                length = element.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();
                //

                // wall height - высота

                height = element.get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM).AsDouble();
                buferHeight = height;
                height = ConvertFootToMeters(height);



                // wall width - ширина (глубина)

                // Проверяем, является ли элемент экземпляром семейства и категорией окон
                if (element is FamilyInstance familyInstance && familyInstance.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Windows)
                {
                    // Получаем элемент-хозяина (host) для окна
                    Element hostElement = familyInstance.Host;

                    if (hostElement is Wall)
                    {
                        Wall hostWall = (Wall)hostElement;
                        //TaskDialog.Show("Стенка-хозяин", "Id стенки-хозяина: " + hostWall.Id);

                        WallType wallType = hostWall.WallType;
                        width = wallType.get_Parameter(BuiltInParameter.WALL_ATTR_WIDTH_PARAM).AsDouble();
                        buferWidth = width;
                        width = ConvertFootToMeters(width);
                    }
                    else
                    {
                        TaskDialog.Show("Ошибка", "Окно не прикреплено к стене.");
                    }
                }
                else
                {
                    TaskDialog.Show("Ошибка", "Выбранный элемент не является окном.");
                }


                length = length / buferHeight / buferWidth;
                length = ConvertFootToMeters(length);

                // Преобразуем в см
                length = length * 10;
                height = height * 10;
                width = width * 10;


                TaskDialog.Show("Информация",
                    "wall length = " + length + " см" +
                    "\nwall height = " + height + " см" +
                    "\nwall width = " + width + " см");
            }
            else if (categoryElement == "Doors")
            {

            }
            else
            {
                TaskDialog.Show("Информация", "Неизвестный тип элемента!\n\ncategoryElement = " + categoryElement);
            }

            return height;
        }





      

















        // Переводит числовое значение из метров в футы
        public double ConvertMetersToFoot(double inputVal)
        {
            inputVal = inputVal / 3.048f;
            inputVal = Math.Round(inputVal, 2);

            return (inputVal);
        }

        // Переводит числовое значение из футов в метры
        public double ConvertFootToMeters(double inputVal)
        {
            inputVal = inputVal * 3.048f;
            inputVal = Math.Round(inputVal, 2);

            return (inputVal);
        }
    }
}