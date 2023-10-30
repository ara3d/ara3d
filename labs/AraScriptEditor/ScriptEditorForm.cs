using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Ara3D.ScriptEditor.Utils;
using ScintillaNET;

namespace Ara3D.ScriptEditor
{
    /// <summary>
    /// TODO: Menu hot keys don't seem to be supported automatically. 
    /// TODO: initialize with "new" or something else. 
    /// TODO: an output window for console logging would be nice.
    /// TODO: load the files that were opened last time.
    /// </summary>
    public partial class ScriptEditorForm : Form
    {
		public ScriptEditorForm()
        {
            // Note: I had some problems art one point because the library would generate a DLL 
            // and put it somewhere, and try to link it. This did not work well when using
            // this as a library, so I made it its own process. Here are some notes about that problem.
            // https://github.com/jacobslusser/ScintillaNET/issues/230
            // https://github.com/jacobslusser/ScintillaNET/wiki/Using-a-Custom-SciLexer.dll
            //Scintilla.SetModulePath(@"C:\Program Files\Autodesk\3ds Max 2019\SciLexer.dll");
            InitializeComponent();
        }

        Scintilla TextArea;
        string FileName;

        public IEditorClientCallback Client => EditorService.Callback;

        void MainForm_Load(object sender, EventArgs e)
        {
            //openFileDialog.InitialDirectory = initialDir;
            //saveFileDialog.InitialDirectory = initialDir;

            // CREATE CONTROL
            TextArea = new Scintilla();
            TextPanel.Controls.Add(TextArea);

            // BASIC CONFIG
            TextArea.Dock = DockStyle.Fill;
            TextArea.TextChanged += (this.OnTextChanged);

            // INITIAL VIEW CONFIG
            TextArea.WrapMode = WrapMode.None;
            TextArea.IndentationGuides = IndentView.LookBoth;

            // STYLING
            InitColors();
            InitSyntaxColoring();

            // NUMBER MARGIN
            InitNumberMargin();

            // BOOKMARK MARGIN
            InitBookmarkMargin();

            // CODE FOLDING MARGIN
            InitCodeFolding();

            // DRAG DROP
            InitDragDropFile();

            // DEFAULT FILE
            //LoadDataFromFile("../../MainForm.cs");

            // INIT HOTKEYS
            InitHotkeys();

            // TODO: this would be nice, but I think it works because the process 
            // isn't ready when first opened. 
            //OnNew();
        }

        void InitColors()
        {
            TextArea.CaretForeColor = Color.BlanchedAlmond;
            TextArea.SetSelectionBackColor(true, IntToColor(0x114D9C));
        }

        void InitHotkeys()
        {
            // register the hotkeys with the form
            HotKeyManager.AddHotKey(this, OpenSearch, Keys.F, true);
            HotKeyManager.AddHotKey(this, OpenFindDialog, Keys.F, true, false, true);
            HotKeyManager.AddHotKey(this, OpenReplaceDialog, Keys.R, true);
            HotKeyManager.AddHotKey(this, OpenReplaceDialog, Keys.H, true);
            HotKeyManager.AddHotKey(this, Uppercase, Keys.U, true);
            HotKeyManager.AddHotKey(this, Lowercase, Keys.L, true);
            HotKeyManager.AddHotKey(this, ZoomIn, Keys.Oemplus, true);
            HotKeyManager.AddHotKey(this, ZoomOut, Keys.OemMinus, true);
            HotKeyManager.AddHotKey(this, ZoomDefault, Keys.D0, true);
            HotKeyManager.AddHotKey(this, CloseSearch, Keys.Escape);

            // remove conflicting hotkeys from scintilla
            TextArea.ClearCmdKey(Keys.Control | Keys.F);
            TextArea.ClearCmdKey(Keys.Control | Keys.R);
            TextArea.ClearCmdKey(Keys.Control | Keys.H);
            TextArea.ClearCmdKey(Keys.Control | Keys.L);
            TextArea.ClearCmdKey(Keys.Control | Keys.U);

            // Prevent Scinitilla from rendering certain keys 
            TextArea.KeyPress += (object sender, KeyPressEventArgs e) =>
            {
                if (e.KeyChar < 32)
                {
                    // Prevent control characters from getting inserted into the text buffer
                    e.Handled = true;
                }
            };
        }

