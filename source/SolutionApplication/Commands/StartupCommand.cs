using Autodesk.Revit.Attributes;
using Module_1.ViewModels;
using Module_1.Views;
using Nice3point.Revit.Toolkit.External;

namespace SolutionApplication.Commands
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class StartupCommand : ExternalCommand
    {
        public override void Execute()
        {
            var viewModel = new Module_1ViewModel();
            var view = new Module_1View(viewModel);
            view.ShowDialog();
        }
    }
}