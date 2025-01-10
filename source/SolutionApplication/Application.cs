using Nice3point.Revit.Toolkit.External;
using SolutionApplication.Commands;

namespace SolutionApplication
{
    [UsedImplicitly]
    public class Application : ExternalApplication
    {
        public override void OnStartup()
        {
            CreateRibbon();
        }

        private void CreateRibbon()
        {
            var panel = Application.CreatePanel("Commands", "RevitAddIn16");

            panel.AddPushButton<StartupCommand>("Кнопка 1")
                .SetImage("/SolutionApplication;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/SolutionApplication;component/Resources/Icons/RibbonIcon32.png");
        }
    }
}