        void InitSyntaxColoring()
        {
            // Configure the default style
            TextArea.StyleResetDefault();
            TextArea.Styles[Style.Default].Font = "Consolas";
            TextArea.Styles[Style.Default].Size = 10;
            TextArea.Styles[Style.Default].BackColor = IntToColor(0x212121);
            TextArea.Styles[Style.Default].ForeColor = IntToColor(0xFFFFFF);
            TextArea.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            TextArea.Styles[Style.Cpp.Identifier].ForeColor = IntToColor(0xD0DAE2);
            TextArea.Styles[Style.Cpp.Comment].ForeColor = IntToColor(0xBD758B);
            TextArea.Styles[Style.Cpp.CommentLine].ForeColor = IntToColor(0x40BF57);
            TextArea.Styles[Style.Cpp.CommentDoc].ForeColor = IntToColor(0x2FAE35);
            TextArea.Styles[Style.Cpp.Number].ForeColor = IntToColor(0xFFFF00);
            TextArea.Styles[Style.Cpp.String].ForeColor = IntToColor(0xFFFF00);
            TextArea.Styles[Style.Cpp.Character].ForeColor = IntToColor(0xE95454);
            TextArea.Styles[Style.Cpp.Preprocessor].ForeColor = IntToColor(0x8AAFEE);
            TextArea.Styles[Style.Cpp.Operator].ForeColor = IntToColor(0xE0E0E0);
            TextArea.Styles[Style.Cpp.Regex].ForeColor = IntToColor(0xff00ff);
            TextArea.Styles[Style.Cpp.CommentLineDoc].ForeColor = IntToColor(0x77A7DB);
            TextArea.Styles[Style.Cpp.Word].ForeColor = IntToColor(0x48A8EE);
            TextArea.Styles[Style.Cpp.Word2].ForeColor = IntToColor(0xF98906);
            TextArea.Styles[Style.Cpp.CommentDocKeyword].ForeColor = IntToColor(0xB3D991);
            TextArea.Styles[Style.Cpp.CommentDocKeywordError].ForeColor = IntToColor(0xFF0000);
            TextArea.Styles[Style.Cpp.GlobalClass].ForeColor = IntToColor(0x48A8EE);

            TextArea.Lexer = Lexer.Cpp;

            TextArea.SetKeywords(0, "class extends implements import interface new case do while else if for in switch throw get set function var try catch finally while with default break continue delete return each const namespace package include use is as instanceof typeof author copy default deprecated eventType example exampleText exception haxe inheritDoc internal link mtasc mxmlc param private return see serial serialData serialField since throws usage version langversion playerversion productversion dynamic private public partial static intrinsic internal native override protected AS3 final super this arguments null Infinity NaN undefined true false abstract as base bool break by byte case catch char checked class const continue decimal default delegate do double descending explicit event extern else enum false finally fixed float for foreach from goto group if implicit in int interface internal into is lock long new null namespace object operator out override orderby params private protected public readonly ref return switch struct sbyte sealed short sizeof stackalloc static string select this throw true try typeof uint ulong unchecked unsafe ushort using var virtual volatile void while where yield");
            TextArea.SetKeywords(1, "void Null ArgumentError arguments Array Boolean Class Date DefinitionError Error EvalError Function int Math Namespace Number Object RangeError ReferenceError RegExp SecurityError String SyntaxError TypeError uint XML XMLList Boolean Byte Char DateTime Decimal Double Int16 Int32 Int64 IntPtr SByte Single UInt16 UInt32 UInt64 UIntPtr Void Path File System Windows Forms ScintillaNET");

        }

