using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;

namespace WpfApp4
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly TicTacToeGame game;

        private string startMessage;
        private Difficulty selectedDifficulty;

        public ObservableCollection<CellViewModel> Cells { get; }

        public ICommand MakeMoveCommand { get; }
        public ICommand NewGameCommand { get; }

        public string StartMessage
        {
            get => startMessage;
            set
            {
                if (startMessage != value)
                {
                    startMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public Difficulty SelectedDifficulty
        {
            get => selectedDifficulty;
            set
            {
                if (selectedDifficulty != value)
                {
                    selectedDifficulty = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainViewModel()
        {
            game = new TicTacToeGame();

            Cells = new ObservableCollection<CellViewModel>();

            SelectedDifficulty = Difficulty.Smart;

            MakeMoveCommand = new RelayCommand(
                p => MakeMove(p));

            NewGameCommand = new RelayCommand(
                p => NewGame());

            NewGame();
        }

        private void NewGame()
        {
            game.NewGame();

            Cells.Clear();

            for (int row = 0; row < TicTacToeGame.BoardSize; row++)
            {
                for (int col = 0; col < TicTacToeGame.BoardSize; col++)
                {
                    Cells.Add(
                        new CellViewModel(row, col));
                }
            }

            startMessage = "Ваш хід — X";
        }

        private void MakeMove(object para)
        {
            if (para is not CellViewModel cell)
                return;

            bool success = game.MakeMove(cell.Row, cell.Column, CellState.X);

            if (!success)
                return;

            UpdateCells();

            GameResult result = game.GetResult();

            if (result != GameResult.Continue)
            {
                ShowResult(result);
                return;
            }

            var computerMove =
                game.ChooseComputerMove(SelectedDifficulty);

            if (computerMove.HasValue)
            {
                game.MakeMove(
                    computerMove.Value.Row,
                    computerMove.Value.Col,
                    CellState.O);
            }

            UpdateCells();

            result = game.GetResult();

            if (result != GameResult.Continue)
            {
                ShowResult(result);
                return;
            }

            startMessage = "Ваш хід — X";
        }

        private void UpdateCells()
        {
            foreach (CellViewModel cell in Cells)
            {
                CellState state =
                    game.Board[cell.Row, cell.Column];

                if (state == CellState.X)
                    cell.Value = "X";
                else if (state == CellState.O)
                    cell.Value = "O";
                else
                    cell.Value = "";
            }
        }

        private void ShowResult(GameResult result)
        {
            switch (result)
            {
                case GameResult.XWins:
                    MessageBox.Show("Ви перемогли!", "TicTacToe");
                    break;

                case GameResult.OWins:
                    MessageBox.Show("Комп'ютер переміг!", "TicTacToe");
                    break;

                case GameResult.Draw:
                    MessageBox.Show("Нічия!", "TicTacToe");
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}