using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


namespace Diploma.Device
{    
    public enum ImageLoaderState
        {
            NoImagesAdded,
            AddImagesRequested,
            AddingImages,
            Done,
            Error
        }

    /// <summary>
    /// Adds images to the reference library at runtime.
    /// </summary>
    [RequireComponent(typeof(ARTrackedImageManager))]
    public class RefImageLoader : MonoBehaviour
    {
        public class ImageData
        {
            Texture2D m_Texture;

            public Texture2D texture
            {
                get => m_Texture;
                set => m_Texture = value;
            }
            string m_Name;

            public string name
            {
                get => m_Name;
                set => m_Name = value;
            }
            float m_Width;

            public float width
            {
                get => m_Width;
                set => m_Width = value;
            }

            public AddReferenceImageJobState jobState { get; set; }
        }

        public ImageData m_Image {get; set;}

        public  ImageLoaderState m_State;

    ARTrackedImageManager manager;

        public void Update()
        {
                switch (m_State)
                {
                    case ImageLoaderState.AddImagesRequested:
                    {
                        if (m_Image == null)
                        {
                            DeviceRead_Program.error = "No images to add.";
                            DeviceRead_Program.state = State.ERROR;
                            break;
                        }

                        var manager = GetComponent<ARTrackedImageManager>();
                        if (manager == null)
                        {
                            DeviceRead_Program.error = $"No {nameof(ARTrackedImageManager)} available.";
                            DeviceRead_Program.state = State.ERROR;
                            break;
                        }

                        if (manager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
                        {
                            try
                            {
                                m_Image.jobState = mutableLibrary.ScheduleAddImageWithValidationJob(m_Image.texture, m_Image.name, m_Image.width);
                                m_State = ImageLoaderState.AddingImages;
                            }
                            catch (InvalidOperationException e)
                            {
                                DeviceRead_Program.error = $"ScheduleAddImageJob threw exception: {e.Message}";
                                DeviceRead_Program.state = State.ERROR;
                            }
                        }
                        else
                        {
                            DeviceRead_Program.error = $"The reference image library is not mutable.";
                            DeviceRead_Program.state = State.ERROR;
                        }

                        break;
                    }
                    case ImageLoaderState.AddingImages:
                    {
                        var done = true;

                        if (!m_Image.jobState.jobHandle.IsCompleted)
                        {
                            done = false;
                            break;
                        }
                        

                        if (done)
                        {
                            m_State = ImageLoaderState.Done;
                            DeviceRead_Program.logging = "dinamikus referencia kép hozzáadás lefutott\n";
                            DeviceRead_Program.state = State.UPDATE_CONTROLPANEL;
                        }

                        break;
                    }
                }
            
        }
    }
}
