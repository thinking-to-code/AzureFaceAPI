using DlibDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFaceAPI
{
    public class FaceRecognitionServiceOpenCV
    {
        public bool CompareFaces(string imagePath1, string imagePath2)
        {
            using var faceDetector = Dlib.GetFrontalFaceDetector();

            var img1 = Dlib.LoadImage<RgbPixel>(imagePath1);
            var img2 = Dlib.LoadImage<RgbPixel>(imagePath2);

            var faces1 = faceDetector.Operator(img1);
            var faces2 = faceDetector.Operator(img2);

            return faces1.Length > 0 && faces2.Length > 0; // Simplified
        }
    }
}
