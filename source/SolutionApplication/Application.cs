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

            panel.AddPushButton<StartupCommand>("Получить размеры объекта")
                .SetImage("/SolutionApplication;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/SolutionApplication;component/Resources/Icons/RibbonIcon32.png");

            panel.AddPushButton<StartupCommand2>("Добавить комментарий к объекту")
                .SetImage("/SolutionApplication;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/SolutionApplication;component/Resources/Icons/RibbonIcon32.png");
        }
    }
}