        void OnTextChanged(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// the background color of the text area
        /// </summary>
        const int BACK_COLOR = 0x2A211C;

        /// <summary>
        /// default text color of the text area
        /// </summary>
        const int FORE_COLOR = 0xB7B7B7;

        /// <summary>
        /// change this to whatever margin you want the line numbers to show in
        /// </summary>
        const int NUMBER_MARGIN = 1;

        /// <summary>
        /// change this to whatever margin you want the bookmarks/breakpoints to show in
        /// </summary>
        const int BOOKMARK_MARGIN = 2;
        const int BOOKMARK_MARKER = 2;

        /// <summary>
        /// change this to whatever margin you want the code folding tree (+/-) to show in
        /// </summary>
        const int FOLDING_MARGIN = 3;

        /// <summary>
        /// set this true to show circular buttons for code folding (the [+] and [-] buttons on the margin)
        /// </summary>
        const bool CODEFOLDING_CIRCULAR = true;

        void InitNumberMargin()
        {

            TextArea.Styles[Style.LineNumber].BackColor = IntToColor(BACK_COLOR);
            TextArea.Styles[Style.LineNumber].ForeColor = IntToColor(FORE_COLOR);
            TextArea.Styles[Style.IndentGuide].ForeColor = IntToColor(FORE_COLOR);
            TextArea.Styles[Style.IndentGuide].BackColor = IntToColor(BACK_COLOR);

            var nums = TextArea.Margins[NUMBER_MARGIN];
            nums.Width = 30;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;

            TextArea.MarginClick += TextArea_MarginClick;
        }

        void InitBookmarkMargin()
        {

            //TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));

            var margin = TextArea.Margins[BOOKMARK_MARGIN];
            margin.Width = 20;
            margin.Sensitive = true;
            margin.Type = MarginType.Symbol;
            margin.Mask = (1 << BOOKMARK_MARKER);
            //margin.Cursor = MarginCursor.Arrow;

            var marker = TextArea.Markers[BOOKMARK_MARKER];
            marker.Symbol = MarkerSymbol.Circle;
            marker.SetBackColor(IntToColor(0xFF003B));
            marker.SetForeColor(IntToColor(0x000000));
            marker.SetAlpha(100);
        }

        void InitCodeFolding()
        {
            // I disabled this because I personally find code folding to be complicated and unpleasant.
            /*
			TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));
			TextArea.SetFoldMarginHighlightColor(true, IntToColor(BACK_COLOR));

			// Enable code folding
			TextArea.SetProperty("fold", "1");
			TextArea.SetProperty("fold.compact", "1");

			// Configure a margin to display folding symbols
			TextArea.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
			TextArea.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
			TextArea.Margins[FOLDING_MARGIN].Sensitive = true;
			TextArea.Margins[FOLDING_MARGIN].Width = 20;

			// Set colors for all folding markers
			for (int i = 25; i <= 31; i++) {
				TextArea.Markers[i].SetForeColor(IntToColor(BACK_COLOR)); // styles for [+] and [-]
				TextArea.Markers[i].SetBackColor(IntToColor(FORE_COLOR)); // styles for [+] and [-]
			}

			// Configure folding markers with respective symbols
			TextArea.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
			TextArea.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
			TextArea.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
			TextArea.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
			TextArea.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
			TextArea.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
			TextArea.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

			// Enable automatic folding
			TextArea.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);
            */
        }

        void TextArea_MarginClick(object sender, MarginClickEventArgs e)
        {
            if (e.Margin == BOOKMARK_MARGIN)
            {
                // Do we have a marker for this line?
                const uint mask = (1 << BOOKMARK_MARKER);
                var line = TextArea.Lines[TextArea.LineFromPosition(e.Position)];
                if ((line.MarkerGet() & mask) > 0)
                {
                    // Remove existing bookmark
                    line.MarkerDelete(BOOKMARK_MARKER);
                }
                else
                {
                    // Add bookmark
                    line.MarkerAdd(BOOKMARK_MARKER);
                }
            }
        }

        public void InitDragDropFile() {

			TextArea.AllowDrop = true;
			TextArea.DragEnter += delegate(object sender, DragEventArgs e) {
				if (e.Data.GetDataPresent(DataFormats.FileDrop))
					e.Effect = DragDropEffects.Copy;
				else
					e.Effect = DragDropEffects.None;
			};
			TextArea.DragDrop += delegate(object sender, DragEventArgs e) {

				// get file drop
				if (e.Data.GetDataPresent(DataFormats.FileDrop)) {

					var a = (Array)e.Data.GetData(DataFormats.FileDrop);
					if (a != null) {

						var path = a.GetValue(0).ToString();

						LoadDataFromFile(path);

					}
				}
			};

		}

        void LoadDataFromFile(string path)
        {
            if (File.Exists(path))
            {
                TextArea.Text = File.ReadAllText(path);
                TextArea.SetSavePoint();
                FileName = path;
            }
        }

        void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckModifiedAndContinue())
                return;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                LoadDataFromFile(openFileDialog.FileName);
        }

