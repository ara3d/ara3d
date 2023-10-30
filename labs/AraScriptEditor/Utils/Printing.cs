using System;
using System.ComponentModel;
using System.Windows.Forms;
using ScintillaNET;

namespace Ara3D.ScriptEditor.Utils
{
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class Printing 
	{
        public Scintilla scintilla { get; set; }
        internal Printing(Scintilla scintilla)
		{
			_printDocument = new PrintDocument(scintilla);
		}

		internal bool ShouldSerialize()
		{
			return ShouldSerializePageSettings() || ShouldSerializePrintDocument();
		}

		public bool Print()
		{
			return Print(true);
		}

		public bool Print(bool showPrintDialog)
		{
            try
            {
                if (showPrintDialog)
                {
                    var pd = new PrintDialog();
                    pd.Document = _printDocument;
                    pd.UseEXDialog = true;
                    pd.AllowCurrentPage = true;
                    pd.AllowSelection = true;
                    pd.AllowSomePages = true;
                    pd.PrinterSettings = PageSettings.PrinterSettings;

                    if (pd.ShowDialog(scintilla) == DialogResult.OK)
                    {
                        _printDocument.PrinterSettings = pd.PrinterSettings;
                        _printDocument.Print();
                        return true;
                    }

                    return false;
                }

                _printDocument.Print();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                return false;
            }
        }

		public DialogResult PrintPreview()
		{
			var ppd = new PrintPreviewDialog();
			ppd.WindowState = FormWindowState.Maximized;

			ppd.Document = _printDocument;
			return ppd.ShowDialog();
		}

		public DialogResult PrintPreview(IWin32Window owner)
		{
			var ppd = new PrintPreviewDialog();
			ppd.WindowState = FormWindowState.Maximized;

			if (owner is Form)
				ppd.Icon = ((Form)owner).Icon;

			ppd.Document = _printDocument;
			return ppd.ShowDialog(owner);
		}

		public DialogResult ShowPageSetupDialog()
		{
			var psd = new PageSetupDialog();
			psd.PageSettings = PageSettings;
			psd.PrinterSettings = PageSettings.PrinterSettings;
			return psd.ShowDialog();
		}

		public DialogResult ShowPageSetupDialog(IWin32Window owner)
		{
			var psd = new PageSetupDialog();
			psd.AllowPrinter = true;
			psd.PageSettings = PageSettings;
			psd.PrinterSettings = PageSettings.PrinterSettings;

			return psd.ShowDialog(owner);
		}

		private PrintDocument _printDocument;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public PrintDocument PrintDocument
		{
			get
			{
				return _printDocument;
			}
			set
			{
				_printDocument = value;
			}
		}

		private bool ShouldSerializePrintDocument()
		{
			return _printDocument.ShouldSerialize();
		}


		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public PageSettings PageSettings
		{
			get
			{
				return _printDocument.DefaultPageSettings as PageSettings;
			}
			set
			{
				_printDocument.DefaultPageSettings = value;
			}
		}

		private bool ShouldSerializePageSettings()
		{
			return PageSettings.ShouldSerialize();
		}
	}
}
