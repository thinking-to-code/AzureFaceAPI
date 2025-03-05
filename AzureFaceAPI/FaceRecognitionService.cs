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
        /*
         Here’s a structured comparison of **Azure Face API** and **OpenCV Sharp** in slide format:

---

### **Slide 1: Title Slide**  
**Title:**  
**Comparison Between Azure Face API and OpenCV Sharp**  

**Subtitle:**  
A Pros & Cons Analysis for Face Recognition  

---

### **Slide 2: Introduction**  
**Overview:**  
- Face recognition is widely used for authentication, security, and analysis.  
- Two popular approaches:  
  - **Azure Face API** (Cloud-based, AI-powered)  
  - **OpenCV Sharp** (On-Premise, Open-Source)  
- Let’s compare both solutions.  

---

### **Slide 3: Azure Face API - Pros**  
✅ **High Accuracy** – Pre-trained deep learning models provide superior recognition.  
✅ **Scalability** – Easily handles large-scale image processing via cloud services.  
✅ **Prebuilt Features** – Face detection, verification, emotion analysis, and age estimation.  
✅ **Easy Integration** – REST APIs allow quick integration with .NET and other platforms.  
✅ **Secure & Compliant** – Microsoft ensures GDPR & enterprise-grade security.  

---

### **Slide 4: Azure Face API - Cons**  
❌ **Requires Internet** – Cloud dependency may introduce latency.  
❌ **Costly for Large-Scale Use** – Pricing is based on API calls, which can be expensive over time.  
❌ **Limited Customization** – Users cannot modify the underlying model.  
❌ **Data Privacy Concerns** – Faces are processed in the cloud, which may not suit sensitive applications.  

---

### **Slide 5: OpenCV Sharp - Pros**  
✅ **Completely Free & Open Source** – No licensing costs.  
✅ **Works Offline** – Can run locally without an internet connection.  
✅ **Highly Customizable** – Modify and train models as needed.  
✅ **Fast Execution** – Performs well on local hardware with optimized algorithms.  
✅ **Supports Edge Computing** – Ideal for real-time processing on devices.  

---

### **Slide 6: OpenCV Sharp - Cons**  
❌ **Lower Accuracy** – Requires additional model training for better precision.  
❌ **Complex Setup** – Needs manual configuration and tuning.  
❌ **Lacks Prebuilt Features** – No emotion detection or age estimation like Azure.  
❌ **Hardware Dependent** – Performance varies based on local machine capabilities.  
❌ **No Cloud Scalability** – Not suitable for large-scale cloud-based applications.  

---

### **Slide 7: When to Choose Azure Face API?**  
**Ideal for:**  
✔ Enterprises needing high-accuracy face recognition.  
✔ Applications requiring cloud scalability and global access.  
✔ Businesses that prioritize ease of integration and prebuilt features.  
✔ Compliance-driven industries (finance, healthcare).  

---

### **Slide 8: When to Choose OpenCV Sharp?**  
**Ideal for:**  
✔ Edge computing and offline applications.  
✔ Cost-sensitive projects that need an open-source solution.  
✔ Developers who require deep model customization.  
✔ Real-time processing on local machines (e.g., CCTV systems).  

---

### **Slide 9: Conclusion**  
- **Azure Face API** → Best for **cloud-based, high-accuracy, and scalable applications**.  
- **OpenCV Sharp** → Best for **on-premise, cost-effective, and offline solutions**.  
- The choice depends on **business needs, budget, and technical requirements**.  

---

Would you like me to refine this into a PowerPoint presentation for you? 🚀
         */
    }
}
