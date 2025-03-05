using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace AzureFaceAPI
{
    public class FaceRecognitionService
    {
        private readonly IFaceClient _faceClient;        
        public FaceRecognitionService(string endpoint, string subscriptionKey)
        {
            
            _faceClient = new FaceClient(new ApiKeyServiceClientCredentials(subscriptionKey))
            {
                Endpoint = endpoint
            };
        }

        public async Task<bool> CompareFacesAsync(string imagePath1, string imagePath2)
        {
            try
            {
                using (var stream1 = File.OpenRead(imagePath1))
                using (var stream2 = File.OpenRead(imagePath2))
                {
                    var faces1 = await _faceClient.Face.DetectWithStreamAsync(stream1,
                                 returnFaceId: true,
                                 returnFaceLandmarks: false,
                                 recognitionModel: RecognitionModel.Recognition04,
                                 detectionModel: DetectionModel.Detection01);

                    var faces2 = await _faceClient.Face.DetectWithStreamAsync(stream2,
                                returnFaceId: true,
                                recognitionModel: RecognitionModel.Recognition04,
                                detectionModel: DetectionModel.Detection01);
                    if (!faces1.Any() || !faces2.Any())
                        return false;

                    var verifyResult = await _faceClient.Face.VerifyFaceToFaceAsync(faces1[0].FaceId.Value, faces2[0].FaceId.Value);
                    return verifyResult.IsIdentical;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Face API Error:" + ex.Message);
                return false;
            }
            
        }
    }
}
