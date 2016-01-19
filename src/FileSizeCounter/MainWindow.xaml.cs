using System.Windows;
using FileSizeCounter.Model;
using Winforms = System.Windows.Forms;

namespace FileSizeCounter
{
  /// <summary>
  ///   Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    public MainWindow()
    {
      InitializeComponent();

      DataContext = new SizeCounterViewModel(this);
    }

    private void BrowseButton_OnClick(object sender, RoutedEventArgs e)
    {
      var folderBrowseDialog = new Winforms.FolderBrowserDialog
      {
        Description = Res.Resources.MainWindow_Info_SelectTargetDirectory
      };
      var result = folderBrowseDialog.ShowDialog();
      switch (result)
      {
        case Winforms.DialogResult.OK:
          InspectDirectoryTextBox.Text = folderBrowseDialog.SelectedPath;
          break;
      }
    }
  }
}