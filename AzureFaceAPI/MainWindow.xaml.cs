using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AzureFaceAPI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private FaceRecognitionService _faceService;    
    private string endpoint = "https://facerecogapp1.cognitiveservices.azure.com/";
    private string key = "F2w9RGjav3kXpFvGhn4uG3OJVw8apwKjzRu2KCFkuk5ozokvZadIJQQJ99BBAC3pKaRXJ3w3AAAKACOGqaI3";
    public MainWindow()
    {
        InitializeComponent();
        
    }

    private async void CompareFaces_Click(object sender, RoutedEventArgs e)
    {
        _faceService = new FaceRecognitionService(endpoint, key);

        string image1Path = "", image2Path = "";
        OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Image Files|*.jpg;*.png" };

        if (openFileDialog.ShowDialog() == true)
        {
            Image1.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(openFileDialog.FileName));
            image1Path = openFileDialog.FileName;
        }

        if (openFileDialog.ShowDialog() == true)
        {
            Image2.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(openFileDialog.FileName));
            image2Path = openFileDialog.FileName;
        }

        bool isSamePerson = await _faceService.CompareFacesAsync(openFileDialog.FileName, openFileDialog.FileName);
        tbFaceAPIResult.Text = isSamePerson ? "Match Found!" : "No Match!";

        FaceRecognitionServiceOpenCV service = new FaceRecognitionServiceOpenCV();
        bool isMatch = service.CompareFaces(image1Path, image2Path);
        tbOpenCvResult.Text = isMatch ? "Match Found!" : "No Match!";

    }
}