using FileSizeCounter.Model;

namespace FileSizeCounter
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    public MainWindow()
    {
      InitializeComponent();

      DataContext = new SizeCounterViewModel();
    }
  }
}
