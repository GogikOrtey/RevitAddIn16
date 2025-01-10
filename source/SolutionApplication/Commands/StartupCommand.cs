using Autodesk.Revit.Attributes;
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

            return Result.Succeeded;
        }
    }
}