        void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSearch();
        }

        void findDialogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFindDialog();
        }

        void findAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenReplaceDialog();
        }

        void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextArea.Cut();
        }

        void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextArea.Copy();
        }

        void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextArea.Paste();
        }

        void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextArea.SelectAll();
        }

        void selectLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = TextArea.Lines[TextArea.CurrentLine];
            TextArea.SetSelection(line.Position + line.Length, line.Position);
        }

        void clearSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextArea.SetEmptySelection(0);
        }

        void indentSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Indent();
        }

        void outdentSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Outdent();
        }

        void uppercaseSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Uppercase();
        }

        void lowercaseSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Lowercase();
        }

        void wordWrapToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            // toggle word wrap
            wordWrapItem.Checked = !wordWrapItem.Checked;
            TextArea.WrapMode = wordWrapItem.Checked ? WrapMode.Word : WrapMode.None;
        }

        void indentGuidesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // toggle indent guides
            indentGuidesItem.Checked = !indentGuidesItem.Checked;
            TextArea.IndentationGuides = indentGuidesItem.Checked ? IndentView.LookBoth : IndentView.None;
        }

        void hiddenCharactersToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // toggle view whitespace
            hiddenCharactersItem.Checked = !hiddenCharactersItem.Checked;
            TextArea.ViewWhitespace = hiddenCharactersItem.Checked ? WhitespaceMode.VisibleAlways : WhitespaceMode.Invisible;
        }

        void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        void zoom100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomDefault();
        }

        void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextArea.FoldAll(FoldAction.Contract);
        }

        void expandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextArea.FoldAll(FoldAction.Expand);
        }

        void Lowercase()
        {

            // save the selection
            var start = TextArea.SelectionStart;
            var end = TextArea.SelectionEnd;

            // modify the selected text
            TextArea.ReplaceSelection(TextArea.GetTextRange(start, end - start).ToLower());

            // preserve the original selection
            TextArea.SetSelection(start, end);
        }

        void Uppercase()
        {

            // save the selection
            var start = TextArea.SelectionStart;
            var end = TextArea.SelectionEnd;

            // modify the selected text
            TextArea.ReplaceSelection(TextArea.GetTextRange(start, end - start).ToUpper());

            // preserve the original selection
            TextArea.SetSelection(start, end);
        }

        void Indent()
        {
            // we use this hack to send "Shift+Tab" to scintilla, since there is no known API to indent,
            // although the indentation function exists. Pressing TAB with the editor focused confirms this.
            GenerateKeystrokes("{TAB}");
        }

        void Outdent()
        {
            // we use this hack to send "Shift+Tab" to scintilla, since there is no known API to outdent,
            // although the indentation function exists. Pressing Shift+Tab with the editor focused confirms this.
            GenerateKeystrokes("+{TAB}");
        }

        void GenerateKeystrokes(string keys)
        {
            HotKeyManager.Enable = false;
            TextArea.Focus();
            SendKeys.Send(keys);
            HotKeyManager.Enable = true;
        }

        void ZoomIn()
        {
            TextArea.ZoomIn();
        }

        void ZoomOut()
        {
            TextArea.ZoomOut();
        }        

        void ZoomDefault()
        {
            TextArea.Zoom = 0;
        }
        
        bool SearchIsOpen = false;

        void OpenSearch()
        {

            SearchManager.SearchBox = TxtSearch;
            SearchManager.TextArea = TextArea;

            if (!SearchIsOpen)
            {
                SearchIsOpen = true;
                InvokeIfNeeded(delegate ()
                {
                    PanelSearch.Visible = true;
                    TxtSearch.Text = SearchManager.LastSearch;
                    TxtSearch.Focus();
                    TxtSearch.SelectAll();
                });
            }
            else
            {
                InvokeIfNeeded(delegate ()
                {
                    TxtSearch.Focus();
                    TxtSearch.SelectAll();
                });
            }
        }
        void CloseSearch()
        {
            if (SearchIsOpen)
            {
                SearchIsOpen = false;
                InvokeIfNeeded(delegate ()
                {
                    PanelSearch.Visible = false;
                    //CurBrowser.GetBrowser().StopFinding(true);
                });
            }
        }

        void BtnClearSearch_Click(object sender, EventArgs e)
        {
            CloseSearch();
        }

        void BtnPrevSearch_Click(object sender, EventArgs e)
        {
            SearchManager.Find(false, false);
        }
        void BtnNextSearch_Click(object sender, EventArgs e)
        {
            SearchManager.Find(true, false);
        }
        void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchManager.Find(true, true);
        }

        void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (HotKeyManager.IsHotkey(e, Keys.Enter))
            {
                SearchManager.Find(true, false);
            }
            if (HotKeyManager.IsHotkey(e, Keys.Enter, true) || HotKeyManager.IsHotkey(e, Keys.Enter, false, true))
            {
                SearchManager.Find(false, false);
            }
        }
        
        void OpenFindDialog()
        {
            throw new NotImplementedException();
		}

        void OpenReplaceDialog()
        {
            throw new NotImplementedException();
        }

        public static Color IntToColor(int rgb) {
			return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
		}

		public void InvokeIfNeeded(Action action) {
			if (InvokeRequired) {
				BeginInvoke(action);
			} else {
				action.Invoke();
			}
		}

        void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckModifiedAndContinue())
                return;
            TextArea.Text = Client.NewSnippet();
            FileName = "";
        }

        public bool SaveAs()
        {
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return false;
            FileName = openFileDialog.FileName;
            TextArea.SetSavePoint();
            return true;
        }

        public bool Save()
        {
            if (!File.Exists(FileName))
            {
                if (!SaveAs())
                    return false;
            }
            try
            {
                File.WriteAllText(FileName, TextArea.Text);
                TextArea.SetSavePoint();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error occured " + e.Message);
                return false;
            }
        }

        public bool CheckModifiedAndContinue()
        {
            if (!TextArea.Modified) return true;
            var r = MessageBox.Show("Data has been modified, save?", "Save Text?", MessageBoxButtons.YesNoCancel);
            if (r == DialogResult.Cancel)
                return false;
            if (r == DialogResult.Yes)
                Save();
            return true;
        }

        void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var printer = new Printing(TextArea)
            {
                PageSettings = new PageSettings
                {
                    ColorMode = PageSettings.PrintColorMode.BlackOnWhite
                }
            };
            printer.Print();
        }

        void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var box = new AboutBox1();
            box.ShowDialog();
        }

        bool Compile()
        {
            errorListTextBox.Clear();
            if (!CheckModifiedAndContinue())
                return false;
            var errors = Client.Compile(FileName);
            errorListTextBox.Lines = errors.ToArray();
            return errors.Count == 0;
        }

        void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Compile();
        }

        void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Compile())
                Client.Run(FileName);
        }

        private void ScriptEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CheckModifiedAndContinue();
        }
    }
}
