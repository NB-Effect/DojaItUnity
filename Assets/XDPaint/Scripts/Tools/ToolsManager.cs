﻿using System;
using System.Linq;
using UnityEngine;
using XDPaint.Core;
using XDPaint.Core.PaintObject.Data;
using XDPaint.Tools.Image;
using XDPaint.Tools.Image.Base;
using IDisposable = XDPaint.Core.IDisposable;

namespace XDPaint.Tools
{
    [Serializable]
    public class ToolsManager : IDisposable
    {
        public event Action<IPaintTool> OnToolChanged;

        private IPaintTool currentTool;
        public IPaintTool CurrentTool => currentTool;

        [SerializeField] private BrushTool brushTool;
        [SerializeField] private EraseTool eraseTool;
        [SerializeField] private BucketTool bucketTool;
        [SerializeField] private EyedropperTool eyedropperTool;
        [SerializeField] private BrushSamplerTool brushSamplerTool;
        [SerializeField] private CloneTool cloneTool;
        [SerializeField] private BlurTool blurTool;
        [SerializeField] private GaussianBlurTool gaussianBlurTool;
        [SerializeField] private GrayscaleTool grayscaleTool;
        
        private IPaintTool[] allTools;
        private PaintManager paintManager;
        private bool initialized;

        public ToolsManager(PaintTool paintTool, IPaintData paintData)
        {
            brushTool = new BrushTool(paintData);
            eraseTool = new EraseTool(paintData);
            bucketTool = new BucketTool(paintData);
            eyedropperTool = new EyedropperTool(paintData);
            brushSamplerTool = new BrushSamplerTool(paintData);
            cloneTool = new CloneTool(paintData);
            blurTool = new BlurTool(paintData);
            gaussianBlurTool = new GaussianBlurTool(paintData);
            grayscaleTool = new GrayscaleTool(paintData);
            allTools = new IPaintTool[]
            {
                brushTool, eraseTool, bucketTool, eyedropperTool, brushSamplerTool, cloneTool, blurTool, gaussianBlurTool, grayscaleTool
            };
            currentTool = allTools.First(x => x.Type == paintTool);
            currentTool.Enter();
        }

        public void Init(PaintManager thisPaintManager)
        {
            paintManager = thisPaintManager;
            paintManager.PaintObject.OnPointerHover += OnPointerHover;
            paintManager.PaintObject.OnPointerDown += OnPointerDown;
            paintManager.PaintObject.OnPointerPress += OnPointerMouse;
            paintManager.PaintObject.OnPointerUp += OnPointerUp;
            initialized = true;
        }

        public void DoDispose()
        {
            if (!initialized)
                return;
            
            paintManager.PaintObject.OnPointerHover -= OnPointerHover;
            paintManager.PaintObject.OnPointerDown -= OnPointerDown;
            paintManager.PaintObject.OnPointerPress -= OnPointerMouse;
            paintManager.PaintObject.OnPointerUp -= OnPointerUp;
            
            foreach (var tool in allTools)
            {
                if (currentTool == tool)
                {
                    tool.Exit();
                }
                tool.DoDispose();
            }
            allTools = null;
            initialized = false;
        }

        public void SetTool(PaintTool newTool)
        {
            foreach (var tool in allTools)
            {
                if (tool.Type == newTool)
                {
                    currentTool.Exit();
                    currentTool = tool;
                    OnToolChanged?.Invoke(currentTool);
                    currentTool.Enter();
                    break;
                }
            }
        }

        public IPaintTool GetTool(PaintTool tool)
        {
            return allTools.First(x => x.Type == tool);
        }

        private void OnPointerHover(PointerData pointerData)
        {
            currentTool.UpdateHover(pointerData);
        }

        private void OnPointerDown(PointerData pointerData)
        {
            currentTool.UpdateDown(pointerData);
        }
        
        private void OnPointerMouse(PointerData pointerData)
        {
            currentTool.UpdatePress(pointerData);
        }
        
        private void OnPointerUp(PointerUpData pointerUpData)
        {
            currentTool.UpdateUp(pointerUpData);
        }
    }
}