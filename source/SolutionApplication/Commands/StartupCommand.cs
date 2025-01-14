using System.Windows.Media.Media3D;
using System.Xaml;
using System.Xml.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.DependencyInjection;
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
            Autodesk.Revit.DB.Document doc = uidoc.Document;

            Reference myRef = uidoc.Selection.PickObject(ObjectType.Element, "Выберите элемент для вывода его Id");
            Element element = doc.GetElement(myRef);
            ElementId id = element.Id;
            string categoryElement = element.Category.Name.ToString();

            TaskDialog.Show("Информация", $"Id элемента: {id.ToString()}" + $"\n\nТип элемента: {categoryElement}");

            //Parameter widthParam = element.LookupParameter("Width");

            //TaskDialog.Show("Информация", $"Width элемента = {widthParam.ToString()}");

            //GetWidth(id);
            GetHeightWall(element);
            //GetDimensionsObject(element);

            return Result.Succeeded;
        }

        public double GetDimensionsObject(Element element)
        {
            // Получаем ограничивающую рамку объекта
            BoundingBoxXYZ boundingBox = element.get_BoundingBox(null);

            double Bwidth = 0;
            double Bheight = 0;
            double Blength = 0;

            if (boundingBox != null)
            {                
                XYZ minPoint = boundingBox.Min;
                XYZ maxPoint = boundingBox.Max;

                // И вычисляем значени по длине, ширине и высоте
                Bwidth = maxPoint.X - minPoint.X;
                Blength = maxPoint.Y - minPoint.Y;
                Bheight = maxPoint.Z - minPoint.Z;
            }

            Bwidth = ConvertFootToMeters(Bwidth);
            Bheight = ConvertFootToMeters(Bheight);
            Blength = ConvertFootToMeters(Blength);

            Bwidth = Bwidth * 10;
            Bheight = Bheight * 10;
            Blength = Blength * 10;

            TaskDialog.Show("Информация",
                "height (высота) = " + Bheight + " см" +
                "\nlength (длина) = " + Blength + " см" +
                "\nwidth (ширина/толщина) = " + Bwidth + " см");

            return 0;
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
                // length - длинна 

                length = element.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
                length = ConvertFootToMeters(length);

                // height - высота

                height = element.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM).AsDouble();
                height = ConvertFootToMeters(height);

                // width - ширина (глубина)

                Wall wall = (Wall)element;
                WallType wallType = wall.WallType; // Получаю тип текущей стены

                // И у типа стены уже есть параметр width
                width = wallType.get_Parameter(BuiltInParameter.WALL_ATTR_WIDTH_PARAM).AsDouble();
                width = ConvertFootToMeters(width);

                // Преобразуем в см
                length = length * 10;
                height = height * 10;
                width  = width  * 10;

                TaskDialog.Show("Информация",
                    "wall length = " + length + " см" +
                    "\nwall height = " + height + " см" +
                    "\nwall width = " + width + " см");
            }
            else if (categoryElement == "Windows")
            {
                // Буферные значения размеров в футах, для корректного рассчёта длинны
                double buferHeight = 0;
                double buferWidth = 0;

                // INSTANCE_HEAD_HEIGHT_PARAM
                // width = walltype
                // length = HOST_AREA_COMPUTED / height

                // length - длинна 

                length = element.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();
                //

                // height - высота

                height = element.get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM).AsDouble();
                buferHeight = height;
                height = ConvertFootToMeters(height);



                //// width - ширина (глубина)

                //// Проверяем, является ли элемент экземпляром семейства и категорией окон
                //if (element is FamilyInstance familyInstance && familyInstance.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Windows)
                //{
                //    // Получаем элемент-хозяина (host) для окна
                //    Element hostElement = familyInstance.Host;

                //    if (hostElement is Wall)
                //    {
                //        Wall hostWall = (Wall)hostElement;
                //        //TaskDialog.Show("Стенка-хозяин", "Id стенки-хозяина: " + hostWall.Id);

                //        WallType wallType = hostWall.WallType;
                //        width = wallType.get_Parameter(BuiltInParameter.WALL_ATTR_WIDTH_PARAM).AsDouble();
                //        buferWidth = width;
                //        width = ConvertFootToMeters(width);
                //    }
                //    else
                //    {
                //        TaskDialog.Show("Ошибка", "Окно не прикреплено к стене.");
                //    }
                //}
                //else
                //{
                //    TaskDialog.Show("Ошибка", "Выбранный элемент не является окном.");
                //}



                // Получаем ограничивающую рамку объекта
                BoundingBoxXYZ boundingBox = element.get_BoundingBox(null);

                double Blength = 0;
                double buferLength = 0;

                if (boundingBox != null)
                {
                    XYZ minPoint = boundingBox.Min;
                    XYZ maxPoint = boundingBox.Max;

                    // И вычисляем значени по длине, ширине и высоте
                    Blength = maxPoint.Y - minPoint.Y;
                }

                buferLength = Blength;
                Blength = ConvertFootToMeters(Blength);

                // width - ширина (глубина)

                width = element.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();
                width = width / buferHeight / buferLength;
                width = ConvertFootToMeters(width);

                //length = length / buferHeight / buferWidth;
                //length = ConvertFootToMeters(length);

                // Преобразуем в см
                Blength = Blength * 10;
                height = height * 10;
                width  = width  * 10;


                TaskDialog.Show("Информация",
                    "windows length = " + Blength + " см" +
                    "\nwindows height = " + height + " см" +
                    "\nwindows width = " + width + " см");
            }
            else if (categoryElement == "Doors")
            {
                // Буферные значения размеров в футах, для корректного рассчёта ширины
                double buferLength = 0;
                double buferHeight = 0;

                // Host -> Length: Definition: CURVE_ELEM_LENGTH 
                // INSTANCE_HEAD_HEIGHT_PARAM
                // ??

                // length - длинна 

                //length = element.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
                //length = ConvertFootToMeters(length);

                //length = element.get_Parameter(BuiltInParameter.DOOR_HEIGHT).AsDouble();
                //length = ConvertFootToMeters(length);

                //// Проверяем, является ли элемент экземпляром семейства и категорией дверей
                //if (element is FamilyInstance familyInstance && familyInstance.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Doors)
                //{
                //    // Получаем элемент-хозяина (host) для двери
                //    Element hostElement = familyInstance.Host;

                //    if (hostElement is Wall)
                //    {
                //        Wall hostWall = (Wall)hostElement;
                //        length = hostWall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
                //        buferLength = length;
                //        length = ConvertFootToMeters(length);
                //    }
                //    else
                //    {
                //        TaskDialog.Show("Ошибка", "Дверь не прикреплена к стене.");
                //    }
                //}
                //else
                //{
                //    TaskDialog.Show("Ошибка", "Выбранный элемент не является дверью.");
                //}

                // height - высота

                height = element.get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM).AsDouble();
                buferHeight = height;
                height = ConvertFootToMeters(height);

                // ! Корректно работает



                //width = element.get_Parameter(BuiltInParameter.DOOR_WIDTH).AsDouble();
                //width = ConvertFootToMeters(width);

                //if (element is FamilyInstance familyInstance && familyInstance.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Doors)
                //{
                //    // Получаем параметры ширины и высоты
                //    double widthParam = familyInstance.get_Parameter(BuiltInParameter.DOOR_WIDTH).AsDouble();
                //    double heightParam = familyInstance.get_Parameter(BuiltInParameter.DOOR_HEIGHT).AsDouble();

                //    width = widthParam + 1;
                //    length = heightParam;

                //    width = ConvertFootToMeters(width);
                //    length = ConvertFootToMeters(length);
                //}


                /*
                
                    Не понятно, как получить параметры длины и ширины у двери.

                    Или хотя бы одной величины, а другую я смогу вычислить через объём 

                */


                // Получаем ограничивающую рамку объекта
                BoundingBoxXYZ boundingBox = element.get_BoundingBox(null);

                //double Bwidth = 0;
                //double Bheight = 0;
                double Blength = 0;

                if (boundingBox != null)
                {
                    XYZ minPoint = boundingBox.Min;
                    XYZ maxPoint = boundingBox.Max;

                    // И вычисляем значени по длине, ширине и высоте
                    //Bwidth = maxPoint.X - minPoint.X;
                    Blength = maxPoint.Y - minPoint.Y;
                    //Bheight = maxPoint.Z - minPoint.Z;
                }

                buferLength = Blength;

                //Bwidth = ConvertFootToMeters(Bwidth);
                //Bheight = ConvertFootToMeters(Bheight);
                Blength = ConvertFootToMeters(Blength);

                // width - ширина (глубина)

                width = element.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();
                width = width / buferHeight / buferLength;
                width = ConvertFootToMeters(width);

                // Преобразуем в см
                length = length * 10;
                height = height * 10;
                width = width * 10;

                //Bwidth = Bwidth * 10;
                //Bheight = Bheight * 10;
                Blength = Blength * 10;

                TaskDialog.Show("Информация",
                    "doors length = " + Blength + " см" +
                    "\ndoors height = " + height + " см" +
                    "\ndoors width = " + width + " см");
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