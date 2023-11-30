using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using RoslynPad.Editor;
using RoslynPad.Roslyn;
using RoslynPadReplSample;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace Ara3D.ScriptPaint
{
    /// <summary>
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window
    {
        private readonly ObservableCollection<DocumentViewModel> _documents;
        private readonly RoslynHost _host;

        public EditorWindow()
        {
            InitializeComponent();
            _documents = new ObservableCollection<DocumentViewModel>();
            Items.ItemsSource = _documents;

            _host = new CustomRoslynHost(additionalAssemblies: new[]
            {
                Assembly.Load("RoslynPad.Roslyn.Windows"),
                Assembly.Load("RoslynPad.Editor.Windows")
            }, RoslynHostReferences.NamespaceDefault.With(assemblyReferences: new[]
            {
                typeof(object).Assembly,
                typeof(System.Text.RegularExpressions.Regex).Assembly,
                typeof(Enumerable).Assembly,
            }));

            AddNewDocument();
        }
        private void AddNewDocument(DocumentViewModel? previous = null)
        {
            _documents.Add(new DocumentViewModel(_host, previous));
        }

        private async void OnItemLoaded(object sender, EventArgs e)
        {
            var editor = (RoslynCodeEditor)sender;
            editor.Loaded -= OnItemLoaded;
            editor.Focus();

            var viewModel = (DocumentViewModel)editor.DataContext;
            var workingDirectory = Directory.GetCurrentDirectory();

            var previous = viewModel.LastGoodPrevious;
            if (previous != null)
            {
                editor.CreatingDocument += (o, args) =>
                {
                    args.DocumentId = _host.AddRelatedDocument(previous.Id, new DocumentCreationArgs(
                    args.TextContainer, workingDirectory, SourceCodeKind.Script, args.ProcessDiagnostics,
                        args.TextContainer.UpdateText));
                };
            }

            var documentId = await editor.InitializeAsync(_host, new ClassificationHighlightColors(),
                workingDirectory, string.Empty, SourceCodeKind.Script).ConfigureAwait(true);

            viewModel.Initialize(documentId);
        }

        private async void OnEditorKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var editor = (RoslynCodeEditor)sender;
                if (editor.IsCompletionWindowOpen)
                {
                    return;
                }

                e.Handled = true;

                var viewModel = (DocumentViewModel)editor.DataContext;
                if (viewModel.IsReadOnly) return;

                viewModel.Text = editor.Text;
                if (await viewModel.TrySubmitAsync().ConfigureAwait(true))
                {
                    AddNewDocument(viewModel);
                }
            }
        }

        // TODO: workaround for GetSolutionAnalyzerReferences bug (should be added once per Solution)
        private class CustomRoslynHost : RoslynHost
        {
            private bool _addedAnalyzers;

            public CustomRoslynHost(IEnumerable<Assembly>? additionalAssemblies = null, RoslynHostReferences? references = null, ImmutableArray<string>? disabledDiagnostics = null) : base(additionalAssemblies, references, disabledDiagnostics)
            {
            }

            protected override IEnumerable<AnalyzerReference> GetSolutionAnalyzerReferences()
            {
                if (!_addedAnalyzers)
                {
                    _addedAnalyzers = true;
                    return base.GetSolutionAnalyzerReferences();
                }

                return Enumerable.Empty<AnalyzerReference>();
            }
        }
    }
}
