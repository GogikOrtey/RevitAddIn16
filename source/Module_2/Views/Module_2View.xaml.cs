using Module_2.ViewModels;

namespace Module_2.Views
{
    public sealed partial class Module_2View
    {
        public Module_2View(Module_2ViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}