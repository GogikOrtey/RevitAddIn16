﻿using System.Xml.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
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

            // Создаем транзакцию, для добавления комментария к объекту
            using (Transaction transaction = new Transaction(doc, "Add Comment"))
            {
                transaction.Start();
                try
                {
                    element.AddComment("Комментарий 1");

                    transaction.Commit();
                }
                catch
                {
                    TaskDialog.Show("Информация", "При добавлении комментария к объекту с id = " + idElement + ", произошла ошибка!\n\nКомментарий не добавлен");
                    transaction.RollBack();
                    throw;
                }
            }

            return Result.Succeeded;
        }
    }

    // Создаём метод расширения для встроенного класса Element
    public static class ElementExtensions
    {
        public static void AddComment(this Element element, string comment)
        {
            // Параметр "Комментарии" имеет встроенное имя "ALL_MODEL_INSTANCE_COMMENTS".
            Parameter commentParameter = element.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
            if (commentParameter != null && !commentParameter.IsReadOnly)
            {
                // Установим значение параметра "Комментарии".
                commentParameter.Set(comment);
            }
        }
    }
}