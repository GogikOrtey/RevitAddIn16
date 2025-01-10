using Module_1.ViewModels;

namespace Module_1.Views
{
    public sealed partial class Module_1View
    {
        public Module_1View(Module_1ViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}