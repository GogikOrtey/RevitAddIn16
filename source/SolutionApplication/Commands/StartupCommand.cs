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

                //width = element.LookupParameter("Width").AsDouble();
                // Definition: HOST_VOLUME_COMPUTED / 
                // Definition: WALL_ATTR_WIDTH_PARAM

                //width = element.get_Parameter(BuiltInParameter.WALL_ATTR_WIDTH_PARAM).AsDouble();

                Wall wall = (Wall)element;
                WallType wallType = wall.WallType; // Получаю тип стены

                // И у типа стены уже есть параметр width
                width = wallType.get_Parameter(BuiltInParameter.WALL_ATTR_WIDTH_PARAM).AsDouble();

                width = ConvertFootToMeters(width);


                TaskDialog.Show("Информация",
                    "wall length = " + length + " метров" +
                    "\nwall height = " + height + " метров" +
                    "\nwall width = " + width + " метров");
            }
            else
            {
                TaskDialog.Show("Информация", "Элемент НЕ является стеной!\n\ncategoryElement = " + categoryElement);
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