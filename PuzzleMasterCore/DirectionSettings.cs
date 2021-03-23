using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PuzzleMasterCore
{
    class DirectionSettings : INotifyPropertyChanged
    {
        private bool rightwards = true;
        private bool leftwards = true;
        private bool downwards = true;
        private bool upwards = true;

        private bool diagonalRightUpwards;
        private bool diagonalRightDownwards;
        private bool diagonalLeftUpwards;
        private bool diagonalLeftDownwards;

        private List<Directions> directions = new List<Directions>();

        public DirectionSettings()
        {
            if(rightwards)
                directions.Add(Directions.Right);
            if (leftwards)
                directions.Add(Directions.Left);
            if (downwards)
                directions.Add(Directions.Down);
            if (upwards)
                directions.Add(Directions.Up);
            if (diagonalRightUpwards)
                directions.Add(Directions.Diagonal_Right_Up);
            if (diagonalRightDownwards)
                directions.Add(Directions.Diagonal_Right_Down);
            if (diagonalLeftUpwards)
                directions.Add(Directions.Diagonal_Left_Up);
            if (diagonalLeftDownwards)
                directions.Add(Directions.Diagonal_Left_Down);
        }

        #region props
        public List<Directions> SelectedDirections
        {
            get
            {
                return directions;
            }
        }
        public bool Rightwards
        {
            get { return rightwards; }
            set
            {
                rightwards = value;

                //if value is changed to true the direction ist added to the list, otherwise it is removed
                if (value)
                    directions.Add(Directions.Right);
                else
                    directions.Remove(Directions.Right);

                OnPropertyChanged(nameof(Rightwards));
            }
        }
        public bool Leftwards
        {
            get { return leftwards; }
            set
            {
                leftwards = value;

                //if value is changed to true the direction ist added to the list, otherwise it is removed
                if (value)
                    directions.Add(Directions.Left);
                else
                    directions.Remove(Directions.Left);

                OnPropertyChanged(nameof(Leftwards));
            }
        }
        public bool Downwards
        {
            get { return downwards; }
            set
            {
                downwards = value;

                //if value is changed to true the direction ist added to the list, otherwise it is removed
                if (value)
                    directions.Add(Directions.Down);
                else
                    directions.Remove(Directions.Down);

                OnPropertyChanged(nameof(Downwards));
            }
        }
        public bool Upwards
        {
            get { return upwards; }
            set
            {
                upwards = value;

                //if value is changed to true the direction ist added to the list, otherwise it is removed
                if (value)
                    directions.Add(Directions.Up);
                else
                    directions.Remove(Directions.Up);

                OnPropertyChanged(nameof(Upwards));
            }
        }
        public bool DiagonalRightUpwards
        {
            get { return diagonalRightUpwards; }
            set
            {
                diagonalRightUpwards = value;

                //if value is changed to true the direction ist added to the list, otherwise it is removed
                if (value)
                    directions.Add(Directions.Diagonal_Right_Up);
                else
                    directions.Remove(Directions.Diagonal_Right_Up);

                OnPropertyChanged(nameof(DiagonalRightUpwards));
            }
        }
        public bool DiagonalRightDownwards
        {
            get { return diagonalRightDownwards; }
            set
            {
                diagonalRightDownwards = value;

                //if value is changed to true the direction ist added to the list, otherwise it is removed
                if (value)
                    directions.Add(Directions.Diagonal_Right_Down);
                else
                    directions.Remove(Directions.Diagonal_Right_Down);

                OnPropertyChanged(nameof(DiagonalRightDownwards));
            }
        }
        public bool DiagonalLeftUpwards
        {
            get { return diagonalLeftUpwards; }
            set
            {
                diagonalLeftUpwards = value;

                //if value is changed to true the direction ist added to the list, otherwise it is removed
                if (value)
                    directions.Add(Directions.Diagonal_Left_Up);
                else
                    directions.Remove(Directions.Diagonal_Left_Up);

                OnPropertyChanged(nameof(DiagonalLeftUpwards));
            }
        }

        public bool DiagonalLeftDownwards
        {
            get { return diagonalLeftDownwards; }
            set
            {
                diagonalLeftDownwards = value;

                //if value is changed to true the direction ist added to the list, otherwise it is removed
                if (value)
                    directions.Add(Directions.Diagonal_Left_Down);
                else
                    directions.Remove(Directions.Diagonal_Left_Down);

                OnPropertyChanged(nameof(DiagonalLeftDownwards));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
