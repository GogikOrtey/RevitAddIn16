using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace SolutionApplication.Commands
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]

    // Этот модуль выводит информацию об объекте по нажатию на него мышкой
    public class StartupCommand2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Autodesk.Revit.DB.Document doc = uidoc.Document;

            Reference myRef = uidoc.Selection.PickObject(ObjectType.Element, "Выберите элемент для вывода его Id");
            Element element = doc.GetElement(myRef);

            string idElement = element.Id.ToString();

            TaskDialog.Show("Информация", "id объекта = " + idElement);

            return Result.Succeeded;
        }






















    }
}