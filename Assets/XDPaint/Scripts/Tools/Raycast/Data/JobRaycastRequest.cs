using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using XDPaint.Core;

namespace XDPaint.Tools.Raycast.Data
{
    public class JobRaycastRequest : IRaycastRequest
    {
        public ulong RequestId;
        public JobHandle JobHandle;
        public NativeArray<TriangleData> InputNativeArray;
        public NativeArray<RaycastTriangleData> OutputNativeArray;

        public IPaintManager Sender { get; set; }
        public int FingerId { get; set; }
        public Vector3 PointerPosition { get; set; }
        public bool IsDisposed { get; private set; }
        
        public void DoDispose()
        {
            if (!JobHandle.IsCompleted)
            {
                JobHandle.Complete();
            }
            
            if (InputNativeArray.IsCreated)
            {
                try
                {
                    InputNativeArray.Dispose();
                }
                catch
                {
                    // ignored
                }
            }
            
            if (OutputNativeArray.IsCreated)
            {
                try
                {
                    OutputNativeArray.Dispose();
                }
                catch
                {
                    // ignored
                }
            }

            IsDisposed = true;
        }
    }
}