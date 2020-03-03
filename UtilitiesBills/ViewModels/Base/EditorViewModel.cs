using System;

namespace UtilitiesBills.ViewModels.Base
{
    public enum EditorMode
    {
        View,
        Edit,
        Create
    }

    public abstract class EditorViewModel : BaseViewModel
    {
        private EditorMode _editorMode = EditorMode.View;
        protected Action OnEditorModeChanged { get; set; }

        /// <summary>
        /// Режим редактора. Default = EditorMode.View
        /// </summary>
        public EditorMode EditorMode 
        { 
            get => _editorMode; 
            set => SetProperty(ref _editorMode, value, OnEditorModeChanged); 
        }
    }
}
