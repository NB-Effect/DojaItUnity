using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using XDPaint.Core.PaintObject.Base;

namespace XDPaint.Tools.Image.Base
{
    public abstract class BasePaintToolSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool canPaintLines = true;
        [PaintToolSettings] public bool CanPaintLines
        {
            get => canPaintLines;
            set
            {
                canPaintLines = value; 
                OnPropertyChanged();
            }
        }

        private bool drawOnBrushMove = true;
        [PaintToolSettings] public bool DrawOnBrushMove
        {
            get => drawOnBrushMove;
            set
            {
                drawOnBrushMove = value;
                if (drawOnBrushMove && smoothing > 1)
                {
                    Debug.LogWarning("Disabling DrawOnBrushMove is recommended for getting smoother lines when drawing.");
                }
                
                OnPropertyChanged();
            }
        }
        
        [PaintToolSettings(Group = 1), SerializeField] internal int smoothing = 1;
        [PaintToolSettings(Group = 1), PaintToolConditional("Data.CanSmoothLines"), PaintToolRange(1, 10)] public int Smoothing
        {
            get => smoothing;
            set
            {
                if (!Data.CanSmoothLines)
                {
                    Debug.LogWarning($"Lines smoothing is not supported by the {Data.RenderComponents.ComponentType} component.");
                }
                
                smoothing = value;
                
                if (smoothing > 1 && drawOnBrushMove)
                {
                    DrawOnBrushMove = false;
                    Debug.LogWarning("DrawOnBrushMove has already been disabled to ensure smoother lines when drawing.");
                }

                OnPropertyChanged();
            }
        }
        
        private bool randomizePointsQuadsAngle;
        [PaintToolSettings] public bool RandomizePointsQuadsAngle
        {
            get => randomizePointsQuadsAngle;
            set
            {
                randomizePointsQuadsAngle = value;
                OnPropertyChanged();
            }
        }

        private bool randomizeLinesQuadsAngle;
        [PaintToolSettings] public bool RandomizeLinesQuadsAngle
        {
            get => randomizeLinesQuadsAngle;
            set
            {
                randomizeLinesQuadsAngle = value;
                OnPropertyChanged();
            }
        }
        
        protected readonly IPaintData Data;

        protected BasePaintToolSettings(IPaintData paintData)
        {
            Data = paintData;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) 
                return false;
            
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}