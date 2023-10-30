using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using ScintillaNET;

namespace Ara3D.ScriptEditor.Utils
{
	/// <summary>
	/// ScintillaNET derived class for handling printing of source code from a Scintilla control.
	/// </summary>
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class PrintDocument : System.Drawing.Printing.PrintDocument
	{
		private Scintilla m_oScintillaControl;

		private int m_iPosition;
		private int m_iPrintEnd;
		private int m_iCurrentPage;
        private const int SCI_SETPRINTMAGNIFICATION = 2146;
        private const int SCI_SETPRINTCOLOURMODE = 2148;

        //sacado de NativeStruct de version 4.0
        private struct PrintRectangle
        {
            /// <summary>
            /// Left X Bounds Coordinate
            /// </summary>
            public int Left;
            /// <summary>
            /// Top Y Bounds Coordinate
            /// </summary>
            public int Top;
            /// <summary>
            /// Right X Bounds Coordinate
            /// </summary>
            public int Right;
            /// <summary>
            /// Bottom Y Bounds Coordinate
            /// </summary>
            public int Bottom;

            public PrintRectangle(int iLeft, int iTop, int iRight, int iBottom)
            {
                Left = iLeft;
                Top = iTop;
                Right = iRight;
                Bottom = iBottom;
            }
        }
        private struct RangeToFormat
        {
            /// <summary>
            /// The HDC (device context) we print to
            /// </summary>
            public IntPtr hdc;
            /// <summary>
            /// The HDC we use for measuring (may be same as hdc)
            /// </summary>
            public IntPtr hdcTarget;
            /// <summary>
            /// Rectangle in which to print
            /// </summary>
            public PrintRectangle rc;
            /// <summary>
            /// Physically printable page size
            /// </summary>
            public PrintRectangle rcPage;
            /// <summary>
            /// Range of characters to print
            /// </summary>
            public CharacterRange chrg;
        }
        private struct CharacterRange
        {
            public int cpMin;
            public int cpMax;
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="oScintillaControl">Scintilla control being printed</param>
        public PrintDocument(Scintilla oScintillaControl)
		{
			m_oScintillaControl = oScintillaControl;
			DefaultPageSettings = new PageSettings();
		}

		
		internal bool ShouldSerialize()
		{
			return base.DocumentName != "document" || OriginAtMargins;
		}

		/// <summary>
		/// Method called after the Print method is called and before the first page of the document prints
		/// </summary>
		/// <param name="e">A PrintPageEventArgs that contains the event data</param>
		protected override void OnBeginPrint(PrintEventArgs e)
		{
			base.OnBeginPrint(e);

			m_iPosition = 0;
			m_iPrintEnd = m_oScintillaControl.TextLength;
			m_iCurrentPage = 1;
		}

		/// <summary>
		/// Method called when the last page of the document has printed
		/// </summary>
		/// <param name="e">A PrintPageEventArgs that contains the event data</param>
		protected override void OnEndPrint(PrintEventArgs e)
		{
			base.OnEndPrint(e);
		}

        
        /// <summary>
        /// Method called when printing a page
        /// </summary>
        /// <param name="e">A PrintPageEventArgs that contains the event data</param>
        protected override void OnPrintPage(PrintPageEventArgs e)
		{
			base.OnPrintPage(e);

			PageSettings oPageSettings = null;
			var oHeader = ((PageSettings)DefaultPageSettings).Header;
			var oFooter = ((PageSettings)DefaultPageSettings).Footer;
			var oPrintBounds = e.MarginBounds;
			var bIsPreview = this.PrintController.IsPreview;

			// When not in preview mode, adjust graphics to account for hard margin of the printer
			if (!bIsPreview)
			{
				e.Graphics.TranslateTransform(-e.PageSettings.HardMarginX, -e.PageSettings.HardMarginY);
			}

			// Get the header and footer provided if using Scintilla.Printing.PageSettings
			if (e.PageSettings is PageSettings)
			{
				oPageSettings = (PageSettings)e.PageSettings;

				oHeader = oPageSettings.Header;
				oFooter = oPageSettings.Footer;
                
                SetPrintMagnification(oPageSettings.FontMagnification);
                SetPrintColourMode((int)oPageSettings.ColorMode);
            }

			// Draw the header and footer and get remainder of page bounds
			oPrintBounds = DrawHeader(e.Graphics, oPrintBounds, oHeader);
			oPrintBounds = DrawFooter(e.Graphics, oPrintBounds, oFooter);

			// When not in preview mode, adjust page bounds to account for hard margin of the printer
			if (!bIsPreview)
			{
				oPrintBounds.Offset((int)-e.PageSettings.HardMarginX, (int)-e.PageSettings.HardMarginY);
			}
			DrawCurrentPage(e.Graphics, oPrintBounds);

			// Increment the page count and determine if there are more pages to be printed
			m_iCurrentPage++;
			e.HasMorePages = (m_iPosition < m_iPrintEnd);

            
		}

        private void SetPrintMagnification(int magnification)
        {
            m_oScintillaControl.DirectMessage(SCI_SETPRINTMAGNIFICATION, new IntPtr(magnification), IntPtr.Zero);
        }

        private void SetPrintColourMode(int mode)
        {
            m_oScintillaControl.DirectMessage(SCI_SETPRINTCOLOURMODE, new IntPtr(mode), IntPtr.Zero);
        }

        private Rectangle DrawHeader(Graphics oGraphics, Rectangle oBounds, PageInformation oHeader)
		{
			if (oHeader.Display)
			{
				var oHeaderBounds = new Rectangle(oBounds.Left, oBounds.Top, oBounds.Width, oHeader.Height);

				oHeader.Draw(oGraphics, oHeaderBounds, this.DocumentName, m_iCurrentPage);

				return new Rectangle(
					oBounds.Left, oBounds.Top + oHeaderBounds.Height + oHeader.Margin,
					oBounds.Width, oBounds.Height - oHeaderBounds.Height - oHeader.Margin
					);
			}
			else
			{
				return oBounds;
			}
		}

		private Rectangle DrawFooter(Graphics oGraphics, Rectangle oBounds, PageInformation oFooter)
		{
			if (oFooter.Display)
			{
				var iHeight = oFooter.Height;
				var oFooterBounds = new Rectangle(oBounds.Left, oBounds.Bottom - iHeight, oBounds.Width, iHeight);

				oFooter.Draw(oGraphics, oFooterBounds, this.DocumentName, m_iCurrentPage);

				return new Rectangle(
					oBounds.Left, oBounds.Top,
					oBounds.Width, oBounds.Height - oFooterBounds.Height - oFooter.Margin
					);
			}
			else
			{
				return oBounds;
			}
		}

		private void DrawCurrentPage(Graphics oGraphics, Rectangle oBounds)
		{
			Point[] oPoints = {
                new Point(oBounds.Left, oBounds.Top),
                new Point(oBounds.Right, oBounds.Bottom)
                };
			oGraphics.TransformPoints(CoordinateSpace.Device, CoordinateSpace.Page, oPoints);
            
			var oPrintRectangle = new PrintRectangle(oPoints[0].X, oPoints[0].Y, oPoints[1].X, oPoints[1].Y);

			var oRangeToFormat = new RangeToFormat();
			oRangeToFormat.hdc = oRangeToFormat.hdcTarget = oGraphics.GetHdc();
			oRangeToFormat.rc = oRangeToFormat.rcPage = oPrintRectangle;
			oRangeToFormat.chrg.cpMin = m_iPosition;
			oRangeToFormat.chrg.cpMax = m_iPrintEnd;

            m_iPosition = FormatRange(true, ref oRangeToFormat);

        }
        private int FormatRange(bool bDraw, ref RangeToFormat pfr)
        {
            var handle = GCHandle.Alloc(pfr, GCHandleType.Pinned);
            try
            {
                var pointer = handle.AddrOfPinnedObject();
                const int SCI_FORMATRANGE = 2151;

                return m_oScintillaControl.DirectMessage(SCI_FORMATRANGE, new IntPtr(bDraw ? 1 : 0), pointer).ToInt32(); ;
            }
            finally
            {
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            }
        }
        public new string DocumentName
		{
			get
			{
				return base.DocumentName;
			}
			set
            {
            	base.DocumentName = value;
            }
		}

		private bool ShouldSerializeDocumentName()
		{
			return DocumentName != "document";
		}

		private void ResetDocumentName()
		{
			DocumentName = "document";
		}

		public new bool OriginAtMargins
		{
			get
			{
				return base.OriginAtMargins;
			}
			set
            {
            	base.OriginAtMargins = value;
            }
		}

		private bool ShouldSerializeOriginAtMargins()
		{
			return OriginAtMargins;
		}

		private void ResetOriginAtMargins()
		{
			OriginAtMargins = false;
		}
    
	}

}
