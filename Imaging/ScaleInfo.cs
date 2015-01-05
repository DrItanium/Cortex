//-----------------------------------------------------------------------------
//Cortex
//Copyright (c) 2010-2015, Joshua Scoggins 
//All rights reserved.
//
//Redistribution and use in source and binary forms, with or without
//modification, are permitted provided that the following conditions are met:
//    * Redistributions of source code must retain the above copyright
//      notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright
//      notice, this list of conditions and the following disclaimer in the
//      documentation and/or other materials provided with the distribution.
//    * Neither the name of Cortex nor the
//      names of its contributors may be used to endorse or promote products
//      derived from this software without specific prior written permission.
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//DISCLAIMED. IN NO EVENT SHALL Joshua Scoggins BE LIABLE FOR ANY
//DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//-----------------------------------------------------------------------------
using System;

namespace Cortex.Imaging
{
  public class ScaleInfo
  {
    private float factor, wFac, hFac;
    private int srcWidth, srcHeight, rsltWidth, rsltHeight;
    private long srcSize, rsltSize;
    public long SourceImagePixelCount { get { return srcSize; } }
    public long ResultImagePixelCount { get { return rsltSize; } }
    public int SourceWidth { get { return srcWidth; } }
    public int SourceHeight { get { return srcHeight; } }
    public int ResultWidth { get { return rsltWidth; } }
    public int ResultHeight { get { return rsltHeight; } }
    public float WidthScalingFactor { get { return wFac; } }
    public float HeightScalingFactor { get { return hFac; } }
    public float ScalingFactor { get { return factor; } }
    public bool IsZooming { get { return factor > 1.0f; } }
    public bool IsShrinking { get { return factor < 1.0f; } }
    public ScaleInfo(int sWidth, int sHeight, int rWidth, int rHeight)
    {
      srcWidth = sWidth;
      srcHeight = sHeight;
      rsltWidth = rWidth;
      rsltHeight = rHeight;
      srcSize = (long)sWidth * (long)sHeight;
      rsltSize = (long)rWidth * (long)rHeight;
      float fsWidth = sWidth;
      float fsHeight = sHeight;
      float frWidth = rWidth;
      float frHeight = rHeight;
      factor = (float)rsltSize / (float)srcSize;
      wFac = (float)frWidth / (float)fsWidth;
      hFac = (float)frHeight / (float)fsHeight;
    }
  }
}
