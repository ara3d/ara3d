﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf.SharpDX;
using Camera = HelixToolkit.Wpf.SharpDX.Camera;
using OrthographicCamera = HelixToolkit.Wpf.SharpDX.OrthographicCamera;
using PerspectiveCamera = HelixToolkit.Wpf.SharpDX.PerspectiveCamera;

namespace G3DViewer
{
    /// <summary>
    /// Base ViewModel for Demo Applications?
    /// </summary>
    public abstract class BaseViewModel : ObservableObject, IDisposable
    {
        public const string Orthographic = "Orthographic Camera";

        public const string Perspective = "Perspective Camera";

        private string cameraModel;

        private Camera camera;

        private string subTitle;

        private string title;

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                SetValue(ref title, value);
            }
        }

        public string SubTitle
        {
            get
            {
                return subTitle;
            }
            set
            {
                SetValue(ref subTitle, value);
            }
        }

        public List<string> CameraModelCollection { get; private set; }

        public string CameraModel
        {
            get
            {
                return cameraModel;
            }
            set
            {
                if (SetValue(ref cameraModel, value))
                {
                    OnCameraModelChanged();
                }
            }
        }

        public Camera Camera
        {
            get
            {
                return camera;
            }

            protected set
            {
                SetValue(ref camera, value);
                CameraModel = value is PerspectiveCamera
                                       ? Perspective
                                       : value is OrthographicCamera ? Orthographic : null;
            }
        }
        private IEffectsManager effectsManager;
        public IEffectsManager EffectsManager
        {
            get { return effectsManager; }
            protected set
            {
                SetValue(ref effectsManager, value);
            }
        }

        protected OrthographicCamera defaultOrthographicCamera = new OrthographicCamera { Position = new Point3D(0, 0, 5), LookDirection = new Vector3D(-0, -0, -5), UpDirection = new Vector3D(0, 1, 0), NearPlaneDistance = 1, FarPlaneDistance = 100 };

        protected PerspectiveCamera defaultPerspectiveCamera = new PerspectiveCamera { Position = new Point3D(0, 0, 5), LookDirection = new Vector3D(-0, -0, -5), UpDirection = new Vector3D(0, 1, 0), NearPlaneDistance = 0.5, FarPlaneDistance = 150 };

        public event EventHandler CameraModelChanged;

        protected BaseViewModel()
        {
            // camera models
            CameraModelCollection = new List<string>
            {
                Orthographic,
                Perspective,
            };

            // on camera changed callback
            CameraModelChanged += (s, e) =>
            {
                if (cameraModel == Orthographic)
                {
                    if (!(Camera is OrthographicCamera))
                        Camera = defaultOrthographicCamera;
                }
                else if (cameraModel == Perspective)
                {
                    if (!(Camera is PerspectiveCamera))
                        Camera = defaultPerspectiveCamera;
                }
                else
                {
                    throw new HelixToolkitException("Camera Model Error.");
                }
            };

            // default camera model
            CameraModel = Perspective;

            Title = "Demo (HelixToolkitDX)";
            SubTitle = "Default Base View Model";
        }

        protected virtual void OnCameraModelChanged()
        {
            var eh = CameraModelChanged;
            if (eh != null)
            {
                eh(this, new EventArgs());
            }
        }

        public static MemoryStream LoadFileToMemory(string filePath)
        {
            using (var file = new FileStream(filePath, FileMode.Open))
            {
                var memory = new MemoryStream();
                file.CopyTo(memory);
                return memory;
            }
        }

        #region IDisposable Support
        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                if (EffectsManager != null)
                {
                    var effectManager = EffectsManager as IDisposable;
                    Disposer.RemoveAndDispose(ref effectManager);
                }
                disposedValue = true;
                GC.SuppressFinalize(this);
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~BaseViewModel()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
